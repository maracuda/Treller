using System.Collections.Generic;
using Serialization;

namespace Storage.FileStorage
{
    public class CollectionsStorage<T> : ICollectionsStorage<T>
    {
        private readonly IJsonSerializer jsonSerializer;
        private readonly IFileSystemHandler fileSystemHandler;
        private readonly string fileName;
        private readonly object changeLock = new object();
        private T[] storedValue = null;

        public CollectionsStorage(
            IJsonSerializer jsonSerializer,
            IFileSystemHandler fileSystemHandler, 
            string fileName)
        {
            this.jsonSerializer = jsonSerializer;
            this.fileSystemHandler = fileSystemHandler;
            this.fileName = fileName;
        }

        public void Append(T item)
        {
            var itemsList = new List<T>(GetAll()) {item};
            Put(itemsList.ToArray());
        }

        public void Put(T[] items)
        {
            lock (changeLock)
            {
                storedValue = items;
                var json = jsonSerializer.Serialize(items);
                fileSystemHandler.WriteUTF8(fileName, json);
            }
        }

        public T Get(int index)
        {
            return GetAll()[index];
        }

        public int IndexOf(T item, IComparer<T> comparer)
        {
            var collection = GetAll();
            for (var index = 0; index < collection.Length; index++)
            {
                var current = collection[index];
                if (comparer.Compare(item, current) == 0)
                    return index;
            }
            return -1;
        }

        public T[] GetAll()
        {
            if (storedValue != null)
                return storedValue;

            lock (changeLock)
            {
                var str = fileSystemHandler.ReadUTF8(fileName);
                var result = string.IsNullOrEmpty(str) ? new T[0] : jsonSerializer.Deserialize<T[]>(str);
                storedValue = result;
                return result;
            }
        }

        public void RemoveAt(int index)
        {
            var itemsList = new List<T>(GetAll());
            itemsList.RemoveAt(index);
            Put(itemsList.ToArray());
        }

        public void Clear()
        {
            lock (changeLock)
            {
                storedValue = new T[0];
                fileSystemHandler.Delete(fileName);
            }
        }
    }
}