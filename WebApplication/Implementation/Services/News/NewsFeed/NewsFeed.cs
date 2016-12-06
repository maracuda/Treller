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
                var maybeTaskNew = taskNewStorage.Find(taskNew.TaskId);
                if (maybeTaskNew.HasNoValue)
                {
                    taskNewStorage.Create(taskNew);
                }
                else
                {
                    var existentTaskNew = maybeTaskNew.Value;
                    existentTaskNew.Content = taskNew.Content;
                    existentTaskNew.TimeStamp = taskNew.TimeStamp;
                    foreach (var report in taskNew.Reports)
                    {
                        var existentReport = existentTaskNew.Reports.FirstOrDefault(r => r.PublishStrategy == report.PublishStrategy);
                        if (existentReport == null)
                        {
                            existentTaskNew.Reports = new List<Report>(existentTaskNew.Reports) { report }.ToArray();
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

        public TaskNewModel[] SelectAll()
        {
            return taskNewStorage.ReadAll()
                     .Select(x => taskNewConverter.Build(x))
                     .ToArray();
        }

        public TaskNewModel Read(string taskId)
        {
            var result = taskNewStorage.ReadAll()
                                       .Where(x => x.TaskId.Equals(taskId, StringComparison.Ordinal))
                                       .Select(x => taskNewConverter.Build(x))
                                       .FirstOrDefault();

            if (result == null)
            {
                throw new Exception($"Fail to read task new model with id {taskId}.");
            }

            return result;
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