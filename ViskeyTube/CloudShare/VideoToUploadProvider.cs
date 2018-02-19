using System;
using System.Text.RegularExpressions;
using ViskeyTube.Common;
using ViskeyTube.Wiki;

namespace ViskeyTube.CloudShare
{
    public class VideoToUploadProvider : IVideoToUploadProvider
    {
        private const string ArchieveWhiskeyPageId = "156696999";

        private readonly IWikiClient wikiClient;

        public VideoToUploadProvider(IWikiClient wikiClient)
        {
            this.wikiClient = wikiClient;
        }

       

        public VideoToUpload GetVideoToUpload(DriveFile driveFile)
        {
            var videoToUpload = new VideoToUpload
            {
                Title = driveFile.Name,
                Description = $"Night Whiskey"
            };

            var date = DateTimeHelpers.ExtractRussianDateTime(videoToUpload.Title);

            var whiskeyPages = wikiClient.GetChildren(ArchieveWhiskeyPageId);


            return videoToUpload;
        }
    }
}