using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.BlocksMapping.Attributes;
using SKBKontur.Infrastructure.CommonExtenssions;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.Builders;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using SKBKontur.Treller.WebApplication.Services.Settings;
using SKBKontur.Treller.WebApplication.Services.TaskCacher;
using SKBKontur.Treller.WebApplication.Extensions;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList.Builders
{
    public class TaskListBuilder
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ISettingService settingService;
        private readonly IUserAvatarViewModelBuilder userAvatarViewModelBuilder;
        private readonly ICardStageInfoBuilder cardStageInfoBuilder;
        private readonly ITaskCacher taskCacher;
        private readonly IReleaseCandidateService releaseCandidateService;

        public TaskListBuilder(ITaskManagerClient taskManagerClient,
                               ISettingService settingService,
                               IUserAvatarViewModelBuilder userAvatarViewModelBuilder,
                               ICardStageInfoBuilder cardStageInfoBuilder,
                               ITaskCacher taskCacher,
                               IReleaseCandidateService releaseCandidateService)
        {
            this.taskManagerClient = taskManagerClient;
            this.settingService = settingService;
            this.userAvatarViewModelBuilder = userAvatarViewModelBuilder;
            this.cardStageInfoBuilder = cardStageInfoBuilder;
            this.taskCacher = taskCacher;
            this.releaseCandidateService = releaseCandidateService;
        }

        [BlockModel(ContextKeys.TasksKey)]
        public Dictionary<string, BoardSettings> BuildSettings(CardListEnterModel enterModel)
        {
            if (enterModel == null || enterModel.BoardIds == null || enterModel.BoardIds.Length == 0)
            {
                return settingService.GetDevelopingBoards().ToDictionary(x => x.Id);
            }

            return settingService.GetDevelopingBoards().Where(x => enterModel.BoardIds.Contains(x.Id)).ToDictionary(x => x.Id);
        }

        [BlockModel(ContextKeys.TasksKey)]
        [BlockModelParameter("boardIds")]
        private string[] BuildSettings(Dictionary<string, BoardSettings> settings)
        {
            return settings.Select(x => x.Key).ToArray();
        }

        [BlockModel(ContextKeys.TasksKey)]
        public SimpleRepoBranch[] BuildReleaseCandidateBranches()
        {
            return releaseCandidateService.WhatBranchesInReleaseCandidate();
        }

        [BlockModel(ContextKeys.TasksKey)]
        private BoardCard[] BuildCards([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskCacher.GetCached(boardIds, ids => taskManagerClient.GetBoardCardsAsync(ids).Result, TaskCacherStoredTypes.BoardCards);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private ILookup<string, BoardList> BuildBoardLists([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskCacher.GetCached(boardIds, ids => taskManagerClient.GetBoardListsAsync(ids).Result, TaskCacherStoredTypes.BoardLists).ToLookup(x => x.BoardId);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private Dictionary<string, User> BuildUsers([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskCacher.GetCached(boardIds, ids => taskManagerClient.GetBoardUsersAsync(ids).Result, TaskCacherStoredTypes.BoardUsers).ToDictionary(x => x.Id);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private ILookup<string, CardAction> BuildCardActions([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskCacher.GetCached(boardIds, ids => taskManagerClient.GetActionsForBoardCardsAsync(ids).Result, TaskCacherStoredTypes.BoardActions).ToLookup(x => x.CardId);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private Board[] BuildBoards([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskCacher.GetCached(boardIds, ids => taskManagerClient.GetBoardsAsync(ids).Result, TaskCacherStoredTypes.Boards);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private ILookup<string, CardChecklist> BuildCardChecklists([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskCacher.GetCached(boardIds, ids => taskManagerClient.GetBoardChecklistsAsync(ids).Result, TaskCacherStoredTypes.BoardChecklists).ToLookup(x => x.CardId);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private CardStateOverallViewModel[] BuildCards(BoardCard[] cards, Dictionary<string, User> users, ILookup<string, BoardList> boardLists, 
                                                   Dictionary<string, BoardSettings> boardSettings, ILookup<string, CardAction> cardActions,
                                                   ILookup<string, CardChecklist> cardChecklists, SimpleRepoBranch[] branches)
        {
            var rcBranches = new HashSet<string>(branches.Select(x => x.Name));
            return cards.Select(card => BuildCard(users, boardLists, boardSettings, cardActions, cardChecklists, card, rcBranches))
                        .OrderByDescending(x => x.StageInfo.State)
                        .ThenByDescending(x => x.StageInfo.StageParrots.PastDays)
                        .ThenBy(x => x.StageInfo.StageParrots.BeginDate)
                        .GroupBy(x => x.StageInfo.State)
                        .Select(x => new CardStateOverallViewModel
                                        {
                                            Cards = x.ToArray(),
                                            State = x.Key,
                                            Title = x.Key.GetEnumDescription()
                                        })
                        .ToArray();
        }

        private CardListItemViewModel BuildCard(Dictionary<string, User> users, ILookup<string, BoardList> boardLists, 
                                                Dictionary<string, BoardSettings> boardSettings, ILookup<string, CardAction> cardActions,
                                                ILookup<string, CardChecklist> cardChecklists, BoardCard card, HashSet<string> rcBranches)
        {
            var stageInfo = cardStageInfoBuilder.Build(card, cardActions[card.Id].ToArray(),
                                                       cardChecklists[card.Id].ToArray(),
                                                       boardSettings[card.BoardId],
                                                       boardLists[card.BoardId].ToArray());
            
            var branchName = card.GetCardBranchName();
            var isInRc = !string.IsNullOrEmpty(branchName) && rcBranches.Contains(branchName);

            return new CardListItemViewModel
                       {
                           CardId = card.Id,
                           CardName = card.Name,
                           Labels = card.Labels.OrderBy(x => x.Color).ToArray(),
                           Avatars = card.UserIds.Select(id => users[id]).Select(userAvatarViewModelBuilder.Build).ToArray(),
                           CardUrl = card.Url,
                           StageInfo = stageInfo,
                           IsNewCard = stageInfo.StageParrots.BeginDate.HasValue && stageInfo.StageParrots.BeginDate.Value.Date == DateTime.Now.Date,
                           BranchName = branchName,
                           IsInCandidateRelease = isInRc
                       };
        }
    }
}