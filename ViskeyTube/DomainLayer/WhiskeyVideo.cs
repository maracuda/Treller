using System;

namespace ViskeyTube.DomainLayer
{
    public class WhiskeyVideo
    {
        private readonly WhiskeyDriveFile driveFile;
        private readonly WhiskeyWikiPage wikiPage;
        private readonly Func<WhiskeyWikiPage, string> getPageBody;


        public WhiskeyVideo(WhiskeyDriveFile driveFile, WhiskeyWikiPage wikiPage)
        {
            this.driveFile = driveFile;
            this.wikiPage = wikiPage;
        }

        public bool ReadyToUpload => driveFile != null && wikiPage != null && !wikiPage.HasUploadedLabel;


        public string Description => wikiPage.GetBody();    
        public string Title => wikiPage.Title;
        public byte[] VideoBytes => driveFile.GetFileBody();

        public WhiskeyWikiPage MarkUploaded()
        {
            return wikiPage.MarkUploaded();
        }
    }
}