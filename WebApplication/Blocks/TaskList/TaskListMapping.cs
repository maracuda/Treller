using SKBKontur.BlocksMapping.Abstrations;
using SKBKontur.BlocksMapping.Mappings;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.Builders;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList
{
    public class TaskListMapping : IContextBlocksMapping
    {
        private static readonly IBlockMapper[] Mappers =
            new IBlockMapper[]
                {
                    BlockMapper.Declare<CardListBlock, CardStateOverallViewModel[]>(x => x.OverallStateCards),
                    BlockMapper.Declare<BoardsBlock, Board[]>(x => x.Boards),
                    BlockMapper.Declare<BoardsBlock, SimpleRepoBranch[]>(x => x.BranchesInCandidateRelease)
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