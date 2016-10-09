namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Publisher
{
    public interface INewsNotificator
    {
        void NotifyAboutReleases(string mailingList, string title, string text);
    }
}