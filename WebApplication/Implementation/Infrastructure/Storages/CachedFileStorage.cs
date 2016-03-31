using System.Collections.Concurrent;
using SKBKontur.Infrastructure.Common;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages
{
    public class CachedFileStorage : ICachedFileStorage
    {
        private readonly IFileSystemHandler fileSystemHandler;
        private readonly ConcurrentDictionary<string, dynamic> cache = new ConcurrentDictionary<string, dynamic>();

        public CachedFileStorage(IFileSystemHandler fileSystemHandler)
        {
            this.fileSystemHandler = fileSystemHandler;
        }

        public T Find<T>(string storeName)
        {
            return (T)cache.GetOrAdd(storeName, (store) => fileSystemHandler.FindSafeInJsonUtf8File<T>(GetFileName(store)));
        }

        public void Write<T>(string storeName, T serializableEntity)
        {
            dynamic stored;
            cache.TryRemove(storeName, out stored);
            cache.TryAdd(storeName, serializableEntity);

            fileSystemHandler.WriteInJsonUtf8File(GetFileName(storeName), serializableEntity);
        }

        private static string GetFileName(string uniqueName)
        {
            return string.Format("Store_{0}.json", uniqueName);
        }
    }
}