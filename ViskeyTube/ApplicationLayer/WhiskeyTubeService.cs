using System;
using System.Linq;
using ViskeyTube.CloudShare;
using ViskeyTube.DomainLayer;
using ViskeyTube.DomainLayer.Common;
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
                .Select(x => new WhiskeyWikiPage(x, () => wikiClient.GetPage(x.Id).Body.View.Value))
                .ToArray();

            var driveFiles = cloudShare.GetFiles(driveFolderId)
                .Select(x => new WhiskeyDriveFile(x, () => cloudShare.DownloadFile(x.FileId)))
                .ToArray();

            var videoSet = new WhiskeyVideoSet()
                .AddWikiPages(wikiPages)
                .AddDriveFiles(driveFiles);

            var videos = videoSet.GetVideosToUpload(inclusiveFromDate, inclusiveEndDate);

            return videos
                .Select(x => Upload(x, youtubeChannelId))
                .ToArray();
        }

        private UploadResultDto Upload(WhiskeyVideo video, string channelId, string playlistId = null)
        {
            var videoMeta = new VideoMeta
            {
                Title = video.Title.SafeSubString(0, 100),
                Description = $"{video.Title}\r\n\r\n{video.Description.FromHtml()}"
            };

            var uploadResult = cloudShare.UploadToYouTube(video.VideoBytes, videoMeta, channelId);

            if (uploadResult.Success && !string.IsNullOrWhiteSpace(playlistId))
            {
                cloudShare.AddVideoToPlayList(uploadResult.VideoId, playlistId);
            }

            if (uploadResult.Success)
            {
                var newPage = video.MarkUploaded();
                wikiClient.UpdateTitleAndGetNewPage(newPage.Id, newPage.Title);
            }

            return uploadResult;
        }

    }
}