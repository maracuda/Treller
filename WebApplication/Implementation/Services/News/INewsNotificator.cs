namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public interface INewsNotificator
    {
        void NotifyAboutReleases(string recipient, NewsModel newsModel);
    }
}