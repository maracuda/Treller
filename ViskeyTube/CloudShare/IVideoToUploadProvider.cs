using ViskeyTube.RepositoryLayer;
using ViskeyTube.RepositoryLayer.Google;

namespace ViskeyTube.CloudShare
{
    public interface IVideoToUploadProvider
    {
        VideoToUpload GetVideoToUpload(DriveFile driveFile);
    }
}