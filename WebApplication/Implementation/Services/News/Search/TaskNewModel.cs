using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Search
{
    public class TaskNewModel
    {
        public string BoardId { get; set; }
        public string TaskId { get; set; }
        public TaskNewState State { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public NewDeliveryChannelType DeliveryChannel { get; set; }
        public DateTime? DoNotDeliverUntil { get; set; }
        public long TimeStamp { get; set; }
    }
}