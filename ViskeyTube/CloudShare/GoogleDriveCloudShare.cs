using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace ViskeyTube.CloudShare
{
    public class GoogleDriveCloudShare : ICloudShare
    {
        private readonly string apiKey;
        private readonly string clientSecret;
        private readonly IDriveQueryBuilderFactory driveQueryBuilderFactory;

        public GoogleDriveCloudShare(
            string apiKey,
            string clientSecret,
            IDriveQueryBuilderFactory driveQueryBuilderFactory
        )
        {
            this.apiKey = apiKey;
            this.clientSecret = clientSecret;
            this.driveQueryBuilderFactory = driveQueryBuilderFactory;
        }

        private DriveService CreateDriveService() => new DriveService(new BaseClientService.Initializer
        {
            ApplicationName = "Treller",
            ApiKey = apiKey
        });

        private YouTubeService CreateYouTubeService()
        {
            UserCredential credential;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(clientSecret)))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.YoutubeUpload, "https://www.googleapis.com/auth/plus.login" },
                    "billing.kontur",
                    CancellationToken.None
                ).Result;
            }

            return new YouTubeService(new BaseClientService.Initializer
            {
                ApplicationName = "Treller",
                HttpClientInitializer = credential
            });
        }

        public byte[] DownloadFile(string fileId)
        {
            using (var driveService = CreateDriveService())
            {
                using (var memoryStream = new MemoryStream())
                {
                    driveService.Files.Get(fileId).Download(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public DriveFile[] GetFiles(string folderId)
        {
            using (var driveService = CreateDriveService())
            {
                var request = driveService.Files.List();
                request.Q = driveQueryBuilderFactory.Create().InFolder(folderId).ToQueryString();
                var result = request.Execute();
                return result.Files
                    .Select(x => new DriveFile
                    {
                        Name = x.Name,
                        FileId = x.Id
                    })
                    .ToArray();
            }
        }

        public UploadResult MoveToYouTube(string fileId)
        {
            using (var driveService = CreateDriveService())
            using (var youTubeService = CreateYouTubeService())
            {
                var file = driveService.Files.Get(fileId);

                var fileInfo = file.Execute();
                var video = new Video
                {
                    Snippet = new VideoSnippet
                    {
                        Title = fileInfo.Name,
                        Description = "Night Whiskey",
                        Tags = new[] { "nightwhiskey", "whiskeytube", "konturbilling" },
                        CategoryId = "22" // See https://developers.google.com/youtube/v3/docs/videoCategories/list
                    },
                    Status = new VideoStatus
                    {
                        PrivacyStatus = "public" // or "private" or "public"
                    }
                };

                byte[] bytes;
                using (var memoryStream = new MemoryStream())
                {
                    file.Download(memoryStream);
                    bytes = memoryStream.ToArray();
                }

                using (var memoryStream = new MemoryStream(bytes))
                {
                    var request = youTubeService.Videos.Insert(video, "snippet,status", memoryStream, "video/*");
                    request.ProgressChanged += videosInsertRequest_ProgressChanged;
                    request.ResponseReceived += videosInsertRequest_ResponseReceived;
                    var progress = request.Upload();

                    return new UploadResult
                    {
                        Exception = progress.Exception,
                        Success = progress.Status == UploadStatus.Completed
                    };
                }
            }
        }

        void videosInsertRequest_ProgressChanged(IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    Console.WriteLine("{0} bytes sent.", progress.BytesSent);
                    break;

                case UploadStatus.Failed:
                    Console.WriteLine("An error prevented the upload from completing.\n{0}", progress.Exception);
                    break;
            }
        }

        void videosInsertRequest_ResponseReceived(Video video)
        {
            Console.WriteLine("Video id '{0}' was successfully uploaded.", video.Id);
        }
    }
}