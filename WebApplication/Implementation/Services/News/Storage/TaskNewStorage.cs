using System;
using System.Collections.Generic;
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
        private static readonly object writeLock = new object();
        
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
            UpdateStorage(taskNews.Concat(new[] {taskNew}).ToArray());
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
            var index = IndexOf(taskNews, changedTaskNew);
            if (index == -1)
                throw new Exception($"Fail to find task new with {changedTaskNew.PrimaryKey} at storage.");

            taskNews[index] = changedTaskNew;
            taskNewActionsLogStorage.RegisterUpdate(changedTaskNew.PrimaryKey, diffInfo);
            UpdateStorage(taskNews);
        }

        public void Delete(params TaskNew[] uselessTaskNews)
        {
            var taskNews = new List<TaskNew>(ReadAll());
            for (var i = 0; i < uselessTaskNews.Length; i++)
            {
                var index = IndexOf(taskNews, uselessTaskNews[i]);
                if (index != -1)
                {
                    taskNews.RemoveAt(index);
                    taskNewActionsLogStorage.RegisterDelete(uselessTaskNews[i].PrimaryKey);
                }
            }
            UpdateStorage(taskNews.ToArray());
        }

        public TaskNew[] ReadAll()
        {
            return cachedFileStorage.Find<TaskNew[]>(dataFileName) ?? new TaskNew[0];
        }

        private void UpdateStorage(TaskNew[] taskNews)
        {
            lock (writeLock)
            {
                cachedFileStorage.Write(dataFileName, taskNews);
            }
        }

        private static int IndexOf(IReadOnlyList<TaskNew> allTaskNews, TaskNew taskNew)
        {
            for (var index = 0; index < allTaskNews.Count; index++)
            {
                if (allTaskNews[index].HasSamePrimaryKey(taskNew))
                {
                    return index;
                }
            }
            return -1;
        }
    }
}