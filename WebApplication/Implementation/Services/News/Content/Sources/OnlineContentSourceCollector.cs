using System;
using System.Linq;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Sources
{
    public class OnlineContentSourceCollector : IContentSourceCollector
    {
        private readonly IBoardsService boardsService;
        private readonly ITaskManagerClient taskManagerClient;
        private readonly IContentSourceRepository contentSourceRepository;

        public OnlineContentSourceCollector(
            IBoardsService boardsService,
            ITaskManagerClient taskManagerClient,
            IContentSourceRepository contentSourceRepository)
        {
            this.boardsService = boardsService;
            this.taskManagerClient = taskManagerClient;
            this.contentSourceRepository = contentSourceRepository;
        }

        public void Collect()
        {
            var boardIds = boardsService.SelectKanbanBoards(false).Select(x => x.Id).ToArray();
            var boardsLists = taskManagerClient.GetBoardLists(boardIds)
                .Where(x => string.Equals(x.Name, KanbanBoardTemplate.TestingListName, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(x.Name, KanbanBoardTemplate.WaitForReleaseListName, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(x.Name, KanbanBoardTemplate.ReleasedListName, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            foreach (var boardsList in boardsLists)
            {
                foreach (var cardInfo in boardsList.Cards)
                {
                    if (!contentSourceRepository.Contains(cardInfo.Id))
                    {
                        contentSourceRepository.FindOrRegister(cardInfo.Id);
                    }
                }
            }
        }
    }
}