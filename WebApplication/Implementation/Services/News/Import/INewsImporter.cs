using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Import
{
    public interface INewsImporter
    {
        void ImportAll();
        Maybe<string> TryImport(string trelloCardId);
    }
}