using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Releases
{
    public class Release
    {
        public Guid ReleaseId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
    }
}