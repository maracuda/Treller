using System;
using System.Collections.Generic;
using SKBKontur.Billy.Core.BlocksMapping.Attributes;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.Builders;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks.Parts;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.ViewModels;
using SKBKontur.Treller.WebApplication.Services.Settings;
using System.Linq;
using SKBKontur.Infrastructure.CommonExtenssions;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Builders
{
    public class TasksBuilder
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ISettingService settingService;
        private readonly ICardStateInfoBuilder cardStateInfoBuilder;
        private readonly IUserAvatarViewModelBuilder userAvatarViewModelBuilder;
        private readonly ICardStateBuilder cardStateBuilder;

        public TasksBuilder(ITaskManagerClient taskManagerClient, 
                            ISettingService settingService,
                            ICardStateInfoBuilder cardStateInfoBuilder,
                            IUserAvatarViewModelBuilder userAvatarViewModelBuilder,
                            ICardStateBuilder cardStateBuilder)
        {
            this.taskManagerClient = taskManagerClient;
            this.settingService = settingService;
            this.cardStateInfoBuilder = cardStateInfoBuilder;
            this.userAvatarViewModelBuilder = userAvatarViewModelBuilder;
            this.cardStateBuilder = cardStateBuilder;
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private Dictionary<string, BoardSettings> BuildBoardSettings()
        {
            return settingService.GetDevelopingBoards().ToDictionary(x => x.Id);
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private BoardCard BuildCard(string cardId)
        {
            return taskManagerClient.GetCard(cardId);
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private Dictionary<string, User> BuildCardUsers(string cardId)
        {
            return taskManagerClient.GetCardUsers(cardId).ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardAction[] BuildCardActions(string cardId)
        {
            return taskManagerClient.GetCardActions(cardId).ToArray();
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private Dictionary<string, BoardList[]> BuildBoardLists(BoardCard card)
        {
            return taskManagerClient.GetBoardLists(card.BoardId).GroupBy(x => x.BoardId).ToDictionary(x => x.Key, x => x.ToArray(), StringComparer.OrdinalIgnoreCase);
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private Dictionary<bool, UserAvatarViewModel[]> BuildUserActivity(Dictionary<string, User> users, CardStateInfo stateInfo)
        {
            if (stateInfo.CurrentState == CardState.Unknown)
            {
                return new Dictionary<bool, UserAvatarViewModel[]> { { false, users.Select(u => userAvatarViewModelBuilder.Build(u.Value)).ToArray() } };
            }

            var activeIds = new HashSet<string>(stateInfo.States[stateInfo.CurrentState].NewStateUsers.Select(u => u.Id), StringComparer.OrdinalIgnoreCase);
            return users.GroupBy(x => activeIds.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Select(u => userAvatarViewModelBuilder.Build(u.Value)).ToArray());
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardStateInfo BuildCardStateInfo(CardAction[] actions, Dictionary<string, BoardSettings> boardSettings, Dictionary<string, BoardList[]> boardLists)
        {
            return cardStateInfoBuilder.Build(actions, boardSettings, boardLists);
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardState BuildCardState(BoardCard card, Dictionary<string, BoardSettings> boardSettings, Dictionary<string, BoardList[]> boardLists)
        {
            if (!boardSettings.ContainsKey(card.BoardId) || !boardLists.ContainsKey(card.BoardId))
            {
                return CardState.Unknown;
            }

            return cardStateBuilder.GetState(card.BoardListId, boardSettings[card.BoardId], boardLists[card.BoardId]);
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardBeforeDevelopPartBlock BuildBeforeDevelopingBlock(CardStateInfo stateInfo, BoardCard card)
        {
            var result = CreateBasePart<CardBeforeDevelopPartBlock>(stateInfo, CardState.BeforeDevelop, card);

            
            return result;
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardDevelopPartBlock BuildDevelopingBlock(CardStateInfo stateInfo, BoardCard card)
        {
            var result = CreateBasePart<CardDevelopPartBlock>(stateInfo, CardState.BeforeDevelop, card);


            return result;
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardPresentationPartBlock BuildPresentationBlock(CardStateInfo stateInfo, BoardCard card)
        {
            var result = CreateBasePart<CardPresentationPartBlock>(stateInfo, CardState.BeforeDevelop, card);


            return result;
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardArchivePartBlock BuildArchiveBlock(CardStateInfo stateInfo, BoardCard card)
        {
            var result = CreateBasePart<CardArchivePartBlock>(stateInfo, CardState.BeforeDevelop, card);


            return result;
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardReleaseWaitingPartBlock BuildReleaseWaitingBlock(CardStateInfo stateInfo, BoardCard card)
        {
            var result = CreateBasePart<CardReleaseWaitingPartBlock>(stateInfo, CardState.BeforeDevelop, card);


            return result;
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardReviewPartBlock BuildReviewBlock(CardStateInfo stateInfo, BoardCard card)
        {
            var result = CreateBasePart<CardReviewPartBlock>(stateInfo, CardState.BeforeDevelop, card);


            return result;
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardTestingPartBlock BuildTestingBlock(CardStateInfo stateInfo, BoardCard card)
        {
            var result = CreateBasePart<CardTestingPartBlock>(stateInfo, CardState.BeforeDevelop, card);


            return result;
        }

        private static T CreateBasePart<T>(CardStateInfo state, CardState baseState, BoardCard card) where T : BasePartTaskDetalizationBlock
        {
            if (!state.States.ContainsKey(baseState))
            {
                return null;
            }

            var stateInfo = state.States[baseState];
            var result = Activator.CreateInstance<T>();

//            result.CardId = card.Id;
            result.IsExists = true;
            result.IsCurrent = stateInfo.State == state.CurrentState;
            result.BlockDisclamer = stateInfo.State.GetEnumDescription();
            result.PartUsers = stateInfo.NewStateUsers.Select(x => x.FullName).ToArray();
            result.PartBeginDate = stateInfo.BeginDate;
            result.PartEndDate = stateInfo.EndDate;
            result.PartDueDays = 0; // Эта вроде была идея сделать нашей настройкой
            return result;
        }
    }
}