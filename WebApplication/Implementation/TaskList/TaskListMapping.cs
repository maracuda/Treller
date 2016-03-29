using SKBKontur.BlocksMapping.Abstrations;
using SKBKontur.BlocksMapping.Mappings;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Abstractions;
using SKBKontur.Treller.WebApplication.Implementation.Services.Repository;
using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.Blocks;
using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskList
{
    public class TaskListMapping : IContextBlocksMapping
    {
        private static readonly IBlockMapper[] Mappers =
            new IBlockMapper[]
                {
                    BlockMapper.Declare<CardListBlock, CardStateOverallViewModel[]>(x => x.OverallStateCards),
                    BlockMapper.Declare<BoardsBlock, Board[]>(x => x.Boards),
                    BlockMapper.Declare<BoardsBlock, SimpleRepoBranch[]>(x => x.BranchesInCandidateRelease),
                    BlockMapper.Declare<BugsBlock, BugsCountLinkInfoViewModel>(x => x.BattleUnassigned, "battleBugsUnassignedCount"),
                    BlockMapper.Declare<BugsBlock, BugsCountLinkInfoViewModel>(x => x.BattleAssigned, "battleBugsCount"),
                    BlockMapper.Declare<BugsBlock, BugsCountLinkInfoViewModel>(x => x.BillyCurrent, "currentBillyBugsCount"),
                    BlockMapper.Declare<BugsBlock, BugsCountLinkInfoViewModel>(x => x.BillyAll, "overallBillyBugsCount"),
                    BlockMapper.Declare<BugsBlock, BugsCountLinkInfoViewModel>(x => x.CsCurrent, "currentCSBugsCount"),
                    BlockMapper.Declare<BugsBlock, BugsCountLinkInfoViewModel>(x => x.BillyNotVerified, "billyNotVerified")
                };

        public IBlockMapper[] SelectAll()
        {
            return Mappers;
        }

        public string GetContextKey()
        {
            return ContextKeys.TasksKey;
        }
    }
}