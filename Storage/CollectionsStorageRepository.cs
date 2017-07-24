using System.Collections.Concurrent;
using Serialization;
using Storage.FileStorage;

namespace Storage
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
            return Get<T>($"Store_{typeof(T).Name}.json");
        }

        public ICollectionsStorage<T> Get<T>(string uniqueStorageName)
        {
            var fileName = $"Store_{uniqueStorageName}.json";
            return (ICollectionsStorage<T>)storagesMap.GetOrAdd(fileName, key => new CollectionsStorage<T>(jsonSerializer, fileSystemHandler, key));
        }
    }
}