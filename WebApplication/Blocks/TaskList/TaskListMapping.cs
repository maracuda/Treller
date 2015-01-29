using SKBKontur.BlocksMapping.Abstrations;
using SKBKontur.BlocksMapping.Mappings;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.Builders;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList
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
                    BlockMapper.Declare<BugsBlock, BugsCountLinkInfoViewModel>(x => x.CsCurrent, "currentCSBugsCount")
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