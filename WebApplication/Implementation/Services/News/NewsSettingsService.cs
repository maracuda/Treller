using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsSettingsService : INewsSettingsService
    {
        private readonly IEntitySotrage entitySotrage;

        private static readonly NewsSettings defaultSettings = new NewsSettings
        {
            TechMailingList = "tech.news.billing@skbkontur.ru",
            PublicMailingList = "news.billing@skbkontur.ru"
        };

        public NewsSettingsService(IEntitySotrage entitySotrage)
        {
            this.entitySotrage = entitySotrage;
        }

        public NewsSettings GetOrRead()
        {
            var emails = entitySotrage.Get<NewsEmail>();
            if (emails == null)
            {
                return defaultSettings;
            }
            return new NewsSettings
            {
                TechMailingList = emails.TechnicalEmail,
                PublicMailingList = emails.ReleaseEmail
            };
        }

        public void Reset()
        {
            Update(defaultSettings.TechMailingList, defaultSettings.PublicMailingList);
        }

        public void Update(string techMailingList, string publicMailingList)
        {
            var emails = new NewsEmail
            {
                TechnicalEmail = !string.IsNullOrEmpty(techMailingList) ? techMailingList : defaultSettings.TechMailingList,
                ReleaseEmail = !string.IsNullOrEmpty(publicMailingList) ? publicMailingList : defaultSettings.PublicMailingList
            };
            entitySotrage.Put(emails);
        }
    }
}