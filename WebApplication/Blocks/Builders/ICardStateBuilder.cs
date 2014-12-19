using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Services.Settings;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public interface ICardStateBuilder
    {
        CardState GetState(string boardListId, BoardSettings setting, BoardList[] boardLists);
    }
}