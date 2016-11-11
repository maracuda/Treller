using System;
using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Sources
{
    public class ContentSource
    {
        public Guid Id { get; set; }
        public ContentSourceState State { get; set; }
        public string ExternalId { get; set; }

        public static readonly IComparer<ContentSource> IdComparer = new ContentSourceIdComparer();
        public static readonly IComparer<ContentSource> ExternalIdComparer = new ContentSourceExternalIdComparer();

        private sealed class ContentSourceIdComparer : IComparer<ContentSource>
        {
            public int Compare(ContentSource x, ContentSource y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(x, null)) return -1;
                if (ReferenceEquals(y, null)) return -1;

                return x.Id == y.Id ? 0 : -1;
            }
        }

        private sealed class ContentSourceExternalIdComparer : IComparer<ContentSource>
        {
            public int Compare(ContentSource x, ContentSource y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(x, null)) return -1;
                if (ReferenceEquals(y, null)) return -1;

                return string.Equals(x.ExternalId, y.ExternalId, StringComparison.Ordinal) ? 0 : -1;
            }
        }
    }

    public enum ContentSourceState
    {
        Actual,
        Blocked
    }
}