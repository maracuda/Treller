using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SKBKontur.BlocksMapping.Attributes;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Blocks.Builders;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks.Parts;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.ViewModels;
using SKBKontur.Treller.WebApplication.Services.Settings;
using System.Linq;
using SKBKontur.Infrastructure.CommonExtenssions;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Services.TaskCacher;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Builders
{
    public class TasksBuilder
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ISettingService settingService;
        private readonly ICardStateInfoBuilder cardStateInfoBuilder;
        private readonly IUserAvatarViewModelBuilder userAvatarViewModelBuilder;
        private readonly ICardStateBuilder cardStateBuilder;
        private readonly ITaskCacher taskCacher;
        private readonly IChecklistParrotsBuilder checklistParrotsBuilder;
        private readonly IBugsBuilder bugsBuilder;

        public TasksBuilder(ITaskManagerClient taskManagerClient, 
                            ISettingService settingService,
                            ICardStateInfoBuilder cardStateInfoBuilder,
                            IUserAvatarViewModelBuilder userAvatarViewModelBuilder,
                            ICardStateBuilder cardStateBuilder,
                            ITaskCacher taskCacher,
                            IChecklistParrotsBuilder checklistParrotsBuilder,
                            IBugsBuilder bugsBuilder)
        {
            this.taskManagerClient = taskManagerClient;
            this.settingService = settingService;
            this.cardStateInfoBuilder = cardStateInfoBuilder;
            this.userAvatarViewModelBuilder = userAvatarViewModelBuilder;
            this.cardStateBuilder = cardStateBuilder;
            this.taskCacher = taskCacher;
            this.checklistParrotsBuilder = checklistParrotsBuilder;
            this.bugsBuilder = bugsBuilder;
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private Dictionary<string, BoardSettings> BuildBoardSettings()
        {
            return settingService.GetDevelopingBoards().ToDictionary(x => x.Id);
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private async Task<BoardCard> BuildCard(string cardId)
        {
            return FindCard(cardId)
                   ?? await taskManagerClient.GetCardAsync(cardId);
        }

        private BoardCard FindCard(string cardId)
        {
            return taskCacher.GetBuilded<BoardCard>().FirstOrDefault(x => string.Equals(x.Id, cardId, StringComparison.OrdinalIgnoreCase));
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private async Task<Dictionary<string, User>> BuildCardUsers(string cardId)
        {
            var card = FindCard(cardId);
            if (card != null)
            {
                if (card.UserIds.Length == 0)
                {
                    return new Dictionary<string, User>(0);
                }

                var result = taskCacher.GetBuilded<User>()
                                 .Where(x => card.UserIds.Contains(x.Id, StringComparer.OrdinalIgnoreCase))
                                 .ToDictionary(x => x.Id);
                if (result.Count > 0)
                {
                    return result;
                }
            }

            return (await taskManagerClient.GetCardUsersAsync(cardId)).ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private async Task<CardAction[]> BuildCardActions(string cardId)
        {
            var actions = taskCacher.GetBuilded<CardAction>();
            return actions.Length > 0 
                    ? actions.Where(x => string.Equals(x.CardId, cardId, StringComparison.OrdinalIgnoreCase)).ToArray()
                    : await taskManagerClient.GetCardActionsAsync(cardId);
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private async Task<Dictionary<string, CardChecklist>> BuildCardChecklists(string cardId)
        {
            var checklists = taskCacher.GetBuilded<CardChecklist>();
            return (checklists.Length > 0 
                   ? checklists.Where(x => string.Equals(x.CardId, cardId, StringComparison.OrdinalIgnoreCase))
                   : await taskManagerClient.GetCardChecklistsAsync(cardId)).ToDictionary(x => x.Id);
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private async Task<Dictionary<string, BoardList[]>> BuildBoardLists(BoardCard card)
        {
            var lists = taskCacher.GetBuilded<BoardList>();
            return (lists.Length > 0 
                            ? lists.Where(x => string.Equals(x.BoardId, card.BoardId, StringComparison.OrdinalIgnoreCase))
                            : await taskManagerClient.GetBoardListsAsync(card.BoardId))
                        .GroupBy(x => x.BoardId)
                        .ToDictionary(x => x.Key, x => x.ToArray(), StringComparer.OrdinalIgnoreCase);
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
        private CardBeforeDevelopPartBlock BuildBeforeDevelopingBlock(CardStateInfo stateInfo, Dictionary<string, CardChecklist> checklists)
        {
            var result = CreateBasePart<CardBeforeDevelopPartBlock>(stateInfo, CardState.BeforeDevelop, checklists);
            if (result != null && stateInfo.States.Any(x => x.Key >= CardState.Develop))
            {
                result.Description = " арточка ушла на доработку в аналитику";
            }
            
            return result;
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardDevelopPartBlock BuildDevelopingBlock(CardStateInfo stateInfo, Dictionary<string, CardChecklist> checklists)
        {
            var result = CreateBasePart<CardDevelopPartBlock>(stateInfo, CardState.Develop, checklists);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardPresentationPartBlock BuildPresentationBlock(CardStateInfo stateInfo, Dictionary<string, CardChecklist> checklists)
        {
            var result = CreateBasePart<CardPresentationPartBlock>(stateInfo, CardState.Presentation, checklists);
            if (result == null)
            {
                return null;
            }

            // TODO: Integration with what ?) Outlook ? May be comment or descrption parse ?
            result.PresentationTime = DateTime.Now;

            return result;
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardArchivePartBlock BuildArchiveBlock(CardStateInfo stateInfo, Dictionary<string, CardChecklist> checklists)
        {
            return CreateBasePart<CardArchivePartBlock>(stateInfo, CardState.Archived, checklists);
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardReleaseWaitingPartBlock BuildReleaseWaitingBlock(CardStateInfo stateInfo, Dictionary<string, CardChecklist> checklists)
        {
            var result = CreateBasePart<CardReleaseWaitingPartBlock>(stateInfo, CardState.ReleaseWaiting, checklists);
            if (result == null)
            {
                return null;
            }

            // TODO: Integration with control version system (git)
            result.InCandidateRelease = false;
            return result;
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardReviewPartBlock BuildReviewBlock(CardStateInfo stateInfo, Dictionary<string, CardChecklist> checklists)
        {
            var result = CreateBasePart<CardReviewPartBlock>(stateInfo, CardState.Review, checklists);
            if (result == null)
            {
                return null;
            }

            result.ReviewToDoListsViewModel = BuildToDoListsViewModels(stateInfo, checklists, CardState.Review);
            return result;
        }

        private static ToDoItemsListViewModel[] BuildToDoListsViewModels(CardStateInfo stateInfo, Dictionary<string, CardChecklist> checklists, CardState state)
        {
            return stateInfo.States[state].CheckListIds.Distinct().Where(checklists.ContainsKey).Select(x => checklists[x]).Select(CreateToDoItemsListViewModel).ToArray();
        }

        private static ToDoItemsListViewModel CreateToDoItemsListViewModel(CardChecklist checkList)
        {
            var lastIncomplete = checkList.Items.FirstOrDefault(i => !i.IsChecked);
            return new ToDoItemsListViewModel
                       {
                           Count = checkList.Items.Length,
                           CompleteCount = checkList.Items.Count(i => i.IsChecked),
                           ListName = checkList.Name,
                           LastIncompletedDescription = lastIncomplete != null ? lastIncomplete.Description : null
                       };
        }

        [BlockModel(ContextKeys.TaskDetalizationKey)]
        private CardTestingPartBlock BuildTestingBlock(CardStateInfo stateInfo, Dictionary<string, CardChecklist> checklists)
        {
            if (!stateInfo.States.ContainsKey(CardState.Testing))
            {
                return null;
            }

            var result = CreateBasePart<CardTestingPartBlock>(stateInfo, CardState.Testing, checklists);

            var bugsInfo = bugsBuilder.Build(checklists.Values);
            result.Bugs = bugsInfo;
            
            return result;
        }

        private T CreateBasePart<T>(CardStateInfo state, CardState baseState, Dictionary<string, CardChecklist> checklists) where T : BasePartTaskDetalizationBlock
        {
            if (!state.States.ContainsKey(baseState))
            {
                return null;
            }

            var stateInfo = state.States[baseState];
            var result = Activator.CreateInstance<T>();

            result.IsExists = true;
            result.IsCurrent = stateInfo.State == state.CurrentState;
            result.BlockDisclamer = stateInfo.State.GetEnumDescription();
            result.PartBeginDate = stateInfo.BeginDate;
            result.PartEndDate = stateInfo.EndDate;

            var partDueDays = ((stateInfo.EndDate ?? DateTime.Now).Date - stateInfo.BeginDate.Date).Days;
            var cardChecklists = stateInfo.CheckListIds.Distinct().Where(checklists.ContainsKey).Select(x => checklists[x]);
            result.CartParrots = checklistParrotsBuilder.Build(cardChecklists, partDueDays, result.PartBeginDate, result.PartEndDate);

            return result;
        }
    }
}