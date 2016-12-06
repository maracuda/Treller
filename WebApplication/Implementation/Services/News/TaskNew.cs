using System;
using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class TaskNew
    {
        public TaskNew()
        {
            Reports = new Report[0];
        }

        public Content.Content Content { get; set; }
        public Report[] Reports { get; set; }
        public string TaskId { get; set; }
        public long TimeStamp { get; set; }

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