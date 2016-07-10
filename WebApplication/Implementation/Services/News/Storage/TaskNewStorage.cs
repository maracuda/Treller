using System;
using System.Linq;
using SKBKontur.Infrastructure.Sugar;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage
{
    public class TaskNewStorage : ITaskNewStorage
    {
        private readonly ICachedFileStorage cachedFileStorage;
        private readonly ITaskNewActionsLogStorage taskNewActionsLogStorage;
        private const string dataFileName = "TaskNews";
        
        public TaskNewStorage(
            ICachedFileStorage cachedFileStorage,
            ITaskNewActionsLogStorage taskNewActionsLogStorage)
        {
            this.cachedFileStorage = cachedFileStorage;
            this.taskNewActionsLogStorage = taskNewActionsLogStorage;
        }
        
        public Maybe<TaskNew[]> Find(string taskId)
        {
            var result = ReadAll().Where(x => string.Equals(taskId, x.TaskId, StringComparison.OrdinalIgnoreCase))
                                  .ToArray();
            return result.Any() ? result : null;
        }

        public void Create(TaskNew taskNew)
        {
            if (taskNew == null)
            {
                throw new Exception($"Unable to add null task new.");
            }

            var taskNews = ReadAll();
            if (taskNews.Any(x => x.HasSamePrimaryKey(taskNew)))
            {
                throw new Exception($"Unable to add duplicate task new by primary key {taskNew.PrimaryKey}.");
            }

            taskNewActionsLogStorage.RegisterCreate(taskNew.PrimaryKey);
            cachedFileStorage.Write(dataFileName, taskNews.Concat(new[] {taskNew}));
        }

        public void Update(TaskNew changedTaskNew, string diffInfo)
        {
            if (changedTaskNew == null)
            {
                throw new Exception($"Unable to update null task new.");
            }
            if (string.IsNullOrWhiteSpace(diffInfo))
            {
                throw new Exception($"Unable to update task new with empty diff.");
            }

            var taskNews = ReadAll();
            for (var index = 0; index < taskNews.Length; index++)
            {
                if (taskNews[index].HasSamePrimaryKey(changedTaskNew))
                {
                    taskNews[index] = changedTaskNew;
                }
            }

            taskNewActionsLogStorage.RegisterUpdate(changedTaskNew.PrimaryKey, diffInfo);
            cachedFileStorage.Write(dataFileName, taskNews);
        }

        private TaskNew[] ReadAll()
        {
            return cachedFileStorage.Find<TaskNew[]>(dataFileName) ?? new TaskNew[0];
        }
    }
}