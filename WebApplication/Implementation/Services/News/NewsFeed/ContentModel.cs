using System;

namespace WebApplication.Implementation.Services.News.NewsFeed
{
    public class ContentModel
    {
        public string Title { get; set; }
        public string Motivation { get; set; }
        public string Analytics { get; set; }
        public string Branch { get; set; }
        public string TechInfo { get; set; }
        public string PubicInfo { get; set; }
        public DateTime? DeadLine { get; set; }

        public string Hint => $"{Motivation}\r\n{Analytics}\r\n{Branch}\r\n{PubicInfo}\r\n{TechInfo}";
    }
}