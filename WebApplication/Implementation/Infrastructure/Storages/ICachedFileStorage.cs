namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages
{
    public interface ICachedFileStorage
    {
        T Find<T>(string storeName);
        void Write<T>(string storeName, T entity);
    }
}