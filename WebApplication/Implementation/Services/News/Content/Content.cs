using System;
using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Content
{
    public class Content
    {
        public Guid SourceId { get; set; }
        public string Title { get; set; }
        public string Motivation { get; set; }
        public string Analytics { get; set; }
        public string Branch { get; set; }
        public string TechInfo { get; set; }
        public string PubicInfo { get; set; }
        public DateTime? DeadLine { get; set; }
        public long Timestamp { get; set; }

        protected bool Equals(Content other)
        {
            return SourceId.Equals(other.SourceId) && string.Equals(Title, other.Title) &&
                   string.Equals(Motivation, other.Motivation) && string.Equals(Analytics, other.Analytics) &&
                   string.Equals(Branch, other.Branch) && string.Equals(TechInfo, other.TechInfo) &&
                   string.Equals(PubicInfo, other.PubicInfo) && DeadLine.Equals(other.DeadLine);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Content) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = SourceId.GetHashCode();
                hashCode = (hashCode*397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Motivation != null ? Motivation.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Analytics != null ? Analytics.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Branch != null ? Branch.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TechInfo != null ? TechInfo.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (PubicInfo != null ? PubicInfo.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ DeadLine.GetHashCode();
                return hashCode;
            }
        }

        public static readonly IComparer<Content> SourceIdComparer = new ContentSourceIdComparer();

        private sealed class ContentSourceIdComparer : IComparer<Content>
        {
            public int Compare(Content x, Content y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(x, null)) return -1;
                if (ReferenceEquals(y, null)) return -1;

                return x.SourceId == y.SourceId ? 0 : -1;
            }
        }
    }
}