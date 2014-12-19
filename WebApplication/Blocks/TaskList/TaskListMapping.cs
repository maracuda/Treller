using SKBKontur.Billy.Core.BlocksMapping.Abstrations;
using SKBKontur.Billy.Core.BlocksMapping.Mappings;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using System.Linq;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList
{
    public class TaskListMapping : IContextBlocksMapping
    {
        private static readonly IBlockMapper[] Mappers =
            new IBlockMapper[]
                {
                    BlockMapper.Declare<CardListBlock, CardListItemViewModel[]>(x => x.Cards),
                    BlockMapper.Declare<CardOverallBlock, BoardCard[], int>(x => x.TotalCount, x => x.Length),
                    BlockMapper.Declare<CardOverallBlock, CardListItemViewModel[], int>(x => x.DevelopCount, x => x.Count(c => c.State >= CardState.Develop && c.State < CardState.Testing)),
                    BlockMapper.Declare<CardOverallBlock, CardListItemViewModel[], int>(x => x.TestingCount, x => x.Count(c => c.State == CardState.Testing || c.State == CardState.ReleaseWaiting)),
                    BlockMapper.Declare<CardOverallBlock, CardListItemViewModel[], int>(x => x.AnaliticsCount, x => x.Count(c => c.State == CardState.BeforeDevelop)),
                    BlockMapper.Declare<BoardsBlock, Board[]>(x => x.Boards)
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