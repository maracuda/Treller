namespace ViskeyTube.RepositoryLayer
{
    public class YoutubeVideoDto
    {
        public string Name { get; set; }
        public string VideoId { get; set; }
        //public DateTime? PublishTime { get; set; }
        //public string FileName { get; set; }
        //public long FileSize { get; set; }

        public bool IsProbablyTheSameAs(string fileName)
        {
            return Name == fileName;
        }
    }
}