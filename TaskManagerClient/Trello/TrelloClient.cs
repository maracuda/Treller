using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SKBKontur.HttpInfrastructure.Clients;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects.Actions;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects.Boards;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects.Cards;
using Action = SKBKontur.TaskManagerClient.Trello.BusinessObjects.Actions.Action;
using Board = SKBKontur.TaskManagerClient.BusinessObjects.TaskManager.Board;
using BoardList = SKBKontur.TaskManagerClient.BusinessObjects.TaskManager.BoardList;
using CardLabel = SKBKontur.TaskManagerClient.BusinessObjects.TaskManager.CardLabel;
using SKBKontur.TaskManagerClient.Extensions;

namespace SKBKontur.TaskManagerClient.Trello
{
    public class TrelloClient : ITaskManagerClient
    {
        private readonly IHttpClient httpClient;
        private readonly Dictionary<string, string> trelloParameters;
        private const int MaxActionsLimitCount = 1000;

        public TrelloClient(IHttpClient httpClient, ITrelloUserCredentialService trelloUserCredentialService)
        {
            this.httpClient = httpClient;
            var trelloCredential = trelloUserCredentialService.GetCredentials();
            trelloParameters = new Dictionary<string, string>
                                   {
                                       {"key", trelloCredential.UserKey},
                                       {"token", trelloCredential.UserToken}
                                   };
        }

        public Task<Board[]> GetOpenBoardsAsync(string organizationIdOrName)
        {
            return GetTrelloDataAsync<BusinessObjects.Boards.Board[]>(organizationIdOrName, "organizations/{0}/boards", new Dictionary<string, string>{{"filter", "open"}})
                    .Await(x => x.Select(b => new Board { Id = b.Id, Name = b.Name, Url = b.Url, OrganizationId = b.IdOrganization }).ToArray());
        }

        public Board[] GetOpenBoards(string organizationIdOrName)
        {
            return GetTrelloData<BusinessObjects.Boards.Board[]>(organizationIdOrName, "organizations/{0}/boards", new Dictionary<string, string> {{"filter", "open"}})
                    .Select(board => new Board
                    {
                        Id = board.Id,
                        Name = board.Name,
                        Url = board.Url,
                        OrganizationId = board.IdOrganization
                    })
                    .ToArray();
        }

        public Board[] GetAllBoards(string organizationIdOrName)
        {
            return GetTrelloData<BusinessObjects.Boards.Board[]>(organizationIdOrName, "organizations/{0}/boards",
                new Dictionary<string, string> {{"filter", "all"}})
                .Select(b => new Board {Id = b.Id, Name = b.Name, Url = b.Url, OrganizationId = b.IdOrganization})
                .ToArray();
        }

        public Task<Board[]> GetBoardsAsync(string[] boardIds)
        {
            return boardIds.Select(id => GetTrelloDataAsync<BusinessObjects.Boards.Board>(id, "boards/{0}"))
                           .Await(x => new Board { Id = x.Id, OrganizationId = x.IdOrganization, Name = x.Name, Url = x.Url });
        }

        public Task<BoardList[]> GetBoardListsAsync(params string[] boardIds)
        {
            return boardIds.Select(id => GetTrelloDataAsync<BusinessObjects.Boards.BoardList[]>(id, "boards/{0}/lists"))
                           .Await(x => new BoardList { Id = x.Id, BoardId = x.IdBoard, Name = x.Name, Position = x.Pos });

        }

        public BoardList[] GetBoardLists(params string[] boardIds)
        {
            return boardIds
                .SelectMany(boardId => GetTrelloData<BusinessObjects.Boards.BoardList[]>(boardId, "boards/{0}/lists"))
                .Select(x => new BoardList {Id = x.Id, BoardId = x.IdBoard, Name = x.Name, Position = x.Pos})
                .ToArray();
        }

        public Task<BoardCard[]> GetBoardCardsAsync(string[] boardIds)
        {
            return boardIds.Select(id => GetTrelloDataAsync<Card[]>(id, "boards/{0}/cards"))
                           .Await(CreateBoardCard);
        }

        public Task<User[]> GetBoardUsersAsync(string[] boardIds)
        {
            var queryString = new Dictionary<string, string> { { "fields", "all" } };
            return boardIds.Select(id => GetTrelloDataAsync<BoardMember[]>(id, "boards/{0}/members", queryString))
                           .Await(CreateUser, result => result.GroupBy(x => x.Id, StringComparer.OrdinalIgnoreCase).Select(x => x.First()));
        }

        public Task<CardChecklist[]> GetBoardChecklistsAsync(string[] boardIds)
        {
            return boardIds.Select(id => GetTrelloDataAsync<Checklist[]>(id, "boards/{0}/checklists"))
                           .Await(CreateChecklist);
        }

        public CardAction[] GetActionsForBoardCards(string[] boardIds, DateTime fromUtc, DateTime toUtc)
        {
            var result = new LinkedList<CardAction>();
            var queryString = new Dictionary<string, string> { { "filter", "all" }, { "limit", MaxActionsLimitCount.ToString()} };

            foreach (var boardId in boardIds)
            {
                queryString["since"] = fromUtc.ToString("O");
                
                while (true)
                {
                    var boardResult = GetTrelloData<Action[]>(boardId, "boards/{0}/actions", queryString);
                    foreach (var action in boardResult.Where(x => x.Date < toUtc))
                    {
                        result.AddLast(CreateCardAction(action));
                    }

                    if (boardResult.Length < MaxActionsLimitCount || boardResult.Any(x => x.Date > toUtc))
                    {
                        break;
                    }

                    queryString["since"] = boardResult.Max(b => b.Date).AddMilliseconds(1).ToString("O");
                }
            }

            return result.Where(x => x != null).ToArray();
        }

