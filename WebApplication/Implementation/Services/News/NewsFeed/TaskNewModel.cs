using System;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Extensions;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public class TaskNewModel
    {
        public string TaskId { get; set; }
        public TaskNewState State { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public NewDeliveryChannelType DeliveryChannel { get; set; }
        public DateTime? DoNotDeliverUntil { get; set; }
        public long TimeStamp { get; set; }

        public string DoNotDeliverUntilStr => DoNotDeliverUntil.HasValue ? DoNotDeliverUntil.Value.DateTimeFormat() : "не указано";
    }
}