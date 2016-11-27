using System;
using System.Linq;
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

        public void Publish(string taskId, PublishStrategy publishStrategy)
        {
            var maybeTaskNew = taskNewStorage.Find(taskId);
            if (maybeTaskNew.HasValue)
            {
                var now = dateTimeFactory.UtcNow;
                var report = maybeTaskNew.Value.Reports.FirstOrDefault(r => r.PublishStrategy == publishStrategy);
                if (report == null)
                {
                    throw new Exception($"Fail to publish report for taskId {taskId} and publish strategy {publishStrategy}.");
                }

                if (report.TryPublish(newsNotificator, now))
                {
                    taskNewStorage.Update(maybeTaskNew.Value);
                }
            }
        }
    }
}