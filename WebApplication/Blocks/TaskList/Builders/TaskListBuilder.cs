using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.Billy.Core.BlocksMapping.Attributes;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.Builders;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using SKBKontur.Treller.WebApplication.Services.Settings;
using SKBKontur.Treller.WebApplication.Services.TaskCacher;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList.Builders
{
    public class TaskListBuilder
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ISettingService settingService;
        private readonly IUserAvatarViewModelBuilder userAvatarViewModelBuilder;
        private readonly ICardStageInfoBuilder cardStageInfoBuilder;
        private readonly ITaskCacher taskCacher;

        public TaskListBuilder(ITaskManagerClient taskManagerClient,
                               ISettingService settingService,
                               IUserAvatarViewModelBuilder userAvatarViewModelBuilder,
                               ICardStageInfoBuilder cardStageInfoBuilder,
                               ITaskCacher taskCacher)
        {
            this.taskManagerClient = taskManagerClient;
            this.settingService = settingService;
            this.userAvatarViewModelBuilder = userAvatarViewModelBuilder;
            this.cardStageInfoBuilder = cardStageInfoBuilder;
            this.taskCacher = taskCacher;
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
        private BoardCard[] BuildCards([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskCacher.GetCached(boardIds, taskManagerClient.GetBoardCards, TaskCacherStoredTypes.BoardCards);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private ILookup<string, BoardList> BuildBoardLists([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskCacher.GetCached(boardIds, taskManagerClient.GetBoardLists, TaskCacherStoredTypes.BoardLists).ToLookup(x => x.BoardId);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private Dictionary<string, User> BuildUsers([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskCacher.GetCached(boardIds, taskManagerClient.GetBoardUsers, TaskCacherStoredTypes.BoardUsers).ToDictionary(x => x.Id);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private ILookup<string, CardAction> BuildCardActions([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskCacher.GetCached(boardIds, ids => taskManagerClient.GetActionsForBoardCards(ids), TaskCacherStoredTypes.BoardActions).ToLookup(x => x.CardId);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private Board[] BuildBoards([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskCacher.GetCached(boardIds, taskManagerClient.GetBoards, TaskCacherStoredTypes.Boards);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private ILookup<string, CardChecklist> BuildCardChecklists([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskCacher.GetCached(boardIds, taskManagerClient.GetBoardChecklists, TaskCacherStoredTypes.BoardChecklists).ToLookup(x => x.CardId);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private Dictionary<CardState, CardStateOverallViewModel> BuildCards(BoardCard[] cards, Dictionary<string, User> users, ILookup<string, BoardList> boardLists, 
                                                   Dictionary<string, BoardSettings> boardSettings, ILookup<string, CardAction> cardActions,
                                                   ILookup<string, CardChecklist> cardChecklists)
        {
            return cards.Select(card => BuildCard(users, boardLists, boardSettings, cardActions, cardChecklists, card))
                        .OrderByDescending(x => x.StageInfo.State)
                        .ThenByDescending(x => x.StageInfo.StageParrots.PastDays)
                        .ThenBy(x => x.StageInfo.StageParrots.BeginDate)
                        .GroupBy(x => x.StageInfo.State)
                        .ToDictionary(x => x.Key, x => new CardStateOverallViewModel
                                                                       {
                                                                           Cards = x.ToArray(),
                                                                           State = x.Key
                                                                       });
        }

        private CardListItemViewModel BuildCard(Dictionary<string, User> users, ILookup<string, BoardList> boardLists, Dictionary<string, BoardSettings> boardSettings,
                                                ILookup<string, CardAction> cardActions, ILookup<string, CardChecklist> cardChecklists, BoardCard card)
        {
            var stageInfo = cardStageInfoBuilder.Build(card, cardActions[card.Id].ToArray(),
                                                       cardChecklists[card.Id].ToArray(),
                                                       boardSettings[card.BoardId],
                                                       boardLists[card.BoardId].ToArray());
            return new CardListItemViewModel
                       {
                           CardId = card.Id,
                           CardName = card.Name,
                           Labels = card.Labels.OrderBy(x => x.Color).ToArray(),
                           Avatars = card.UserIds.Select(id => users[id]).Select(userAvatarViewModelBuilder.Build).ToArray(),
                           CardUrl = card.Url,
                           StageInfo = stageInfo,
                           IsNewCard = stageInfo.StageParrots.BeginDate.HasValue && stageInfo.StageParrots.BeginDate.Value.Date == DateTime.Now.Date
                       };
        }
    }
}