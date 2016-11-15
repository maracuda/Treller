using System;
using System.Text;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Extensions;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Publisher;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class Report
    {
        public string TaskId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public PublishStrategy PublishStrategy { get; set; }
        public DateTime? DoNotDeliverUntil { get; set; }
        public DateTime? PublishDate { get; set; }

        public bool TryPublish(INewsNotificator notificator, DateTime now)
        {
            if (DoNotDeliverUntil.HasValue && DoNotDeliverUntil.Value > now)
                return false;

            if (PublishDate.HasValue)
                return false;

            Publish(notificator, now);
            return true;
        }

        private void Publish(INewsNotificator notificator, DateTime now)
        {
            var mailingList = ChooseMailingList();
            notificator.NotifyAboutReleases(mailingList, Title, Message);
            PublishDate = now;
        }

        private string ChooseMailingList()
        {
            switch (PublishStrategy)
            {
                case PublishStrategy.Customer:
                    return "news.billing@skbkontur.ru";
                case PublishStrategy.Support:
                    return "tech.news.billing@skbkontur.ru";
                case PublishStrategy.Team:
                    return "dream_proj@skbkontur.ru";
                default:
                    throw new Exception($"Fail to find mailing list for publish strategy {PublishStrategy}.");
            }
        }
    }

    public class TaskNew
    {
        public Content.Content Content { get; set; }
        public string TaskId { get; set; }
        [Obsolete]
        public string Text { get; set; }
        public PublishStrategy DeliveryChannel { get; set; }
        public DateTime? DoNotDeliverUntil { get; set; }
        public bool Delivered { get; set; }
        public DateTime? DeliverDateTime { get; set; }
        public long TimeStamp { get; set; }

        public string PrimaryKey => $"{TaskId}{DeliveryChannel}";

        public string GetContentTitle()
        {
            if (Content == null)
                throw new Exception($"Content is empty for TaskNew with TaskId {TaskId}.");
            return Content.Title;
        }

        public string GetContentText()
        {
            if (Content == null)
                throw new Exception($"Content is empty for TaskNew with TaskId {TaskId}.");
            return $"{Content.Motivation}\r\n{Content.Analytics}\r\n{Content.Branch}\r\n{Content.PubicInfo}\r\n{Content.TechInfo}";
        }

        public bool TryPublish(INewsNotificator notificator, DateTime now)
        {
            if (DoNotDeliverUntil.HasValue && DoNotDeliverUntil.Value > now)
                return false;

            if (Delivered)
                return false;

            Publish(notificator, now);
            return true;
        }

        private void Publish(INewsNotificator notificator, DateTime now)
        {
            var mailingList = ChooseMailingList();
            notificator.NotifyAboutReleases(mailingList, GetContentTitle(), Text);

            Delivered = true;
            DeliverDateTime = now;
            TimeStamp = now.Ticks;
        }

        private string ChooseMailingList()
        {
            switch (DeliveryChannel)
            {
                case PublishStrategy.Customer:
                    return "news.billing@skbkontur.ru";
                case PublishStrategy.Support:
                    return "tech.news.billing@skbkontur.ru";
                case PublishStrategy.Team:
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
            if (!string.Equals(GetContentTitle(), anotherTaskNew.GetContentTitle(), StringComparison.Ordinal))
            {
                builder.Append($"Title {GetContentTitle()} changed to {anotherTaskNew.GetContentTitle()}");
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

    public enum PublishStrategy
    {
        Team = 1,
        Support = 2,
        Customer = 3
    }
}