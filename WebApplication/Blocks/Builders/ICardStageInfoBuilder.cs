using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using SKBKontur.Treller.WebApplication.Implementation.Services.Settings;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public interface ICardStageInfoBuilder
    {
        CardStageInfoViewModel Build(BoardCard card, CardAction[] actions, CardChecklist[] checklists, BoardSettings boardSetting, BoardList[] boardLists);
    }
}