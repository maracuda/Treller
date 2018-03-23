using System.IO;
using System.Net;
using Google;
using ViskeyTube.RepositoryLayer.Google;
using Xunit;

namespace Tests.Tests.IntegrationTests.ViskeyTube
{
    public class GoogleDriveCloudShareTest : IntegrationTest
    {
        protected readonly ICloudShare googleDriveCloudShare;

        protected const string BillingChannelId = "UCiGKUGNeK8-KHPpRqxZoYcw";
        protected const string BillingGooglePhotoFolderId = "17K6Nj556UL2ylNYzh2lXPpYzq7Bif8tl";

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
            var fileNames = googleDriveCloudShare.GetFiles(BillingGooglePhotoFolderId);
            Assert.True(fileNames.Length > 0);
        }

        [Fact]
        public void AbleToGetMyVideos()
        {
            var googleApiException = Assert.Throws<GoogleApiException>(() => googleDriveCloudShare.GetVideos(BillingChannelId));
            Assert.Equal(HttpStatusCode.Forbidden, googleApiException.HttpStatusCode);            
        }

        [Fact]
        public void AbleToMoveVideoToPlaylist()
        {
            var googleApiException = Assert.Throws<GoogleApiException>(() => googleDriveCloudShare.AddVideoToPlayList("MmBhsdxoCYE", "PLsUe9ECHg2WapJxV0CQVUCw_dy9oWomSX"));
            Assert.Equal(HttpStatusCode.Forbidden, googleApiException.HttpStatusCode);
        }

        [Fact]
        public void AbleToGetMyChannels()
        {
            var googleApiException = Assert.Throws<GoogleApiException>(() => googleDriveCloudShare.GetMyChannels());
            Assert.Equal(HttpStatusCode.Forbidden, googleApiException.HttpStatusCode);
        }
    }
}