using System;
using SKBKontur.Infrastructure.Sugar;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Content
{
    public interface IContentRepository
    {
        Maybe<Content> Find(Guid sourceId);
        Content Read(Guid sourceId);
        void CreateOrUpdate(Content content);
    }

    public class ContentRepository : IContentRepository
    {
        private static readonly object changeLock = new object();
        private readonly ICollectionsStorage collectionsStorage;

        public ContentRepository(
            ICollectionsStorage collectionsStorage)
        {
            this.collectionsStorage = collectionsStorage;
        }

        public Maybe<Content> Find(Guid sourceId)
        {
            throw new NotImplementedException();
        }

        public Content Read(Guid sourceId)
        {
            throw new NotImplementedException();
        }

        public void CreateOrUpdate(Content content)
        {
            lock (changeLock)
            {
                var index = collectionsStorage.IndexOf(content, Content.SourceIdComparer);
                if (index == -1)
                {
                    collectionsStorage.Append(content);
                }
                else
                {
                    var storageContent = collectionsStorage.Get<Content>(index);
                    if (!storageContent.Equals(content))
                    {
                        collectionsStorage.RemoveAt<Content>(index);
                        collectionsStorage.Append(content);
                    }
                }
            }
        }
    }
}