using System.IO;
using TaskManagerClient.CredentialServiceAbstractions;
using ViskeyTube.CloudShare;
using Xunit;

namespace Tests.Tests.IntegrationTests.ViskeyTube
{
    public class GoogleDriveCloudShareTest : IntegrationTest
    {
        private readonly GoogleDriveCloudShare googleDriveCloudShare;

        public GoogleDriveCloudShareTest()
        {
            googleDriveCloudShare = new GoogleDriveCloudShare(container.Get<IGoogleApiCredentialService>().GoogleApiKey);
        }

        [Fact]
        public void AbleToDownloadFile()
        {
            var bytes = googleDriveCloudShare.DownloadFile("1XNz2OF6xYiKPObWvBpczLx13Xossk6kR");
            Assert.True(bytes.Length > 0);
            File.WriteAllBytes("downloadResult.png", bytes);
        }
    }
}