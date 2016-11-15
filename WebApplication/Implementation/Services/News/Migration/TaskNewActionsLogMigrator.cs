using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Migration
{
    public class TaskNewActionsLogMigrator
    {
        private readonly IFileSystemHandler fileSystemHandler;
        private readonly IErrorService errorService;

        public TaskNewActionsLogMigrator(
            IFileSystemHandler fileSystemHandler,
            IErrorService errorService)
        {
            this.fileSystemHandler = fileSystemHandler;
            this.errorService = errorService;
        }

        public void Migrate()
        {
            fileSystemHandler.Delete("TaskNewsActionLogs");
            errorService.SendError("TaskNewActionsLogMigrator successfully finished.");
        }
    }
}