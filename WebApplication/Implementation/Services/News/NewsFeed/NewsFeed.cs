using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public class NewsFeed : INewsFeed
    {
        private readonly ITaskNewStorage taskNewStorage;
        private readonly IOutdatedNewsFilter outdatedNewsFilter;
        private readonly ITaskNewConverter taskNewConverter;

        public NewsFeed(
            ITaskNewStorage taskNewStorage,
            IOutdatedNewsFilter outdatedNewsFilter,
            ITaskNewConverter taskNewConverter)
        {
            this.taskNewStorage = taskNewStorage;
            this.outdatedNewsFilter = outdatedNewsFilter;
            this.taskNewConverter = taskNewConverter;
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
                    if (existentTaskNews.Value.Length > 1)
                        throw new Exception($"Found more than one task news for taskId {taskNew.TaskId}.");

                    if (existentTaskNews.Value.Length == 0)
                    {
                        taskNewStorage.Create(taskNew);
                    }
                    else
                    {
                        var existentTaskNew = existentTaskNews.Value[0];
                        existentTaskNew.Content = taskNew.Content;
                        existentTaskNew.TimeStamp = taskNew.TimeStamp;
                        foreach (var report in taskNew.Reports)
                        {
                            var existentReport = existentTaskNew.Reports.FirstOrDefault(r => r.PublishStrategy == report.PublishStrategy);
                            if (existentReport == null)
                            {
                                existentTaskNew.Reports = new List<Report>(existentTaskNew.Reports) { existentReport }.ToArray();
                            }
                            else
                            {
                                if (!existentReport.PublishDate.HasValue)
                                {
                                    existentReport.Title = report.Title;
                                    existentReport.Message = report.Message;
                                    existentReport.DoNotDeliverUntil = report.DoNotDeliverUntil;
                                }
                            }
                        }
                        taskNewStorage.Update(existentTaskNew);
                    }
                }
            }
        }

        public TaskNewModel[] SelectAll()
        {
            return taskNewStorage.ReadAll()
                     .Select(x => taskNewConverter.Build(x))
                     .ToArray();
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