using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService
{
    public interface IBoardsService
    {
        Board[] SelectKanbanBoards(bool includeClosed);
    }
}