using System;
using System.Collections.Generic;
using System.Linq;

namespace ViskeyTube.DomainLayer
{
    public class WhiskeyVideoSet
    {
        private readonly List<WhiskeyWikiPage> WikiPages = new List<WhiskeyWikiPage>();
        private readonly List<WhiskeyDriveFile> DriveFiles = new List<WhiskeyDriveFile>();

        public WhiskeyVideoSet AddWikiPages(params WhiskeyWikiPage[] pages)
        {
            WikiPages.AddRange(pages);
            return this;
        }

        public WhiskeyVideoSet AddDriveFiles(params WhiskeyDriveFile[] files)
        {
            DriveFiles.AddRange(files);
            return this;
        }

        public WhiskeyVideo[] GetVideosToUpload(DateTime inclusiveFromDate, DateTime inclusiveEndDate)
        {
            var suitableDriveFiles = DriveFiles.Where(x => x.Date.HasValue);

            return WikiPages.Where(x => x.Date.HasValue)
                .Where(x => x.Date >= inclusiveFromDate && x.Date <= inclusiveEndDate).Select(p =>
                {
                    var driveFile = suitableDriveFiles.FirstOrDefault(x => p.Date.Value == x.Date.Value);
                    return new WhiskeyVideo(driveFile, p);
                })
                .Where(x => x.ReadyToUpload)
                .ToArray();
        }
    }
}