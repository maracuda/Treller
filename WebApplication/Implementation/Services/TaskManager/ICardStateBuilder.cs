using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Services.Settings;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager
{
    public interface ICardStateBuilder
    {
        CardState GetState(string boardListId, KanbanBoardMetaInfo metaInfo, BoardList[] boardLists);
    }
}