using System;
using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Sources
{
    public class ContentSourceRepository : IContentSourceRepository
    {
        private static readonly object changeLock = new object();
        private readonly ICollectionsStorage collectionsStorage;

        public ContentSourceRepository(
            ICollectionsStorage collectionsStorage)
        {
            this.collectionsStorage = collectionsStorage;
        }

        public bool Contains(string externalId)
        {
            return Find(externalId) != null;
        }

        private ContentSource Find(string externalId)
        {
            var index = collectionsStorage.IndexOf(new ContentSource { ExternalId = externalId }, ContentSource.ExternalIdComparer);
            return index != -1 ? null : collectionsStorage.Get<ContentSource>(index);
        }

        public ContentSource FindOrRegister(string externalId)
        {
            lock (changeLock)
            {
                var findResult = Find(externalId);
                if (findResult == null)
                {
                    var newContentSource = new ContentSource
                    {
                        Id = Guid.NewGuid(),
                        ExternalId = externalId,
                        State = ContentSourceState.Actual
                    };
                    collectionsStorage.Append(newContentSource);
                    return newContentSource;
                }
                return findResult;
            }
        }

        public void Block(Guid sourceId)
        {
            lock (changeLock)
            {
                var index = collectionsStorage.IndexOf(new ContentSource { Id = sourceId }, ContentSource.IdComparer);
                if (index == -1)
                    throw new Exception($"Fail to find content source with id {sourceId}.");
                var contentSource = collectionsStorage.Get<ContentSource>(index);
                contentSource.State = ContentSourceState.Blocked;
                collectionsStorage.RemoveAt<ContentSource>(index);
                collectionsStorage.Append(contentSource);
            }
        }

        public ContentSource[] SelectActual()
        {
            return collectionsStorage.GetAll<ContentSource>()
                .Where(s => s.State == ContentSourceState.Actual)
                .ToArray();
        }
    }
}