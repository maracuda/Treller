using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Migration
{
    public class NullReportConsistencyChecker
    {
        private readonly ITaskNewStorage taskNewStorage;
        private readonly IErrorService errorService;

        public NullReportConsistencyChecker(
            ITaskNewStorage taskNewStorage,
            IErrorService errorService)
        {
            this.taskNewStorage = taskNewStorage;
            this.errorService = errorService;
        }

        public void Run()
        {
            var taskNews = taskNewStorage.ReadAll();
            var taskNewsWithNullReportsCount = taskNews.Count(t => t.Reports.Any(r => r == null));
            errorService.SendError($"NullReportConsistencyChecker observed {taskNews.Length} task news and found {taskNewsWithNullReportsCount} task news with null reports.");
        }
    }
}