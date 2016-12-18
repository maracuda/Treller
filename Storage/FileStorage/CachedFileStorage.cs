using System.Collections.Concurrent;

namespace SKBKontur.Treller.Storage.FileStorage
{
    public class CachedFileStorage : IKeyValueStorage
    {
        private readonly IFileSystemHandler fileSystemHandler;
        private readonly ConcurrentDictionary<string, dynamic> cache = new ConcurrentDictionary<string, dynamic>();

        public CachedFileStorage(IFileSystemHandler fileSystemHandler)
        {
            this.fileSystemHandler = fileSystemHandler;
        }

        public T Find<T>(string key)
        {
            return (T)cache.GetOrAdd(key, k => fileSystemHandler.FindSafeInJsonUtf8File<T>(GetFileName(k)));
        }

        public void Write<T>(string key, T value)
        {
            dynamic stored;
            cache.TryRemove(key, out stored);
            cache.TryAdd(key, value);

            fileSystemHandler.WriteInJsonUtf8File(GetFileName(key), value);
        }

        private static string GetFileName(string key)
        {
            return $"Store_{key}.json";
        }
    }
}