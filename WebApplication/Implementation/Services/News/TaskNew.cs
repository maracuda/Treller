using System;
using System.Collections.Generic;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Publisher;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class TaskNew
    {
        public Content.Content Content { get; set; }
        public Report[] Reports { get; set; }
        public string TaskId { get; set; }
        public long TimeStamp { get; set; }

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
}