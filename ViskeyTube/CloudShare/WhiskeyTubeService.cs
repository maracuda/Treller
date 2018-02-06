using System.Collections.Generic;
using System.Linq;

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

        public UploadResult[] Sync(string folderId, string channelId, string playlistId)
        {
            var files = cloudShare.GetFiles(folderId);
            var existedVideos = cloudShare.GetVideos(channelId);
            //todo: равенство скорее всего не работает
            var newVideoFiles = files.Where(f => existedVideos.All(v => !v.IsProbablyTheSameAs(f.Name, f.Size))).ToArray();

            return UploadNewVideos(newVideoFiles, channelId, playlistId).ToArray();
        }

        private IEnumerable<UploadResult> UploadNewVideos(DriveFile[] files, string channelId, string playlistId)
        {
            foreach (var newVideoFile in files)
            {
                var bytes = cloudShare.DownloadFile(newVideoFile.FileId);
                var videoToUpload = videoToUploadProvider.GetVideoToUpload(newVideoFile);
                var uploadResult = cloudShare.UploadToYouTube(bytes, videoToUpload, channelId);

                if (uploadResult.Success)
                {
                    cloudShare.AddVideoToPlayList(uploadResult.VideoId, playlistId);
                }

                yield return uploadResult;
            }
        }
    }
}