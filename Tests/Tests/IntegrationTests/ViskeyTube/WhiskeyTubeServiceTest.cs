using System;
using System.Collections.Generic;
using System.Linq;
using Serialization;
using ViskeyTube.CloudShare;
using ViskeyTube.RepositoryLayer.Wiki;
using Xunit;

namespace Tests.Tests.IntegrationTests.ViskeyTube
{
    public class WhiskeyTubeServiceTest : GoogleDriveCloudShareTest
    {
        private readonly WhiskeyTubeService whiskeyTubeService;

        public WhiskeyTubeServiceTest()
        {
            var wikiClient = new WikiClient(new JsonSerializer(),
                credentialsService.GetWikiCredentials().AuthHeader);
            whiskeyTubeService = new WhiskeyTubeService(googleDriveCloudShare,
                new VideoToUploadProvider(wikiClient), wikiClient);
        }

        [Fact]
        public void AbleToSyncByGoogle()
        {
            //var dates = new[] { new DateTime(2017, 12, 29), new DateTime(2018, 03, 02), new DateTime(2018, 01, 12), new DateTime(2018, 02, 16) };
            var dates = Array.Empty<DateTime>();
            var videos = new List<Tuple<string, DateTime>>();
            foreach (var date in dates)
            {
                var uploads = whiskeyTubeService.SyncByGoogleDrive(date, date, "156696999", BillingGooglePhotoFolderId, BillingChannelId);
                videos.AddRange(uploads.Where(x => x.Success).Select(x => new Tuple<string, DateTime>(x.VideoId, date)));
            }

            Assert.Equal(dates.Length, videos.Count);
        }
    }
}