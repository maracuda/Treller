using System;
using System.Collections.Concurrent;
using Serialization;

namespace Storage.FileStorage
{
    public class CachedFileStorage : IKeyValueStorage
    {
        private readonly IFileSystemHandler fileSystemHandler;
        private readonly IJsonSerializer jsonSerializer;
        private readonly ConcurrentDictionary<string, dynamic> cache = new ConcurrentDictionary<string, dynamic>();

        public CachedFileStorage(
            IFileSystemHandler fileSystemHandler,
            IJsonSerializer jsonSerializer)
        {
            this.fileSystemHandler = fileSystemHandler;
            this.jsonSerializer = jsonSerializer;
        }

        public T Read<T>(string key)
        {
            if (cache.ContainsKey(key))
                return cache[key];

            var fileName = GetFileName(key);
            if (!fileSystemHandler.Contains(fileName)) 
                throw new Exception($"Fail to find file with name {fileName}. Full path {fileSystemHandler.GetFullPath(fileName)}.");

            return Find<T>(key);
        }

        public T Find<T>(string key)
        {
            return (T) cache.GetOrAdd(key, k =>
            {
                var serializedValue = fileSystemHandler.ReadUTF8(GetFileName(k));
                return jsonSerializer.Deserialize<T>(serializedValue);
            });
        }

        public void Write<T>(string key, T value)
        {
            var serializedValue = jsonSerializer.Serialize(value);

            dynamic stored;
            cache.TryRemove(key, out stored);
            cache.TryAdd(key, value);

            fileSystemHandler.WriteUTF8(GetFileName(key), serializedValue);
        }

        private static string GetFileName(string key)
        {
            return $"Store_{key}.json";
        }
    }
}