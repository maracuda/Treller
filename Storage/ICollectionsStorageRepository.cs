namespace Storage
{
    public interface ICollectionsStorageRepository
    {
        ICollectionsStorage<T> Get<T>();
        ICollectionsStorage<T> Get<T>(string uniqueStorageName);
    }
}