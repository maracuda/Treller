using System;
using ViskeyTube.DomainLayer.Common;
using ViskeyTube.RepositoryLayer.Wiki;

namespace ViskeyTube.DomainLayer
{
    public class WhiskeyWikiPage
    {
        private readonly Func<string> getPageBody;
        private const string UploadedLabel = "[uploaded]";

        public DateTime? Date { get; }
        public bool HasUploadedLabel => Title.Contains(UploadedLabel);

        public readonly string Title;
        public readonly string Id;
        private string PageBody { get; set; }

        private WhiskeyWikiPage(string title, string id)
        {
            Title = title;
            Id = id;
        }

        public WhiskeyWikiPage(WikiPageLight wikiPage, Func<string> getPageBody)
        {
            this.getPageBody = getPageBody;
            Id = wikiPage.Id;
            Title = wikiPage.Title;
            Date = DateTime.TryParse(Title.Trim().SafeSubString(0, 10), out var parsedDate) ? (DateTime?)parsedDate : null;
        }

        public string GetBody()
        {
            return PageBody ?? (PageBody = getPageBody());
        }

        public WhiskeyWikiPage MarkUploaded()
        {
            return new WhiskeyWikiPage($"{Title} {UploadedLabel}", Id)
            {
                PageBody = this.PageBody
            };
        }
    }
}