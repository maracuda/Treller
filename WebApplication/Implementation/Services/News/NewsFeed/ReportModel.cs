using System;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Extensions;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public class ReportModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public PublishStrategy PublishStrategy { get; set; }
        public DateTime? DoNotDeliverUntil { get; set; }
        public string DoNotDeliverUntilStr => DoNotDeliverUntil.Stringify("не указано");
        public DateTime? PublishDate { get; set; }
        public string PublishDateStr => PublishDate.Stringify("не указано");

    }
}