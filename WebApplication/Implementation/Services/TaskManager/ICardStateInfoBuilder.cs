using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Services.Settings;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager
{
    public interface ICardStateInfoBuilder
    {
        CardStateInfo Build(CardAction[] actions, Dictionary<string, KanbanBoardMetaInfo> boardSettings, Dictionary<string, BoardList[]> boardLists);
    }
}