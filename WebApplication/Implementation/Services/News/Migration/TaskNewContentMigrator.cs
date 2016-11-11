using System;
using System.Linq;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Content;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Sources;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Migration
{
    public class TaskNewContentMigrator
    {
        private readonly INewsFeed newsFeed;
        private readonly ITaskNewStorage taskNewStorage;
        private readonly ITaskManagerClient taskManagerClient;
        private readonly IContentParser contentParser;
        private readonly IContentSourceRepository contentSourceRepository;
        private readonly IErrorService errorService;

        public TaskNewContentMigrator(
            INewsFeed newsFeed,
            ITaskNewStorage taskNewStorage,
            ITaskManagerClient taskManagerClient,
            IContentParser contentParser,
            IContentSourceRepository contentSourceRepository,
            IErrorService errorService
            )
        {
            this.newsFeed = newsFeed;
            this.taskNewStorage = taskNewStorage;
            this.taskManagerClient = taskManagerClient;
            this.contentParser = contentParser;
            this.contentSourceRepository = contentSourceRepository;
            this.errorService = errorService;
        }

        public void Run()
        {
            var taskNews = taskNewStorage.ReadAll().Where(x => x.Content == null).ToArray();
            var totalTasksToMigration = taskNews.Length;
            int totalMigratedTasks = 0;
            foreach (var taskNew in taskNews)
            {
                try
                {
                    var boardCard = taskManagerClient.GetCard(taskNew.TaskId);
                    var contentSource = contentSourceRepository.FindOrRegister(boardCard.Id);
                    taskNew.Content = contentParser.Parse(contentSource.Id, boardCard.Name, boardCard.Description, boardCard.DueDate);
                    taskNewStorage.Update(taskNew, "add content field");
                    totalMigratedTasks ++;
                }
                catch (Exception)
                {
                }
            }
            errorService.SendError($"TaskNewContentMigrator finished. Total tasks {totalTasksToMigration}, migrated {totalMigratedTasks}.");
        }
    }
}