using System.Collections.Generic;

namespace SKBKontur.Treller.Storage
{
    //TODO: to create a factory for ICollectionStorage to have instance per collection
    //TODO: to have a lock to changing the colletion
    //TODO: to add a searching methods?
    public interface ICollectionsStorage<T>
    {
        void Append(T item);
        void Put(T[] items);
        T Get(int index);
        int IndexOf(T item, IComparer<T> comparer);
        T[] GetAll();
        void RemoveAt(int index);
        void Clear();
    }
}