using System.Collections.Generic;
using System.Linq;
using SKBKontur.Billy.Core.BlocksMapping.Attributes;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.Builders;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using SKBKontur.Treller.WebApplication.Services.Settings;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList.Builders
{
    public class TaskListBuilder
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ISettingService settingService;
        private readonly IUserAvatarViewModelBuilder userAvatarViewModelBuilder;
        private readonly ICardStageInfoBuilder cardStageInfoBuilder;

        public TaskListBuilder(ITaskManagerClient taskManagerClient,
                               ISettingService settingService,
                               IUserAvatarViewModelBuilder userAvatarViewModelBuilder,
                               ICardStageInfoBuilder cardStageInfoBuilder)
        {
            this.taskManagerClient = taskManagerClient;
            this.settingService = settingService;
            this.userAvatarViewModelBuilder = userAvatarViewModelBuilder;
            this.cardStageInfoBuilder = cardStageInfoBuilder;
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
        private ILookup<string, CardChecklist> BuildCardChecklists([BlockModelParameter("boardIds")] string[] boardIds)
        {
            return taskManagerClient.GetBoardChecklists(boardIds).ToLookup(x => x.CardId);
        }

        [BlockModel(ContextKeys.TasksKey)]
        private CardListItemViewModel[] BuildCards(BoardCard[] cards, Dictionary<string, User> users, ILookup<string, BoardList> boardLists, 
                                                   Dictionary<string, BoardSettings> boardSettings, ILookup<string, CardAction> cardActions,
                                                   ILookup<string, CardChecklist> cardChecklists)
        {
            return cards.Select(card => new CardListItemViewModel
                                            {
                                                CardId = card.Id,
                                                CardName = card.Name,
                                                Labels = card.Labels.OrderBy(x => x.Color).ToArray(),
                                                Avatars = card.UserIds.Select(id => users[id]).Select(userAvatarViewModelBuilder.Build).ToArray(),
                                                CardUrl = card.Url,
                                                StageInfo = cardStageInfoBuilder.Build(card,
                                                                                       cardActions[card.Id].ToArray(),
                                                                                       cardChecklists[card.Id].ToArray(),
                                                                                       boardSettings[card.BoardId],
                                                                                       boardLists[card.BoardId].ToArray())
                                            })
                        .OrderByDescending(x => x.StageInfo.State)
                        .ToArray();
        }
    }
}