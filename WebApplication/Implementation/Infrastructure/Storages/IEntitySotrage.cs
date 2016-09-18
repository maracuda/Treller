namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages
{
    public interface IEntitySotrage
    {
        void Put<T>(T entity);
        T Get<T>();
        void Delete<T>();
        void DeleteAll();
    }
}