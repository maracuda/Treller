using System;

namespace ViskeyTube.CloudShare
{
    public class UploadResultDto
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
        public string VideoId { get; set; }
    }
}