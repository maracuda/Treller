using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsSettingsService : INewsSettingsService
    {
        private readonly ICachedFileStorage cachedFileStorage;

        private const string NewsEmailsStoreName = "NewsEmailsToSend";
        private static readonly NewsSettings DefaultSettings = new NewsSettings
        {
            TechMailingList = "tech.news.billing@skbkontur.ru",
            PublicMailingList = "news.billing@skbkontur.ru"
        };

        public NewsSettingsService(
            ICachedFileStorage cachedFileStorage)
        {
            this.cachedFileStorage = cachedFileStorage;
        }

        public NewsSettings GetOrRead()
        {
            var emails = cachedFileStorage.Find<NewsEmail>(NewsEmailsStoreName);
            if (emails == null)
            {
                return DefaultSettings;
            }
            return new NewsSettings
            {
                TechMailingList = emails.TechnicalEmail,
                PublicMailingList = emails.ReleaseEmail
            };
        }

        public void Reset()
        {
            Update(DefaultSettings.TechMailingList, DefaultSettings.PublicMailingList);
        }

        public void Update(string techMailingList, string publicMailingList)
        {
            var emails = new NewsEmail
            {
                TechnicalEmail = !string.IsNullOrEmpty(techMailingList) ? techMailingList : DefaultSettings.TechMailingList,
                ReleaseEmail = !string.IsNullOrEmpty(publicMailingList) ? publicMailingList : DefaultSettings.PublicMailingList
            };
            cachedFileStorage.Write(NewsEmailsStoreName, emails);
        }
    }
}