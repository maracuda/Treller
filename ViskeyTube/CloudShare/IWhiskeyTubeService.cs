namespace ViskeyTube.CloudShare
{
    public interface IWhiskeyTubeService
    {
        UploadResult[] Sync(string folderId, string channelId, string playlistId);
    }
}