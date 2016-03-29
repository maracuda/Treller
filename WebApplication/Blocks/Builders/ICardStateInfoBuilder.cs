using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Services.Settings;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public interface ICardStateInfoBuilder
    {
        CardStateInfo Build(CardAction[] actions, Dictionary<string, BoardSettings> boardSettings, Dictionary<string, BoardList[]> boardLists);
    }
}