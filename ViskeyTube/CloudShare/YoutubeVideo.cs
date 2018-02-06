using System;

namespace ViskeyTube.CloudShare
{
    public class YoutubeVideo
    {
        public string Name { get; set; }
        public string VideoId { get; set; }
        public DateTime? PublishTime { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }

        public bool IsProbablyTheSameAs(string fileName, long? fileSize)
        {
            return FileName == fileName || FileSize == fileSize;
        }
    }
}