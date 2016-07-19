namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage
{
    public interface ITaskNewActionsLogStorage
    {
        void RegisterCreate(string primaryKey);
        void RegisterUpdate(string primaryKey, string diff);
        void RegisterDelete(string primaryKey);
    }
}