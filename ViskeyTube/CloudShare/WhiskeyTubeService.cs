using System;
using System.Collections.Generic;
using System.Linq;
using ViskeyTube.Common;

namespace ViskeyTube.CloudShare
{
    public class WhiskeyTubeService : IWhiskeyTubeService
    {
        private readonly ICloudShare cloudShare;
        private readonly IVideoToUploadProvider videoToUploadProvider;

        public WhiskeyTubeService(ICloudShare cloudShare, IVideoToUploadProvider videoToUploadProvider)
        {
            this.cloudShare = cloudShare;
            this.videoToUploadProvider = videoToUploadProvider;
        }

        public UploadResult[] SyncByGoogleDrive(string folderId, string channelId, string playlistId)
        {
            var files = cloudShare.GetFiles(folderId);
            var existedVideos = cloudShare.GetVideos(channelId);
            //todo: равенство скорее всего не работает
            var newVideoFiles = files.Where(f => existedVideos.All(v => !v.IsProbablyTheSameAs(f.Name))).ToArray();

            return UploadNewVideos(newVideoFiles, channelId, playlistId).ToArray();
        }

        public UploadResult[] SyncByWiki(DateTime inclusiveFromDate, DateTime inclusiveEndDate,
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

            return UploadNewVideos(files, youtubeChannelId).ToArray();
        }

        private IEnumerable<UploadResult> UploadNewVideos(DriveFile[] files, string channelId, string playlistId = null)
        {
            foreach (var newVideoFile in files)
            {
                var bytes = cloudShare.DownloadFile(newVideoFile.FileId);
                var videoToUpload = videoToUploadProvider.GetVideoToUpload(newVideoFile);
                var uploadResult = cloudShare.UploadToYouTube(bytes, videoToUpload, channelId);

                if (uploadResult.Success && !string.IsNullOrWhiteSpace(playlistId))
                {
                    cloudShare.AddVideoToPlayList(uploadResult.VideoId, playlistId);
                }

                yield return uploadResult;
            }
        }
    }
}