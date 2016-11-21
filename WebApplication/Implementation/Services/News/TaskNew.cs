using System;
using System.Collections.Generic;
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
        public Report[] Reports { get; set; }
        public string TaskId { get; set; }
        public long TimeStamp { get; set; }

        public string GetContentText()
        {
            if (Content == null)
                throw new Exception($"Content is empty for TaskNew with TaskId {TaskId}.");
            return
                $"{Content.Motivation}\r\n{Content.Analytics}\r\n{Content.Branch}\r\n{Content.PubicInfo}\r\n{Content.TechInfo}";
        }

        public bool TryPublish(INewsNotificator notificator, DateTime now)
        {
            var isPublished = false;
            foreach (var report in Reports)
            {
                isPublished = report.TryPublish(notificator, now);
                if (isPublished)
                    TimeStamp = now.Ticks;
            }

            return isPublished;
        }

        protected bool Equals(TaskNew other)
        {
            return Equals(Content, other.Content) && string.Equals(TaskId, other.TaskId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TaskNew) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Content != null ? Content.GetHashCode() : 0)*397) ^ (TaskId != null ? TaskId.GetHashCode() : 0);
            }
        }

        public static readonly IComparer<TaskNew> TaskIdComparer = new TaskNewIdComparer();

        private sealed class TaskNewIdComparer : IComparer<TaskNew>
        {
            public int Compare(TaskNew x, TaskNew y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(x, null)) return -1;
                if (ReferenceEquals(y, null)) return -1;

                return string.Equals(x.TaskId, y.TaskId, StringComparison.OrdinalIgnoreCase) ? 0 : -1;
            }
        }
    }

    public enum PublishStrategy
    {
        Team = 1,
        Support = 2,
        Customer = 3
    }
}