using System;
using TaskManagerClient;
using TaskManagerClient.BusinessObjects.TaskManager;

namespace WebApplication.Implementation.Services.BoardsService
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
            if (KanbanBoardTemplate.Matches(board, boardLists))
                return BoardType.Kanban;

            return BoardType.Undefined;
        }
    }
}