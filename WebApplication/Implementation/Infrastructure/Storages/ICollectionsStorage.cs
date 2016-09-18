namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages
{
    public interface ICollectionsStorage
    {
        void Append<T>(T item);
        void Put<T>(T[] items);
        T Get<T>(int index);
        T[] GetAll<T>();
        void RemoveAt<T>(int index);
        void Delete<T>();
        void DeleteAll();
    }
}