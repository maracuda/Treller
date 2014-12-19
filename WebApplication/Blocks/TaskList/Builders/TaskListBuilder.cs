using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.Billy.Core.BlocksMapping.Attributes;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.Builders;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using SKBKontur.Treller.WebApplication.Services.Settings;
using SKBKontur.Infrastructure.CommonExtenssions;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList.Builders
{
    public class TaskListBuilder
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ISettingService settingService;
        private readonly ICardStateBuilder cardStateBuilder;
        private readonly IUserAvatarViewModelBuilder userAvatarViewModelBuilder;

        public TaskListBuilder(ITaskManagerClient taskManagerClient,
                               ISettingService settingService, 
                               ICardStateBuilder cardStateBuilder,
                               IUserAvatarViewModelBuilder userAvatarViewModelBuilder)
        {
            this.taskManagerClient = taskManagerClient;
            this.settingService = settingService;
            this.cardStateBuilder = cardStateBuilder;
            this.userAvatarViewModelBuilder = userAvatarViewModelBuilder;
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
            return taskManagerClient.GetBoardCards(boardIds).ToArray();
        }

        [BlockModel(ContextKeys.TasksKey)]
        private ILookup<string, BoardList> BuildBoardLists([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskManagerClient.GetBoardLists(boardIds).ToLookup(x => x.BoardId);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private Dictionary<string, User> BuildUsers([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskManagerClient.GetBoardUsers(boardIds).ToDictionary(x => x.Id);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private ILookup<string, CardAction> BuildCardActions([BlockModelParameter("boardIds")] string[] boardIds)
        {
            var result = taskManagerClient.GetActionsForBoardCards(boardIds).ToArray();
            return result.ToLookup(x => x.CardId);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private Board[] BuildBoards([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskManagerClient.GetBoards(boardIds).ToArray();
        }

        [BlockModel(ContextKeys.TasksKey)]
        private CardListItemViewModel[] BuildCards(BoardCard[] cards, Dictionary<string, User> users, ILookup<string, BoardList> boardLists, 
                                                   Dictionary<string, BoardSettings> boardSettings, ILookup<string, CardAction> cardActions)
        {
            return cards.Select(card =>
                                    {
                                        var state = cardStateBuilder.GetState(card.BoardListId, boardSettings[card.BoardId], boardLists[card.BoardId].ToArray());
                                        return new CardListItemViewModel
                                                       {
                                                           CardId = card.Id,
                                                           CardName = card.Name,
                                                           Labels = card.Labels,
                                                           Avatars = card.UserIds.Select(id => users[id]).Select(userAvatarViewModelBuilder.Build).ToArray(),
                                                           StageInfo = GetStageInfo(card, state, cardActions[card.Id].ToArray()),
                                                           CardUrl = card.Url,
                                                           State = state
                                                       };
                                    })
                        .OrderByDescending(x => x.State)
                        .ToArray();
        }

        private static string GetStageInfo(BoardCard card, CardState state, CardAction[] actions)
        {
            var lastListChangedDate = actions.FirstOrDefault(x => x.ToListId == card.BoardListId);
            var totalDays = lastListChangedDate != null ? (DateTime.Now.Date - lastListChangedDate.Date).TotalDays : 0;
            var percent = card.CheckLists.Any(cl => cl.Items.Any(i => i.IsChecked)) 
                            ? card.CheckLists.Sum(l => l.Items.Count(i => i.IsChecked)) / card.CheckLists.Sum(l => l.Items.Length)
                            : 0;

            return string.Format("Стадия:{0}, {1} дн., {2:P},. Последняя активность:{3:F}", state.GetEnumDescription(), totalDays, percent, card.LastActivity);
        }
    }
}