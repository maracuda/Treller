using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Sender
{
    public class Magazine : IMagazine
    {
        private readonly INewsNotificator newsNotificator;
        private readonly INewsSettingsService newsSettingsService;
        private readonly IDateTimeFactory dateTimeFactory;
        private readonly ITaskNewStorage taskNewStorage;

        public Magazine(
            INewsNotificator newsNotificator,
            INewsSettingsService newsSettingsService,
            IDateTimeFactory dateTimeFactory,
            ITaskNewStorage taskNewStorage
            )
        {
            this.newsNotificator = newsNotificator;
            this.newsSettingsService = newsSettingsService;
            this.dateTimeFactory = dateTimeFactory;
            this.taskNewStorage = taskNewStorage;
        }

        public void Publish(string taskId)
        {
            var maybeTaskNews = taskNewStorage.Find(taskId);
            if (maybeTaskNews.HasValue)
            {
                var now = dateTimeFactory.UtcNow;
                foreach (var taskNew in maybeTaskNews.Value)
                {
                    if (taskNew.TryDeliver(newsNotificator, newsSettingsService.GetOrRead(), now))
                    {
                        taskNewStorage.Update(taskNew, $"New delivered at {now.ToString("G")}");
                    }
                }
            }
        }
    }
}