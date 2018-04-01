using System.Linq;
using ViskeyTube.CloudShare;
using Xunit;

namespace Tests.Tests.IntegrationTests.ViskeyTube
{
    public class SlowGoogleDriveCloudShareTest : GoogleDriveCloudShareTest
    {
        //[Fact] Залить фейковые видос маленького размера
        public void AbleToMoveFileToYouTube()
        {
            var files = googleDriveCloudShare.GetFiles(BillingGooglePhotoFolderId);
            var videoFile = files.Single(x => x.Name.Contains("29 декабря 2017"));
            var bytes = googleDriveCloudShare.DownloadFile(videoFile.FileId);

            var videoToUpload = new VideoMeta
            {
                Title = "TEST - " + videoFile.Name,
                Description = "Test NightWhiskey"
            };

            var result = googleDriveCloudShare.UploadToYouTube(bytes, videoToUpload, BillingChannelId);
            Assert.True(result.Success, result.Exception?.Message);
        }
    }
}