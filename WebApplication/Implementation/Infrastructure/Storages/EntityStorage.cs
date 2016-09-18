using System;
using System.Collections.Concurrent;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Serialization;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages
{
    public class EntityStorage : IEntitySotrage
    {
        private readonly IFileSystemHandler fileSystemHandler;
        private readonly IJsonSerializer jsonSerializer;
        private readonly ConcurrentDictionary<string, dynamic> cache = new ConcurrentDictionary<string, dynamic>();

        public EntityStorage(
            IFileSystemHandler fileSystemHandler,
            IJsonSerializer jsonSerializer)
        {
            this.fileSystemHandler = fileSystemHandler;
            this.jsonSerializer = jsonSerializer;
        }

        public void Put<T>(T entity)
        {
            var entityKey = GetEntityKey(typeof(T));
            dynamic stored;
            cache.TryRemove(entityKey, out stored);
            cache.TryAdd(entityKey, entity);

            var json = jsonSerializer.Serialize(entity);
            fileSystemHandler.WriteUTF8(entityKey, json);
        }

        public T Get<T>()
        {
            var entityKey = GetEntityKey(typeof(T));
            return (T)cache.GetOrAdd(entityKey, key =>
            {
                var str = fileSystemHandler.ReadUTF8(key);
                if (string.IsNullOrEmpty(str))
                {
                    return default(T);
                }
                return jsonSerializer.Deserialize<T>(str);
            });
        }

        public void Delete<T>()
        {
            Delete(GetEntityKey(typeof(T)));
        }

        private void Delete(string entityKey)
        {
            dynamic stored;
            cache.TryRemove(entityKey, out stored);
            fileSystemHandler.Delete(entityKey);
        }

        public void DeleteAll()
        {
            var entityKeys = cache.Keys;
            foreach (var entityKey in entityKeys)
            {
                Delete(entityKey);
            }
        }

        private static string GetEntityKey(Type entityType)
        {
            return $"Store_{entityType.Name}.json";
        }
    }
}