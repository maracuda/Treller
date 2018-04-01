using System;
using System.Linq;
using ViskeyTube.CloudShare;
using ViskeyTube.DomainLayer;
using ViskeyTube.RepositoryLayer.Google;
using ViskeyTube.RepositoryLayer.Wiki;

namespace ViskeyTube.ApplicationLayer
{
    public class WhiskeyTubeService : IWhiskeyTubeService
    {
        private readonly ICloudShare cloudShare;
        private readonly IWikiClient wikiClient;

        public WhiskeyTubeService(ICloudShare cloudShare, IWikiClient wikiClient)
        {
            this.cloudShare = cloudShare;
            this.wikiClient = wikiClient;
        }

        public UploadResultDto[] SyncByWiki(DateTime inclusiveFromDate, DateTime inclusiveEndDate, string wikiArchivePageId, string driveFolderId, string youtubeChannelId)
        {
            var pages = wikiClient.GetChildren(wikiArchivePageId);
            var wikiPages = pages
                .Select(x => new WhiskeyWikiPage(x))
                .Where(x => x.Date.HasValue)
                .Where(x => x.Date >= inclusiveFromDate && x.Date <= inclusiveEndDate)
                .ToArray();

            var driveFiles = cloudShare.GetFiles(driveFolderId)
                .Select(x => new WhiskeyDriveFile(x))
                .Where(x => x.Date.HasValue)
                .ToArray();

            var videos = wikiPages.Select(p =>
            {
                var driveFile = driveFiles.FirstOrDefault(x => p.Date.Value == x.Date.Value);
                return new WhiskeyVideo(driveFile, p, cloudShare, wikiClient);
            }).ToArray();

            return videos
                .Where(x => x.ReadyToUpload)
                .Select(x => x.Upload(youtubeChannelId))
                .ToArray();
        }
    }
}