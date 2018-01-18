using System.IO;
using System.Linq;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace ViskeyTube.CloudShare
{
    public class GoogleDriveCloudShare : ICloudShare
    {
        private readonly string apiKey;
        private readonly IDriveQueryBuilderFactory driveQueryBuilderFactory;

        private DriveService NewDriveService() => new DriveService(new BaseClientService.Initializer
        {
            ApplicationName = "Treller",
            ApiKey = apiKey

        });

        public GoogleDriveCloudShare(
            string apiKey,
            IDriveQueryBuilderFactory driveQueryBuilderFactory
            )
        {
            this.apiKey = apiKey;
            this.driveQueryBuilderFactory = driveQueryBuilderFactory;
        }

        public byte[] DownloadFile(string fileId)
        {
            using (var driveService = NewDriveService())
            {
                using (var memoryStream = new MemoryStream())
                {
                    driveService.Files.Get(fileId).Download(memoryStream);
                    return memoryStream.ToArray();
                }

            }
        }

        public string[] GetFiles(string folderId)
        {
            using (var driveService = NewDriveService())
            {
                var request = driveService.Files.List();
                request.Q = driveQueryBuilderFactory.Create().InFolder(folderId).ToQueryString();
                var result = request.Execute();
                return result.Files.Select(x => x.Name).ToArray();
            }
        }
    }
}