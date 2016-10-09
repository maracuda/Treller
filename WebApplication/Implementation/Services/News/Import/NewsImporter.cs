using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.Infrastructure.Sugar;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Import
{
    public class NewsImporter : INewsImporter
    {
        private readonly IBoardsService boardsService;
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ITaskNewConverter taskNewConverter;
        private readonly INewsFeed newsFeed;

        public NewsImporter(
            IBoardsService boardsService,
            ITaskManagerClient taskManagerClient,
            ITaskNewConverter taskNewConverter,
            INewsFeed newsFeed)
        {
            this.boardsService = boardsService;
            this.taskManagerClient = taskManagerClient;
            this.taskNewConverter = taskNewConverter;
            this.newsFeed = newsFeed;
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
                var taskNews = taskNewConverter.Convert(boardsList);
                newsList.AddRange(taskNews);
            }

            newsFeed.AddNews(newsList);
        }

        public Maybe<string> TryImport(string trelloCardId)
        {
            try
            {
                var card = taskManagerClient.GetCard(trelloCardId);
                var cardList = taskManagerClient.GetBoardLists(card.BoardId).FirstOrDefault(l => l.Id.Equals(card.BoardListId));
                if (cardList == null)
                {
                    return $"Не удалось испортировать карточку {trelloCardId} так как не нашли список в котором она находится";
                }

                if (string.Equals(cardList.Name, KanbanBoardTemplate.TestingListName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(cardList.Name, KanbanBoardTemplate.WaitForReleaseListName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(cardList.Name, KanbanBoardTemplate.ReleasedListName, StringComparison.OrdinalIgnoreCase))
                {
                    var taskNews = taskNewConverter.Convert(card.BoardId, card.Id, card.Name, card.Description, card.DueDate);
                    newsFeed.AddNews(taskNews);
                    return null;
                }
                return $"Не удалось испортировать карточку {trelloCardId} так как она находится не в списке карточек готовых к релизу.";
            }
            catch (Exception e)
            {
                return $"Не удалось испортировать карточку {trelloCardId} из-за непредвиденной ошибки. Сообщение об ошибке: {e.Message}.";
            }
        }
    }
}