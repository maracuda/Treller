namespace ViskeyTube.CloudShare
{
    public interface ICloudShare
    {
        byte[] DownloadFile(string fileId);
        DriveFile[] GetFiles(string folderId);
        UploadResult UploadToYouTube(byte[] fileBytes, VideoToUpload videoToUpload, string channelId);
        void AddVideoToPlayList(string videoId, string playlistId);
        string[] GetMyChannels();
        YoutubeVideo[] GetVideos(string channelId);
    }
}