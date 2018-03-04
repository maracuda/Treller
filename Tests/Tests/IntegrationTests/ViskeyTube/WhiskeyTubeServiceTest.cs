using System;
using System.Linq;
using Serialization;
using ViskeyTube.CloudShare;
using ViskeyTube.Wiki;
using Xunit;

namespace Tests.Tests.IntegrationTests.ViskeyTube
{
    public class WhiskeyTubeServiceTest : GoogleDriveCloudShareTest
    {
        private readonly WhiskeyTubeService whiskeyTubeService;

        public WhiskeyTubeServiceTest()
        {
            whiskeyTubeService = new WhiskeyTubeService(googleDriveCloudShare,
                new VideoToUploadProvider(new WikiClient(new JsonSerializer(),
                    credentialsService.GetWikiCredentials().AuthHeader)));
        }

        [Fact]
        public void AbleToSyncByWiki()
        {
            var uploads = whiskeyTubeService.SyncByWiki(new DateTime(2017, 11, 24), new DateTime(2017, 11, 24),
                "156696999",
                BillingGooglePhotoFolderId, BillingChannelId);
            Assert.Equal(1, uploads.Length);
            Assert.True(uploads.Single().Success);
            Assert.NotEmpty(uploads.Single().VideoId);
        }
    }
}