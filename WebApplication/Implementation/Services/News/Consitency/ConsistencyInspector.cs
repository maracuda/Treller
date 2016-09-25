using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Search;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Consitency
{
    public class ConsistencyInspector : IConsistencyIspector
    {
        private readonly ITaskNewIndex taskNewIndex;
        private readonly ITaskNewStorage taskNewStorage;
        private readonly ITaskNewConverter taskNewConverter;
        private readonly IErrorService errorService;

        public ConsistencyInspector(
            ITaskNewIndex taskNewIndex,
            ITaskNewStorage taskNewStorage,
            ITaskNewConverter taskNewConverter,
            IErrorService errorService)
        {
            this.taskNewIndex = taskNewIndex;
            this.taskNewStorage = taskNewStorage;
            this.taskNewConverter = taskNewConverter;
            this.errorService = errorService;
        }

        public void Run()
        {
            var taskNewsWithEmptyText = taskNewIndex.SelectCurrentNews()
                .Where(x => string.IsNullOrWhiteSpace(x.Text))
                .ToArray();

            errorService.SendError($"Consistency inspector found {taskNewsWithEmptyText.Length} news with empty text.");

            if (taskNewsWithEmptyText.Length > 0)
            {
                var taskNewsToDelete = taskNewsWithEmptyText.Select(x => taskNewConverter.Project(x)).ToArray();
                taskNewStorage.Delete(taskNewsToDelete);
                errorService.SendError($"{taskNewsWithEmptyText.Length} news with empty text was deleted.");
            }
        }
    }
}