using System.Collections.Generic;
using SKBKontur.Billy.Core.BlocksMapping.Abstrations;
using SKBKontur.Billy.Core.BlocksMapping.Mappings;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList
{
    public class TaskListMapping : IContextBlocksMapping
    {
        private static readonly IBlockMapper[] Mappers =
            new IBlockMapper[]
                {
                    BlockMapper.Declare<CardListBlock, Dictionary<CardState, CardStateOverallViewModel>>(x => x.OverallStateCards),
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