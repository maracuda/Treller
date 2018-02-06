using System;

namespace ViskeyTube.CloudShare
{
    public class DriveFile
    {
        public string Name { get; set; }
        public string FileId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public long? Size { get; set; }
    }
}