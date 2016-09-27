namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Import
{
    public interface INewsImporter
    {
        void ImportAll();
        void Import(string trelloCardId);
    }
}