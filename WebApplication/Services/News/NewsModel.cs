using System;

namespace SKBKontur.Treller.WebApplication.Services.News
{
    public class NewsModel
    {
        public Guid Id { get; set; }
        public string NewsText { get; set; }
        public string NewsHeader { get; set; }
        public bool IsTechnicalNews { get; set; }
        public string[] CardIds { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
    }
}