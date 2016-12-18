using System.Collections.Generic;

namespace SKBKontur.Treller.Storage
{
    //TODO: to create a factory for ICollectionStorage to have instance per collection
    //TODO: to have a lock to changing the colletion
    //TODO: to add a searching methods?
    public interface ICollectionsStorage
    {
        void Append<T>(T item);
        void Put<T>(T[] items);
        T Get<T>(int index);
        int IndexOf<T>(T item, IComparer<T> comparer);
        T[] GetAll<T>();
        void RemoveAt<T>(int index);
        void Delete<T>();
        void DeleteAll();
    }
}