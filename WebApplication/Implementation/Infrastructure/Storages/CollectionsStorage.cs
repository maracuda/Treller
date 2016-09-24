using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Serialization;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages
{
    public class CollectionsStorage : ICollectionsStorage
    {
        private readonly IJsonSerializer jsonSerializer;
        private readonly IFileSystemHandler fileSystemHandler;
        private readonly ConcurrentDictionary<string, dynamic> cache = new ConcurrentDictionary<string, dynamic>();

        public CollectionsStorage(
            IJsonSerializer jsonSerializer,
            IFileSystemHandler fileSystemHandler)
        {
            this.jsonSerializer = jsonSerializer;
            this.fileSystemHandler = fileSystemHandler;
        }

        public void Append<T>(T item)
        {
            var itemsList = new List<T>(GetAll<T>()) {item};
            Put(itemsList.ToArray());
        }

        public void Put<T>(T[] items)
        {
            var entityKey = GetEntityKey(typeof(T));
            dynamic stored;
            cache.TryRemove(entityKey, out stored);
            cache.TryAdd(entityKey, items);

            var json = jsonSerializer.Serialize(items);
            fileSystemHandler.WriteUTF8(entityKey, json);
        }

        public T Get<T>(int index)
        {
            return GetAll<T>()[index];
        }

        public int IndexOf<T>(T item, IComparer<T> comparer)
        {
            var collection = GetAll<T>();
            for (var index = 0; index < collection.Length; index++)
            {
                var current = collection[index];
                if (comparer.Compare(item, current) == 0)
                    return index;
            }
            return -1;
        }

        public T[] GetAll<T>()
        {
            var entityKey = GetEntityKey(typeof(T));
            return (T[])cache.GetOrAdd(entityKey, key =>
            {
                var str = fileSystemHandler.ReadUTF8(key);
                return string.IsNullOrEmpty(str) ? new T[0] : jsonSerializer.Deserialize<T[]>(str);
            });
        }

        public void RemoveAt<T>(int index)
        {
            var itemsList = new List<T>(GetAll<T>());
            itemsList.RemoveAt(index);
            Put(itemsList.ToArray());
        }

        public void Delete<T>()
        {
            Delete(GetEntityKey(typeof(T)));
        }

        public void DeleteAll()
        {
            var entityKeys = cache.Keys;
            foreach (var entityKey in entityKeys)
            {
                Delete(entityKey);
            }
        }

        private void Delete(string entityKey)
        {
            dynamic stored;
            cache.TryRemove(entityKey, out stored);
            fileSystemHandler.Delete(entityKey);
        }

        private static string GetEntityKey(Type entityType)
        {
            return $"Store_{entityType.Name}.json";
        }
    }
}