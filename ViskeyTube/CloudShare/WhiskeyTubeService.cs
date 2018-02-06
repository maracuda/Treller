﻿using System.Collections.Generic;
using System.Linq;

namespace ViskeyTube.CloudShare
{
    public class WhiskeyTubeService : IWhiskeyTubeService
    {
        private readonly ICloudShare cloudShare;

        public WhiskeyTubeService(ICloudShare cloudShare)
        {
            this.cloudShare = cloudShare;
        }

        public UploadResult[] Sync(string folderId, string channelId, string playlistId)
        {
            var files = cloudShare.GetFiles(folderId);
            var existedVideos = cloudShare.GetVideos(channelId);
            var newVideoFiles = files.Where(f => existedVideos.All(v => !v.IsProbablyTheSameAs(f.Name, f.Size))).ToArray();

            return UploadNewVideos(newVideoFiles, channelId, playlistId).ToArray();
        }

        private IEnumerable<UploadResult> UploadNewVideos(DriveFile[] files, string channelId, string playlistId)
        {
            foreach (var newVideoFile in files)
            {
                var uploadResult = cloudShare.MoveToYouTube(newVideoFile.FileId, channelId);

                if (uploadResult.Success)
                {
                    cloudShare.AddVideoToPlayList(uploadResult.VideoId, playlistId);
                }

                yield return uploadResult;
            }
        }
    }
}