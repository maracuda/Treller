using System.Linq;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage
{
    public class TaskNewActionsLogStorage : ITaskNewActionsLogStorage
    {
        private readonly ICachedFileStorage cachedFileStorage;
        private readonly IDateTimeFactory dateTimeFactory;

        private const string dataFileName = "TaskNewsActionLogs";
        private readonly object writeLock = new object();
                
        public TaskNewActionsLogStorage(
            ICachedFileStorage cachedFileStorage,
            IDateTimeFactory dateTimeFactory)
        {
            this.cachedFileStorage = cachedFileStorage;
            this.dateTimeFactory = dateTimeFactory;
        }
        
        public void RegisterCreate(string primaryKey)
        {
            Register(TaskNewActionLogItem.Create(primaryKey, dateTimeFactory.UtcNow));
        }

        public void RegisterUpdate(string primaryKey, string diff)
        {
            Register(TaskNewActionLogItem.Create(primaryKey, diff, dateTimeFactory.UtcNow));
        }

        private void Register(TaskNewActionLogItem logItem)
        {
            lock (writeLock)
            {
                var newLogItemsCollection = ReadAll().Concat(new[] {logItem});
                cachedFileStorage.Write(dataFileName, newLogItemsCollection);
            }
        }

        private TaskNewActionLogItem[] ReadAll()
        {
            return cachedFileStorage.Find<TaskNewActionLogItem[]>(dataFileName) ?? new TaskNewActionLogItem[0];
        }
    }
}