using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager
{
    public interface ICardStageInfoBuilder
    {
        CardStageInfoViewModel Build(BoardCard card, CardAction[] actions, CardChecklist[] checklists, BoardList[] boardLists);
    }
}