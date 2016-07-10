using System.Collections.Generic;
using System.Linq;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService
{
    public class BoardsService : IBoardsService
    {
        private const string organizationName = "konturbilling";
        private readonly ITaskManagerClient taskManagerClient;
        private readonly IBoardClassifier boardClassifier;

        public BoardsService(
            ITaskManagerClient taskManagerClient,
            IBoardClassifier boardClassifier)
        {
            this.taskManagerClient = taskManagerClient;
            this.boardClassifier = boardClassifier;
        }

        public Board[] SelectKanbanBoards(bool includeClosed)
        {
            IEnumerable<Board> boards = taskManagerClient.GetAllBoards(organizationName);
            if (!includeClosed)
            {
                boards = boards.Where(x => !x.IsClosed);
            }

            return boards.Where(board => boardClassifier.IdentifyBoardType(board) == BoardType.Kanban)
                         .ToArray();
        }
    }
}