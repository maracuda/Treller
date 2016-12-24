using System.Linq;
using SKBKontur.Treller.Logger;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Migration
{
    public class NullReportConsistencyChecker
    {
        private readonly ITaskNewStorage taskNewStorage;
        private readonly ILoggerFactory loggerFactory;

        public NullReportConsistencyChecker(
            ITaskNewStorage taskNewStorage,
            ILoggerFactory loggerFactory)
        {
            this.taskNewStorage = taskNewStorage;
            this.loggerFactory = loggerFactory;
        }

        public void Run()
        {
            var taskNews = taskNewStorage.ReadAll();
            var taskNewsWithNullReportsCount = taskNews.Count(t => t.Reports.Any(r => r == null));
            var taskNewsWithNullReportsMirgratedCount = 0;
            foreach (var taskNew in taskNews.Where(t => t.Reports.Any(r => r == null)))
            {
                taskNew.Reports = taskNew.Reports.Where(r => r != null).ToArray();
                taskNewStorage.Update(taskNew);
                taskNewsWithNullReportsMirgratedCount++;
            }

            loggerFactory.Get<NullReportConsistencyChecker>().LogError($"NullReportConsistencyChecker observed {taskNews.Length} task news and found {taskNewsWithNullReportsCount} task news with null reports, ${taskNewsWithNullReportsMirgratedCount} task news was migrated.");
        }
    }
}