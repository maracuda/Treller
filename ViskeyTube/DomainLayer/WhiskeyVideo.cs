using ViskeyTube.CloudShare;
using ViskeyTube.DomainLayer.Common;
using ViskeyTube.RepositoryLayer.Google;
using ViskeyTube.RepositoryLayer.Wiki;

namespace ViskeyTube.DomainLayer
{
    public class WhiskeyVideo
    {
        private readonly WhiskeyDriveFile driveFile;
        private readonly WhiskeyWikiPage wikiPage;
        private readonly ICloudShare cloudShare;
        private readonly IWikiClient wikiClient;

        public WhiskeyVideo(WhiskeyDriveFile driveFile, WhiskeyWikiPage wikiPage, ICloudShare cloudShare, IWikiClient wikiClient)
        {
            this.driveFile = driveFile;
            this.wikiPage = wikiPage;
            this.cloudShare = cloudShare;
            this.wikiClient = wikiClient;
        }

        public bool ReadyToUpload => driveFile != null && wikiPage != null && !wikiPage.HasUploadedLabel;

        public UploadResultDto Upload(string channelId, string playlistId = null)
        {
            var pageBody = wikiPage.GetPageBody(wikiClient);
            var videoMeta = new VideoMeta
            {
                Title = wikiPage.Title.SafeSubString(0, 100),
                Description = $"{wikiPage.Title}\r\n\r\n{pageBody}"
            };

            var videoBytes = driveFile.GetFileBody(cloudShare);
            var uploadResult = cloudShare.UploadToYouTube(videoBytes, videoMeta, channelId);

            if (uploadResult.Success && !string.IsNullOrWhiteSpace(playlistId))
            {
                cloudShare.AddVideoToPlayList(uploadResult.VideoId, playlistId);
            }

            if (uploadResult.Success)
            {
                wikiPage.MarkUploaded(wikiClient);
            }

            return uploadResult;
        }
    }
}