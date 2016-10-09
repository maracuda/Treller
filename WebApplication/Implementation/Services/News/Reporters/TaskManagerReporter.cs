using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.Infrastructure.Sugar;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Reporters
{
    public class TaskManagerReporter : IReporter
    {
        private readonly IBoardsService boardsService;
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ITaskNewConverter taskNewConverter;

        public TaskManagerReporter(
            IBoardsService boardsService,
            ITaskManagerClient taskManagerClient,
            ITaskNewConverter taskNewConverter)
        {
            this.boardsService = boardsService;
            this.taskManagerClient = taskManagerClient;
            this.taskNewConverter = taskNewConverter;
        }

        public IEnumerable<TaskNew> MakeReport()
        {
            var boardIds = boardsService.SelectKanbanBoards(false).Select(x => x.Id).ToArray();
            var boardsLists = taskManagerClient.GetBoardLists(boardIds)
                                               .Where(x => string.Equals(x.Name, KanbanBoardTemplate.TestingListName, StringComparison.OrdinalIgnoreCase) ||
                                                           string.Equals(x.Name, KanbanBoardTemplate.WaitForReleaseListName, StringComparison.OrdinalIgnoreCase) ||
                                                           string.Equals(x.Name, KanbanBoardTemplate.ReleasedListName, StringComparison.OrdinalIgnoreCase))
                                               .ToArray();

            var result = new List<TaskNew>();
            foreach (var boardsList in boardsLists)
            {
                var taskNews = taskNewConverter.Convert(boardsList);
                result.AddRange(taskNews);
            }

            return result;
        }

        public Maybe<IEnumerable<TaskNew>> TryToMakeReport(string aboutCardId)
        {
            try
            {
                var card = taskManagerClient.GetCard(aboutCardId);
                var cardList = taskManagerClient.GetBoardLists(card.BoardId).FirstOrDefault(l => l.Id.Equals(card.BoardListId));
                if (cardList == null)
                {
                    return null;
                }

                if (string.Equals(cardList.Name, KanbanBoardTemplate.TestingListName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(cardList.Name, KanbanBoardTemplate.WaitForReleaseListName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(cardList.Name, KanbanBoardTemplate.ReleasedListName, StringComparison.OrdinalIgnoreCase))
                {
                    return taskNewConverter.Convert(card.BoardId, card.Id, card.Name, card.Description, card.DueDate);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}