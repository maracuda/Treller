namespace ViskeyTube.CloudShare
{
    public class VideoToUploadProvider : IVideoToUploadProvider
    {
        public VideoToUpload GetVideoToUpload(DriveFile driveFile)
        {
            //todo: запарсить вики        
            return new VideoToUpload
            {
                Title = driveFile.Name,
                Description = $"Night Whiskey of {driveFile.CreatedTime:d}"
            };
        }
    }
}