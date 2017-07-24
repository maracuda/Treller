using System;
using System.Linq;
using Infrastructure.Sugar;
using Storage;

namespace WebApplication.Implementation.Services.News.Storage
{
    public class TaskNewStorage : ITaskNewStorage
    {
        private readonly ICollectionsStorage<TaskNew> collectionsStorage;
        private static readonly object writeLock = new object();
        
        public TaskNewStorage(
            ICollectionsStorageRepository collectionsStorageRepository)
        {
            this.collectionsStorage = collectionsStorageRepository.Get<TaskNew>();
        }
        
        public Maybe<TaskNew> Find(string taskId)
        {
            return ReadAll().FirstOrDefault(x => string.Equals(taskId, x.TaskId, StringComparison.OrdinalIgnoreCase));
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

            var index = collectionsStorage.IndexOf(taskNew, TaskNew.TaskIdComparer);
            if (index != -1)
            {
                throw new Exception($"Unable to add duplicate task with id {taskNew.TaskId}.");
            }

            lock (writeLock)
            {
                collectionsStorage.Append(taskNew);
            }
        }

        public void Update(TaskNew changedTaskNew)
        {
            if (changedTaskNew == null)
            {
                throw new Exception($"Unable to update null task new.");
            }

            lock (writeLock)
            {
                var index = collectionsStorage.IndexOf(changedTaskNew, TaskNew.TaskIdComparer);
                if (index == -1)
                {
                    throw new Exception($"Fail to find task new with id {changedTaskNew.TaskId} at storage.");
                }

                collectionsStorage.RemoveAt(index);
                collectionsStorage.Append(changedTaskNew);
            }
        }

        public void Delete(params TaskNew[] uselessTaskNews)
        {
            foreach (TaskNew taskNew in uselessTaskNews)
            {
                lock (writeLock)
                {
                    var index = collectionsStorage.IndexOf(taskNew, TaskNew.TaskIdComparer);
                    if (index != -1)
                    {
                        collectionsStorage.RemoveAt(index);
                    }
                }
            }
        }

        public TaskNew[] ReadAll()
        {
            return collectionsStorage.GetAll();
        }
    }
}