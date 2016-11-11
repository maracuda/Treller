using System;
using System.Text;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Extensions;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Publisher;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class TaskNew
    {
        public Content.Content Content { get; set; }

        [Obsolete]
        public string BoardId { get; set; }
        public string TaskId { get; set; }
        [Obsolete]
        public string Title { get; set; }
        [Obsolete]
        public string Text { get; set; }
        public NewDeliveryChannelType DeliveryChannel { get; set; }
        public DateTime? DoNotDeliverUntil { get; set; }
        public bool Delivered { get; set; }
        public DateTime? DeliverDateTime { get; set; }
        public long TimeStamp { get; set; }

        public string PrimaryKey => $"{TaskId}{DeliveryChannel}";

        public string GetContentTitle()
        {
            return Content == null ? Title : Content.Title;
        }

        public string GetContentText()
        {
            return Content == null ? Text : $"{Content.Motivation}\r\n{Content.Analytics}\r\n{Content.Branch}\r\n{Content.PubicInfo}\r\n{Content.TechInfo}";
        }

        public bool TryDeliver(INewsNotificator notificator, DateTime now)
        {
            if (DoNotDeliverUntil.HasValue && DoNotDeliverUntil.Value > now)
                return false;

            if (Delivered)
                return false;

            Deliver(notificator, now);
            return true;
        }

        public void Deliver(INewsNotificator notificator, DateTime now)
        {
            var mailingList = ChooseMailingList();
            notificator.NotifyAboutReleases(mailingList, Title, Text);

            Delivered = true;
            DeliverDateTime = now;
            TimeStamp = now.Ticks;
        }

        private string ChooseMailingList()
        {
            switch (DeliveryChannel)
            {
                case NewDeliveryChannelType.Customer:
                    return "news.billing@skbkontur.ru";
                case NewDeliveryChannelType.Support:
                    return "tech.news.billing@skbkontur.ru";
                case NewDeliveryChannelType.Team:
                    return "dream_proj@skbkontur.ru";
                default:
                    throw new Exception($"Fail to find mailing list for delivery channel {DeliveryChannel}.");
            }
        }

        public bool HasSamePrimaryKey(TaskNew anotherTaskNew)
        {
            return string.Equals(TaskId, anotherTaskNew.TaskId, StringComparison.OrdinalIgnoreCase)
                   && DeliveryChannel == anotherTaskNew.DeliveryChannel;
        }

        public string BuildDiff(TaskNew anotherTaskNew)
        {
            if (!HasSamePrimaryKey(anotherTaskNew))
                throw new ArgumentException($"Fail to build diff for task news with different primary keys. " +
                                            $"This: {TaskId},{DeliveryChannel}. Another: {anotherTaskNew.TaskId},{anotherTaskNew.DeliveryChannel}.");

            var builder = new StringBuilder();
            if (!string.Equals(Title, anotherTaskNew.Title, StringComparison.Ordinal))
            {
                builder.Append($"Title {Title} changed to {anotherTaskNew.Title}");
            }
            if (!string.Equals(Text, anotherTaskNew.Text, StringComparison.Ordinal))
            {
                builder.Append($"Text {Text} changed to {anotherTaskNew.Text}");
            }
            if (DoNotDeliverUntil != anotherTaskNew.DoNotDeliverUntil)
            {
                builder.Append($"DoNotDeliverUntil {DoNotDeliverUntil.SafeDateTimeFormat()} changed to {anotherTaskNew.DoNotDeliverUntil.SafeDateTimeFormat()}");
            }
            return builder.ToString();
        }
    }

    public enum NewDeliveryChannelType
    {
        Team = 1,
        Support = 2,
        Customer = 3
    }
}