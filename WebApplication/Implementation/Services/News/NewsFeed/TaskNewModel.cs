using System;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Extensions;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public class TaskNewModel
    {
        public string TaskId { get; set; }
        public Content.Content Content { get; set; }
        public Report[] Reports { get; set; }
        public DateTime? DoNotDeliverUntil { get; set; }

        public string DoNotDeliverUntilStr => DoNotDeliverUntil.HasValue ? DoNotDeliverUntil.Value.DateTimeFormat() : "не указано";

        public string Hint => $"{Content.Motivation}\r\n{Content.Analytics}\r\n{Content.Branch}\r\n{Content.PubicInfo}\r\n{Content.TechInfo}";
    }
}