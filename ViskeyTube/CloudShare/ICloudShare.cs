namespace ViskeyTube.CloudShare
{
    public interface ICloudShare
    {
        byte[] DownloadFile(string fileId);
        DriveFile[] GetFiles(string folderId);
        UploadResult MoveToYouTube(string fileId, string channelId);
        string[] GetMyChannels();
    }
}