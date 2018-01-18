namespace ViskeyTube.CloudShare
{
    public interface ICloudShare
    {
        byte[] DownloadFile(string fileId);
        string[] GetFiles(string folderId);
    }
}