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
using ViskeyTube.CloudShare;

namespace ViskeyTube.RepositoryLayer.Google
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
                    new[] { YouTubeService.Scope.Youtube, YouTubeService.Scope.YoutubeUpload, 
                        YouTubeService.Scope.Youtubepartner, "https://www.googleapis.com/auth/plus.login", "https://www.googleapis.com/auth/userinfo.email" },
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

        public DriveFileDto[] GetFiles(string folderId)
        {
            using (var driveService = CreateDriveService())
            {
                var request = driveService.Files.List();
                request.Q = driveQueryBuilderFactory.Create().InFolder(folderId).ToQueryString();
                var result = request.Execute();
                request.Fields = "createdTime";
                return result.Files
                    .Select(x => new DriveFileDto
                    {
                        Name = x.Name,
                        FileId = x.Id
                    })
                    .ToArray();
            }
        }

        private const string YoutubeVideoResourceKind = "youtube#video";
        private const string YoutubeSnippetPart = "snippet";

        public UploadResultDto UploadToYouTube(byte[] fileBytes, VideoMeta videoMeta, string channelId)
        {
            using (var youTubeService = CreateYouTubeService())
            {
                var video = new Video
                {
                    Snippet = new VideoSnippet
                    {
                        Title = videoMeta.Title,
                        Description = videoMeta.Description,
                        Tags = new[] { "nightwhiskey", "whiskeytube", "konturbilling" },
                        ChannelId = channelId,
                        CategoryId = "22", // See https://developers.google.com/youtube/v3/docs/videoCategories/list
                    },
                    Status = new VideoStatus
                    {
                        PrivacyStatus = "public", // or "private" or "public"
                    },
                };

                using (var memoryStream = new MemoryStream(fileBytes))
                {
                    var request = youTubeService.Videos.Insert(video, "snippet,status", memoryStream, "video/*");

                    string videoId = null;
                    request.ResponseReceived += x => videoId = x.Id;
                    var progress = request.Upload();

                    return new UploadResultDto
                    {
                        Exception = progress.Exception,
                        Success = progress.Status == UploadStatus.Completed,
                        VideoId = videoId
                    };
                }
            }
        }

        public void AddVideoToPlayList(string videoId, string playlistId)
        {
            using (var youTubeService = CreateYouTubeService())
            {
                var playListItem = new PlaylistItem
                {
                    Snippet = new PlaylistItemSnippet
                    {
                        ResourceId = new ResourceId
                        {
                            Kind = YoutubeVideoResourceKind,
                            VideoId = videoId
                        }
                    }
                };

                var request = youTubeService.PlaylistItems.Insert(playListItem, YoutubeSnippetPart);
                request.Execute();
            }
        }

        public string[] GetMyChannels()
        {
            using (var youTubeService = CreateYouTubeService())
            {
                var channelsRequest = youTubeService.Channels.List(YoutubeSnippetPart);
                var channels = channelsRequest.Mine().Execute().Items.ToArray();
                return channels.Select(x => x.Id).ToArray();
            }
        }



        public YoutubeVideoDto[] GetVideos(string channelId)
        {
            using (var youTubeService = CreateYouTubeService())
            {
                var request = youTubeService.Videos.List("snippet,fileDetails");
                var response = request.Execute();
                return response.Items
                    .Where(x => x.Snippet.ChannelId == channelId)
                    .Where(x => x.FileDetails.FileSize.HasValue)
                    .Select(x => new YoutubeVideoDto
                    {
                        VideoId = x.Id,
                        Name = x.Snippet.Title,
                        //PublishTime = x.Snippet.PublishedAt,
                        //FileSize = UnsafeConvertFromULong(x.FileDetails.FileSize.Value),
                        //FileName = x.FileDetails.FileName,
                    })
                    .ToArray();
            }
        }

        private static long UnsafeConvertFromULong(ulong u)
        {
            return u > long.MaxValue ? throw new Exception($"Cant convert ulong {u} to long") : (long)u;
        }
    }
}