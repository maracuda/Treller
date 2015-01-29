﻿using System.Collections.Generic;
using SKBKontur.BlocksMapping.Abstrations;
using SKBKontur.BlocksMapping.Mappings;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.Builders;
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
                    BlockMapper.Declare<BoardsBlock, Board[]>(x => x.Boards),
                    BlockMapper.Declare<BoardsBlock, SimpleRepoBranch[]>(x => x.BranchesInCandidateRelease),
                    BlockMapper.Declare<BugsBlock, int>(x => x.BattleBugsUnassignedCount, "battleBugsUnassignedCount"),
                    BlockMapper.Declare<BugsBlock, int>(x => x.BattleBugsCount, "battleBugsCount"),
                    BlockMapper.Declare<BugsBlock, int>(x => x.CurrentBillyBugsCount, "currentBillyBugsCount"),
                    BlockMapper.Declare<BugsBlock, int>(x => x.OverallBillyBugsCount, "overallBillyBugsCount"),
                    BlockMapper.Declare<BugsBlock, int>(x => x.CurrentCSBugsCount, "currentCSBugsCount")
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