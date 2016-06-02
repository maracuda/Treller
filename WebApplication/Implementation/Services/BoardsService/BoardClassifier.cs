using System;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Services.Settings;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService
{
    public class BoardClassifier : IBoardClassifier
    {
        private readonly ITaskManagerClient taskManagerClient;

        public BoardClassifier(ITaskManagerClient taskManagerClient)
        {
            this.taskManagerClient = taskManagerClient;
        }

        public BoardType IdentifyBoardType(Board board)
        {
            var cleanBoardName = board.Name.Trim();

            if (cleanBoardName.Equals("Архив", StringComparison.OrdinalIgnoreCase))
                return BoardType.Archive;

            if (cleanBoardName.StartsWith("Стратегия"))
                return BoardType.Strategy;

            var boardLists = taskManagerClient.GetBoardLists(board.Id);
            if (KanbanBoardMetaInfo.TryParse(board, boardLists).HasValue)
                return BoardType.Kanban;

            return BoardType.Undefined;
        }
    }
}