using System.Collections.Generic;
using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public class NewsFeed : INewsFeed
    {
        private readonly ITaskNewStorage taskNewStorage;
        private readonly IOutdatedNewsFilter outdatedNewsFilter;

        public NewsFeed(
            ITaskNewStorage taskNewStorage,
            IOutdatedNewsFilter outdatedNewsFilter)
        {
            this.taskNewStorage = taskNewStorage;
            this.outdatedNewsFilter = outdatedNewsFilter;
        }

        public void AddNews(IEnumerable<TaskNew> news)
        {
            var actualNews = outdatedNewsFilter.FilterActual(news);
            foreach (var taskNew in actualNews)
            {
                var existentTaskNews = taskNewStorage.Find(taskNew.TaskId);
                if (existentTaskNews.HasNoValue)
                {
                    taskNewStorage.Create(taskNew);
                }
                else
                {
                    var exisitentTask = existentTaskNews.Value.FirstOrDefault(x => x.DeliveryChannel == taskNew.DeliveryChannel);
                    if (exisitentTask == null)
                    {
                        taskNewStorage.Create(taskNew);
                    }
                    else
                    {
                        if (!exisitentTask.Delivered)
                        {
                            var newsDiff = exisitentTask.BuildDiff(taskNew);
                            if (!string.IsNullOrEmpty(newsDiff))
                            {
                                taskNewStorage.Update(taskNew, newsDiff);
                            }
                        }
                    }
                }
            }
        }

        public void Refresh(int batchSize = 30)
        {
            long currentTimestamp = 0;
            TaskNew[] newsBatch;
            do
            {
                newsBatch = taskNewStorage.Enumerate(currentTimestamp, batchSize);
                var uselessTaskNews = outdatedNewsFilter.FilterOutdated(newsBatch);
                taskNewStorage.Delete(uselessTaskNews);
                if (newsBatch.Length > 0)
                    currentTimestamp = newsBatch.Max(x => x.TimeStamp);
            } while (newsBatch.Length > 0);
        }
    }
}