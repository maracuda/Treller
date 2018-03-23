using System;
using ViskeyTube.CloudShare;

namespace ViskeyTube.RepositoryLayer.Google
{
    public interface ICloudShare
    {
        byte[] DownloadFile(string fileId);
        DriveFile[] GetFiles(string folderId);
        UploadResult UploadToYouTube(byte[] fileBytes, VideoToUpload videoToUpload, string channelId);
        [Obsolete("Не работает из-за ошибки доступа к гуглу")]
        void AddVideoToPlayList(string videoId, string playlistId);
        [Obsolete("Не работает из-за ошибки доступа к гуглу")]
        string[] GetMyChannels();
        [Obsolete("Не работает из-за ошибки доступа к гуглу")]
        YoutubeVideo[] GetVideos(string channelId);
    }
}