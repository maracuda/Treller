using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Publisher
{
    public class EmailPublisher : IPublisher
    {
        private readonly INewsNotificator newsNotificator;
        private readonly IDateTimeFactory dateTimeFactory;
        private readonly ITaskNewStorage taskNewStorage;

        public EmailPublisher(
            INewsNotificator newsNotificator,
            IDateTimeFactory dateTimeFactory,
            ITaskNewStorage taskNewStorage
            )
        {
            this.newsNotificator = newsNotificator;
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
                    if (taskNew.TryDeliver(newsNotificator, now))
                    {
                        taskNewStorage.Update(taskNew, $"New delivered at {now.ToString("G")}");
                    }
                }
            }
        }
    }
}