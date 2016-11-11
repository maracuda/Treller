using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.Infrastructure.Sugar;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage
{
    public class TaskNewStorage : ITaskNewStorage
    {
        private readonly TaskNewComparer taskNewComparer = new TaskNewComparer();
        private readonly ICollectionsStorage collectionsStorage;
        private readonly ITaskNewActionsLogStorage taskNewActionsLogStorage;
        private static readonly object writeLock = new object();
        
        public TaskNewStorage(
            ICollectionsStorage collectionsStorage,
            ITaskNewActionsLogStorage taskNewActionsLogStorage)
        {
            this.collectionsStorage = collectionsStorage;
            this.taskNewActionsLogStorage = taskNewActionsLogStorage;
        }
        
        public Maybe<TaskNew[]> Find(string taskId)
        {
            var result = ReadAll().Where(x => string.Equals(taskId, x.TaskId, StringComparison.OrdinalIgnoreCase))
                                  .ToArray();
            return result.Any() ? result : null;
        }

        public TaskNew[] Enumerate(long fromTimestampExclusive, int batchSize)
        {
            return ReadAll().OrderBy(t => t.TimeStamp)
                            .Where(t => t.TimeStamp > fromTimestampExclusive)
                            .Take(batchSize)
                            .ToArray();
        }

        public void Create(TaskNew taskNew)
        {
            if (taskNew == null)
            {
                throw new Exception("Unable to add null task new.");
            }

            var index = collectionsStorage.IndexOf(taskNew, taskNewComparer);
            if (index != -1)
            {
                throw new Exception($"Unable to add duplicate task new by primary key {taskNew.PrimaryKey}.");
            }

            taskNewActionsLogStorage.RegisterCreate(taskNew.PrimaryKey);
            lock (writeLock)
            {
                collectionsStorage.Append(taskNew);
            }
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

            lock (writeLock)
            {
                var index = collectionsStorage.IndexOf(changedTaskNew, taskNewComparer);
                if (index == -1)
                    throw new Exception($"Fail to find task new with {changedTaskNew.PrimaryKey} at storage.");

                taskNewActionsLogStorage.RegisterUpdate(changedTaskNew.PrimaryKey, diffInfo);
                collectionsStorage.RemoveAt<TaskNew>(index);
                collectionsStorage.Append(changedTaskNew);
            }
        }

        public void Delete(params TaskNew[] uselessTaskNews)
        {
            foreach (TaskNew taskNew in uselessTaskNews)
            {
                lock (writeLock)
                {
                    var index = collectionsStorage.IndexOf(taskNew, taskNewComparer);
                    if (index != -1)
                    {
                        taskNewActionsLogStorage.RegisterDelete(taskNew.PrimaryKey);
                        collectionsStorage.RemoveAt<TaskNew>(index);
                    }
                }
            }
        }

        public TaskNew[] ReadAll()
        {
            return collectionsStorage.GetAll<TaskNew>();
        }
    }

    internal class TaskNewComparer : IComparer<TaskNew>
    {
        public int Compare(TaskNew x, TaskNew y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(x, null)) return -1;
            if (ReferenceEquals(y, null)) return -1;

            return x.HasSamePrimaryKey(y) ? 0 : -1;
        }
    }
}