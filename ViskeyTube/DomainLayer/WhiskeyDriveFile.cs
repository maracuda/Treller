using System;
using ViskeyTube.DomainLayer.Common;
using ViskeyTube.RepositoryLayer.Google;

namespace ViskeyTube.DomainLayer
{
    public class WhiskeyDriveFile
    {
        private readonly Func<byte[]> getBody;
        public DateTime? Date { get; }

        private readonly string Title;
        private byte[] Body;

        public WhiskeyDriveFile(DriveFileDto driveFileDto, Func<byte[]> getBody)
        {
            this.getBody = getBody;
            Title = driveFileDto.Name;
            Date = DateTimeHelpers.ExtractRussianDateTime(Title);
        }

        public byte[] GetFileBody()
        {
            return Body ?? (Body = getBody());
        }
    }
}