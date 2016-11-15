using System;
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
                    return "hvorost@skbkontur.ru";
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
        public DateTime? DeliverDateTime { get; set; }
        public long TimeStamp { get; set; }

        public string PrimaryKey => $"{TaskId}{DeliveryChannel}";

        protected bool Equals(TaskNew other)
        {
            return Equals(Content, other.Content) && string.Equals(TaskId, other.TaskId) &&
                   string.Equals(Text, other.Text) && DeliveryChannel == other.DeliveryChannel &&
                   DoNotDeliverUntil.Equals(other.DoNotDeliverUntil) && DeliverDateTime.Equals(other.DeliverDateTime);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TaskNew) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Content != null ? Content.GetHashCode() : 0;
                hashCode = (hashCode*397) ^ (TaskId != null ? TaskId.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Text != null ? Text.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) DeliveryChannel;
                hashCode = (hashCode*397) ^ DoNotDeliverUntil.GetHashCode();
                hashCode = (hashCode*397) ^ DeliverDateTime.GetHashCode();
                return hashCode;
            }
        }

        public bool IsDelivered()
        {
            return DeliverDateTime.HasValue;
        }

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
            return
                $"{Content.Motivation}\r\n{Content.Analytics}\r\n{Content.Branch}\r\n{Content.PubicInfo}\r\n{Content.TechInfo}";
        }

        public bool TryPublish(INewsNotificator notificator, DateTime now)
        {
            if (DoNotDeliverUntil.HasValue && DoNotDeliverUntil.Value > now)
                return false;

            if (IsDelivered())
                return false;

            Publish(notificator, now);
            return true;
        }

        private void Publish(INewsNotificator notificator, DateTime now)
        {
            var mailingList = ChooseMailingList();
            notificator.NotifyAboutReleases(mailingList, GetContentTitle(), Text);
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
                    return "hvorost@skbkontur.ru";
                default:
                    throw new Exception($"Fail to find mailing list for delivery channel {DeliveryChannel}.");
            }
        }

        public bool HasSamePrimaryKey(TaskNew anotherTaskNew)
        {
            return string.Equals(TaskId, anotherTaskNew.TaskId, StringComparison.OrdinalIgnoreCase)
                   && DeliveryChannel == anotherTaskNew.DeliveryChannel;
        }
    }

    public enum PublishStrategy
    {
        Team = 1,
        Support = 2,
        Customer = 3
    }
}