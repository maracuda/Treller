using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Actualization;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Import
{
    public class NewsImporter : INewsImporter
    {
        private readonly IBoardsService boardsService;
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ITaskNewConverter taskNewConverter;
        private readonly ITaskNewStorage taskNewStorage;
        private readonly IAgingCardsFilter agingCardsFilter;

        public NewsImporter(
            IBoardsService boardsService,
            ITaskManagerClient taskManagerClient,
            ITaskNewConverter taskNewConverter,
            ITaskNewStorage taskNewStorage,
            IAgingCardsFilter agingCardsFilter)
        {
            this.boardsService = boardsService;
            this.taskManagerClient = taskManagerClient;
            this.taskNewConverter = taskNewConverter;
            this.taskNewStorage = taskNewStorage;
            this.agingCardsFilter = agingCardsFilter;
        }

        public void ImportAll()
        {
            var boardIds = boardsService.SelectKanbanBoards(false).Select(x => x.Id).ToArray();
            var boardsLists = taskManagerClient.GetBoardLists(boardIds)
                                               .Where(x => string.Equals(x.Name, KanbanBoardTemplate.TestingListName, StringComparison.OrdinalIgnoreCase) ||
                                                           string.Equals(x.Name, KanbanBoardTemplate.WaitForReleaseListName, StringComparison.OrdinalIgnoreCase) ||
                                                           string.Equals(x.Name, KanbanBoardTemplate.ReleasedListName, StringComparison.OrdinalIgnoreCase))
                                               .ToArray();

            var newsList = new List<TaskNew>();
            foreach (var boardsList in boardsLists)
            {
                foreach (var boardListCardInfo in boardsList.Cards)
                {
                    var taskNews = taskNewConverter.Convert(boardsList.BoardId, boardListCardInfo);
                    var freshNews = agingCardsFilter.FilterFresh(taskNews);
                    newsList.AddRange(freshNews);
                }
            }

            foreach (var taskNew in newsList)
            {
                var existentTaskNews = taskNewStorage.Find(taskNew.TaskId);
                if (existentTaskNews.HasNoValue)
                {
                    taskNewStorage.Create(taskNew);
                }
                else
                {
                    var exisitentTask = existentTaskNews.Value.FirstOrDefault(x => x.DeliveryChannel == taskNew.DeliveryChannel);
                    if (exisitentTask == null)
                    {
                        taskNewStorage.Create(taskNew);
                    }
                    else
                    {
                        if (!exisitentTask.Delivered)
                        {
                            var newsDiff = exisitentTask.BuildDiff(taskNew);
                            if (!string.IsNullOrEmpty(newsDiff))
                            {
                                taskNewStorage.Update(taskNew, newsDiff);
                            }
                        }
                    }
                }
            }
        }
    }
}