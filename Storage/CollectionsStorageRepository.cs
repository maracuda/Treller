using System.Collections.Concurrent;
using SKBKontur.Treller.Serialization;
using SKBKontur.Treller.Storage.FileStorage;

namespace SKBKontur.Treller.Storage
{
    public class CollectionsStorageRepository : ICollectionsStorageRepository
    {
        private readonly IJsonSerializer jsonSerializer;
        private readonly IFileSystemHandler fileSystemHandler;
        private static readonly ConcurrentDictionary<string, object> storagesMap = new ConcurrentDictionary<string, object>();

        public CollectionsStorageRepository(
            IJsonSerializer jsonSerializer,
            IFileSystemHandler fileSystemHandler)
        {
            this.jsonSerializer = jsonSerializer;
            this.fileSystemHandler = fileSystemHandler;
        }

        public ICollectionsStorage<T> Get<T>()
        {
            return (ICollectionsStorage<T>) storagesMap.GetOrAdd(typeof(T).Name, x => new CollectionsStorage<T>(jsonSerializer, fileSystemHandler));
        }

        public ICollectionsStorage<T> Get<T>(string uniqueStorageName)
        {
            return (ICollectionsStorage<T>)storagesMap.GetOrAdd(typeof(T).Name, x => new CollectionsStorage<T>(jsonSerializer, fileSystemHandler));
        }
    }
}