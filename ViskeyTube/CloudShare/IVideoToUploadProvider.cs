namespace ViskeyTube.CloudShare
{
    public interface IVideoToUploadProvider
    {
        VideoToUpload GetVideoToUpload(DriveFile driveFile);
    }
}