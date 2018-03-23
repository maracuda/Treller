using System;
using System.Linq;
using ViskeyTube.Common;
using ViskeyTube.RepositoryLayer;
using ViskeyTube.RepositoryLayer.Google;
using ViskeyTube.RepositoryLayer.Wiki;

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
            if (!date.HasValue)
                return videoToUpload;

            videoToUpload.Title = $"{date.Value:yyyy-MM-dd} Empty bottle of Whiskey";

            var whiskeyPages = wikiClient.GetChildren(ArchieveWhiskeyPageId);
            var suitablePage = whiskeyPages.FirstOrDefault(x => x.Title.StartsWith($"{date.Value:yyyy-MM-dd}"));
            if (suitablePage == null)
                return videoToUpload;

            var pageWithBody = wikiClient.GetPage(suitablePage.Id);
            if (pageWithBody == null)
                return videoToUpload;

            return new VideoToUpload
            {
                Title = pageWithBody.Title.SafeSubString(0, 100),
                Description = $"{pageWithBody.Title}\r\n\r\n{pageWithBody.Body.View.Value.FromHtml()}"
            };
        }
    }
}