        public Task<CardAction[]> GetCardActionsAsync(string cardId)
        {
            var queryString = new Dictionary<string, string> {{"filter", "all"}, {"limit", "1000"}};

            return GetTrelloDataAsync<Action[]>(cardId, "cards/{0}/actions", queryString)
                    .Await(CreateCardAction, result => result.Where(x => x != null));
        }

        public Task<CardAction[]> GetActionsForBoardCardsAsync(string[] boardIds, DateTime? fromUtc, int limit)
        {
            var queryString = new Dictionary<string, string> { { "filter", "all" }, { "limit", limit.ToString() } };
            if (fromUtc.HasValue)
            {
                queryString.Add("since", fromUtc.Value.ToString("O"));
            }

            return boardIds.Select(id => GetTrelloDataAsync<Action[]>(id, "boards/{0}/actions", queryString))
                           .Await(CreateCardAction, result => result.Where(x => x != null));
        }

        public Task<BoardCard> GetCardAsync(string cardId)
        {
            return GetTrelloDataAsync<Card>(cardId, "cards/{0}").Await(CreateBoardCard);
        }

        public Task<User[]> GetCardUsersAsync(string cardId)
        {
            return GetTrelloDataAsync<BoardMember[]>(cardId, "cards/{0}/members").Await(CreateUser);
        }

        public Task<CardChecklist[]> GetCardChecklistsAsync(string cardId)
        {
            return GetTrelloDataAsync<Checklist[]>(cardId, "cards/{0}/checklists").Await(CreateChecklist);
        }

        private Task<T> GetTrelloDataAsync<T>(string id, string format, Dictionary<string, string> queryString = null)
        {
            var parameters = trelloParameters;
            if (queryString != null)
            {
                parameters = parameters.Union(queryString).ToDictionary(x => x.Key, x => x.Value);
            }

            return httpClient.SendGetAsync<T>(string.Format($"https://trello.com/1/{format}", id), parameters);
        }

        private T GetTrelloData<T>(string id, string format, Dictionary<string, string> queryString = null)
        {
            var parameters = trelloParameters;
            if (queryString != null)
            {
                parameters = parameters.Union(queryString).ToDictionary(x => x.Key, x => x.Value);
            }

            return httpClient.SendGet<T>(string.Format($"https://trello.com/1/{format}", id), parameters);
        }

        private static CardChecklist CreateChecklist(Checklist list)
        {
            return new CardChecklist
                       {
                           Id = list.Id,
                           Name = list.Name,
                           CardId = list.IdCard,
                           Position = list.Pos,
                           Items = list.CheckItems.Select(i => new ChecklistItem
                                                                   {
                                                                       Id = i.Id,
                                                                       Description = i.Name,
                                                                       Position = i.Pos,
                                                                       IsChecked = i.IsChecked,
                                                                   }).ToArray()
                       };
        }

        private static BoardCard CreateBoardCard(Card card)
        {
            CardLabelColor result;
            return new BoardCard
                       {
                           Id = card.Id,
                           Url = card.Url,
                           DueDate = card.Due,
                           BoardId = card.IdBoard,
                           Name = card.Name,
                           Position = card.Pos,
                           BoardListId = card.IdList,
                           Description = card.Desc,
                           Labels = card.Labels.Select(cardLabel => 
                                                       new CardLabel
                                                           {
                                                               Name = cardLabel.Name,
                                                               Color = Enum.TryParse(cardLabel.Color, true, out result) ? result : CardLabelColor.Undefined
                                                           }).ToArray(),
                           LastActivity = card.DateLastActivity,
                           UserIds = card.IdMembers.ToArray(),
                           CheckListIds = card.IdCheckLists
                       };
        }

        private static CardAction CreateCardAction(Action action)
        {
            return (action.Data.Card == null || action.ActionType == ActionType.Unknown || action.Data.Board == null || action.ActionType > ActionType.RemoveMemberFromBoard)
                    && action.ActionType != ActionType.AddMemberToBoard && action.ActionType != ActionType.RemoveMemberFromBoard ? null
                       : new CardAction
                             {
                                 Id = action.Id,
                                 Date = action.Date,
                                 Initiator = CreateUser(action.MemberCreator),
                                 Type = action.ActionType,
                                 BoardId = action.Data.Board == null ? string.Empty : action.Data.Board.Id,
                                 CardId = action.Data.Card == null ? string.Empty : action.Data.Card.Id,
                                 Comment = action.Data.Text,
                                 AddedUser = action.Member != null ? CreateUser(action.Member) : null,
                                 ListId = action.Data.List != null ? action.Data.List.Id : null,
                                 FromListId = action.Data.ListBefore != null ? action.Data.ListBefore.Id : null,
                                 ToListId = action.Data.ListAfter != null ? action.Data.ListAfter.Id : null,
                                 CreatedCheckListId = action.Data.Checklist != null ? action.Data.Checklist.Id : null
                             };
        }

        private static User CreateUser(ActionMember actionMember)
        {
            var boardMember = actionMember as BoardMember;

            return new User
                       {
                           Id = actionMember.Id,
                           AvatarHash = actionMember.AvatarHash,
                           FullName = actionMember.FullName,
                           Name = actionMember.Username,
                           Initials = actionMember.Initials,
                           UserUrl = boardMember != null ? boardMember.Url : null
                       };
        }
    }
}