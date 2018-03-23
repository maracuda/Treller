using System;
using System.Collections.Generic;
using System.Linq;
using ViskeyTube.Common;
using ViskeyTube.RepositoryLayer;
using ViskeyTube.RepositoryLayer.Google;
using ViskeyTube.RepositoryLayer.Wiki;

namespace ViskeyTube.CloudShare
{
    public class WhiskeyTubeService : IWhiskeyTubeService
    {
        private readonly ICloudShare cloudShare;
        private readonly IVideoToUploadProvider videoToUploadProvider;
        private readonly IWikiClient wikiClient;

        public WhiskeyTubeService(ICloudShare cloudShare, IVideoToUploadProvider videoToUploadProvider, IWikiClient wikiClient)
        {
            this.cloudShare = cloudShare;
            this.videoToUploadProvider = videoToUploadProvider;
            this.wikiClient = wikiClient;
        }

        public UploadResult[] SyncByGoogleDrive(string folderId, string channelId, string playlistId)
        {
            var files = cloudShare.GetFiles(folderId);
            var existedVideos = cloudShare.GetVideos(channelId);
            //todo: равенство скорее всего не работает
            var newVideoFiles = files.Where(f => existedVideos.All(v => !v.IsProbablyTheSameAs(f.Name))).ToArray();

            return newVideoFiles.Select(x => UploadNewVideos(x, channelId, playlistId)).ToArray();
        }

        public UploadResult[] SyncByGoogleDrive(DateTime inclusiveFromDate, DateTime inclusiveEndDate,
            string wikiArchivePageId, string driveFolderId, string youtubeChannelId)
        {
            var files = cloudShare.GetFiles(driveFolderId)
                .Select(x => new
                {
                    Date = DateTimeHelpers.ExtractRussianDateTime(x.Name),
                    File = x
                })
                .Where(x => x.Date.HasValue)
                .Where(x => x.Date.Value >= inclusiveFromDate && x.Date.Value <= inclusiveEndDate)
                .Select(x => x.File)
                .ToArray();

            return files.Select(x => UploadNewVideos(x, youtubeChannelId)).ToArray();
        }

        public UploadResult[] SyncByWiki(DateTime inclusiveFromDate, DateTime inclusiveEndDate, string wikiArchivePageId, string driveFolderId, string youtubeChannelId)
        {
            var pages = wikiClient.GetChildren(wikiArchivePageId);
            var pagesWithDate = pages
                .Where(x => !x.Title.Contains("[uploaded]"))
                .Select(x =>
                {
                    if (!DateTime.TryParse(x.Title.Trim().SafeSubString(0, 10), out var date))
                        return null;

                    return new
                    {
                        Page = x,
                        Date = date
                    };
                })
                .Where(x => x != null)
                .Where(x => x.Date >= inclusiveFromDate && x.Date <= inclusiveEndDate)
                .ToArray();

            var files = cloudShare.GetFiles(driveFolderId)
                .Select(x => new
                {
                    Date = DateTimeHelpers.ExtractRussianDateTime(x.Name),
                    File = x
                })
                .Where(x => x.Date.HasValue)
                .Where(x => x.Date.Value >= inclusiveFromDate && x.Date.Value <= inclusiveEndDate)
                .ToArray();

            var results = new List<UploadResult>();
            foreach (var page in pagesWithDate)
            {
                var file = files.FirstOrDefault(x => x.Date.Value == page.Date);

                if (file == null)
                    continue;

                var result = UploadNewVideos(file.File, youtubeChannelId);
                if (result.Success)
                {
                    wikiClient.UpdateTitleAndGetNewPage(page.Page.Id, $"{page.Page.Title} [uploaded]");
                }
                results.Add(result);
            }

            return results.ToArray();
        }

        private UploadResult UploadNewVideos(DriveFile file, string channelId, string playlistId = null)
        {
            var bytes = cloudShare.DownloadFile(file.FileId);
            var videoToUpload = videoToUploadProvider.GetVideoToUpload(file);
            var uploadResult = cloudShare.UploadToYouTube(bytes, videoToUpload, channelId);

            if (uploadResult.Success && !string.IsNullOrWhiteSpace(playlistId))
            {
                cloudShare.AddVideoToPlayList(uploadResult.VideoId, playlistId);
            }

            return uploadResult;
        }
    }
}