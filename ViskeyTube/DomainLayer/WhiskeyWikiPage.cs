using System;
using ViskeyTube.DomainLayer.Common;
using ViskeyTube.RepositoryLayer.Wiki;

namespace ViskeyTube.DomainLayer
{
    public class WhiskeyWikiPage
    {
        private const string UploadedLabel = "[uploaded]";

        public DateTime? Date { get; }
        public bool HasUploadedLabel => Title.Contains(UploadedLabel);

        public readonly string Title;
        private readonly string Id;
        private string Body;

        public WhiskeyWikiPage(WikiPageLight wikiPage)
        {
            Id = wikiPage.Id;
            Title = wikiPage.Title;
            Date = DateTime.TryParse(Title.Trim().SafeSubString(0, 10), out var parsedDate) ? (DateTime?)parsedDate : null;

        }

        public string GetPageBody(IWikiClient wikiClient)
        {
            return Body ?? (Body = wikiClient.GetPage(Id).Body.View.Value.FromHtml());
        }

        public void MarkUploaded(IWikiClient wikiClient)
        {
            wikiClient.UpdateTitleAndGetNewPage(Id, $"{Title} {UploadedLabel}");
        }
    }
}