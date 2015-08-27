using SKBKontur.Treller.WebApplication.Services.TaskCacher;

namespace SKBKontur.Treller.WebApplication.Storages
{
    public interface ICachedFileStorage
    {
        T Find<T>(string storeName);
        void Write<T>(string storeName, T entity);
    }
}