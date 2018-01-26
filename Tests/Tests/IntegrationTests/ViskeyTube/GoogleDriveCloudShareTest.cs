using System.IO;
using System.Linq;
using ViskeyTube.CloudShare;
using Xunit;

namespace Tests.Tests.IntegrationTests.ViskeyTube
{
    public class GoogleDriveCloudShareTest : IntegrationTest
    {
        private readonly GoogleDriveCloudShare googleDriveCloudShare;

        public GoogleDriveCloudShareTest()
        {
            googleDriveCloudShare = new GoogleDriveCloudShare(credentialsService.GoogleApiKey, 
                                                              credentialsService.GoogleClientSecret, 
                                                              container.Get<IDriveQueryBuilderFactory>());
        }

        [Fact]
        public void AbleToDownloadFile()
        {
            var bytes = googleDriveCloudShare.DownloadFile("1XNz2OF6xYiKPObWvBpczLx13Xossk6kR");
            Assert.True(bytes.Length > 0);
            File.WriteAllBytes("downloadResult.png", bytes);
        }

        [Fact]
        public void AbleToGetFilesFromFolder()
        {
            var fileNames = googleDriveCloudShare.GetFiles("17K6Nj556UL2ylNYzh2lXPpYzq7Bif8tl");
            Assert.True(fileNames.Length > 0);
        }

        [Fact]
        public void AbleToMoveFileToYouTube()
        {
            var files = googleDriveCloudShare.GetFiles("17K6Nj556UL2ylNYzh2lXPpYzq7Bif8tl");
            var videoFile = files.Single(x => x.Name.Contains("29 декабря 2017"));
            Assert.True(result.Success, result.Exception?.Message);
        }
    }
}