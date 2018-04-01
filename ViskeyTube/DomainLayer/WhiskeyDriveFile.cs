using System;
using ViskeyTube.DomainLayer.Common;
using ViskeyTube.RepositoryLayer.Google;

namespace ViskeyTube.DomainLayer
{
    public class WhiskeyDriveFile
    {
        public DateTime? Date { get; }

        private readonly string Id;
        private readonly string Title;
        private byte[] Body;

        public WhiskeyDriveFile(DriveFileDto driveFileDto)
        {
            Id = driveFileDto.FileId;
            Title = driveFileDto.Name;
            Date = DateTimeHelpers.ExtractRussianDateTime(Title);
        }

        public byte[] GetFileBody(ICloudShare cloudShare)
        {
            return Body ?? (Body = cloudShare.DownloadFile(Id));
        }
    }
}