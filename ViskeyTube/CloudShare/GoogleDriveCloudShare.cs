using System.IO;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace ViskeyTube.CloudShare
{
    public class GoogleDriveCloudShare : ICloudShare
    {
        private readonly string apiKey;

        public GoogleDriveCloudShare(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public byte[] DownloadFile(string fileId)
        {
            using (var driveService = new DriveService(new BaseClientService.Initializer
            {
                ApplicationName = "Treller",
                ApiKey = apiKey

            }))
            {
                using (var memoryStream = new MemoryStream())
                {
                    driveService.Files.Get(fileId).Download(memoryStream);
                    return memoryStream.ToArray();
                }
                    
            }
        }
    }
}