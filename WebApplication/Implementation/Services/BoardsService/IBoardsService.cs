using TaskManagerClient.BusinessObjects.TaskManager;

namespace WebApplication.Implementation.Services.BoardsService
{
    public interface IBoardsService
    {
        Board[] SelectKanbanBoards(bool includeClosed);
    }
}