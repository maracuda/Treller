using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.TaskManagerClient.Abstractions;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects.Actions;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects.Boards;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects.Cards;
using Action = SKBKontur.TaskManagerClient.Trello.BusinessObjects.Actions.Action;
using Board = SKBKontur.TaskManagerClient.BusinessObjects.Board;
using BoardList = SKBKontur.TaskManagerClient.BusinessObjects.BoardList;
using CardLabel = SKBKontur.TaskManagerClient.BusinessObjects.CardLabel;

namespace SKBKontur.TaskManagerClient.Trello
{
    public class TrelloClient : ITaskManagerClient
    {
        private readonly IHttpRequester httpClient;

        private const string UserKey = "4349fda675a2a387d7da63a457acdf19";
        private const string TokenKey = "29e9b978138d709e5ab3f5476e70c48a84f30724cc352a5d3d083e04a6ae82da";

        private static readonly Dictionary<string, string> TrelloParameters = new Dictionary<string, string>
                                                                         {
                                                                             {"key", UserKey},
                                                                             {"token", TokenKey}
                                                                         };

        public TrelloClient(IHttpRequester httpClient)
        {
            this.httpClient = httpClient;
        }

        public IEnumerable<Board> GetBoards(string[] boardIds)
        {
            return boardIds.Select(id => GetTrelloData<BusinessObjects.Boards.Board>(id, "boards/{0}"))
                           .Select(x => new Board{ Id = x.Id, OrganizationId = x.IdOrganization, Name = x.Name, Url = x.Url });
        }

        public IEnumerable<BoardList> GetBoardLists(string[] boardIds)
        {
            return boardIds.SelectMany(id => GetTrelloData<BusinessObjects.Boards.BoardList[]>(id, "boards/{0}/lists"))
                           .Select(x => new BoardList { Id = x.Id, BoardId = x.IdBoard, Name = x.Name, Position = x.Pos });

        }

        public IEnumerable<BoardCard> GetBoardCards(string[] boardIds)
        {
            return boardIds.SelectMany(id => GetTrelloData<Card[]>(id, "boards/{0}/cards")).Select(CreateBoardCard);
        }

        public IEnumerable<User> GetBoardUsers(string[] boardIds)
        {
            return boardIds.SelectMany(id => GetTrelloData<BoardMember[]>(id, "boards/{0}/members"))
                           .GroupBy(x => x.Id, StringComparer.OrdinalIgnoreCase)
                           .Select(x => x.First())
                           .Select(CreateUser);
        }

        public IEnumerable<CardChecklist> GetBoardChecklists(string[] boardIds)
        {
            return boardIds.SelectMany(id => GetTrelloData<Checklist[]>(id, "boards/{0}/checklists").Select(CreateChecklist));
        }

        public IEnumerable<CardAction> GetCardActions(string cardId)
        {
            var queryString = new Dictionary<string, string> {{"filter", "all"}, {"limit", "1000"}};
            return GetTrelloData<Action[]>(cardId, "cards/{0}/actions", queryString)
                        .Select(CreateCardAction)
                        .Where(x => x != null);
        }

        public IEnumerable<CardAction> GetActionsForBoardCards(string[] boardIds)
        {
            var queryString = new Dictionary<string, string> {{"limit", "1000"}};
            return boardIds.SelectMany(id => GetTrelloData<Action[]>(id, "boards/{0}/actions", queryString))
                           .Select(CreateCardAction)
                           .Where(x => x != null);
        }

        public BoardCard GetCard(string cardId)
        {
            return CreateBoardCard(GetTrelloData<Card>(cardId, "cards/{0}"));
        }

        public IEnumerable<User> GetCardUsers(string cardId)
        {
            return GetTrelloData<BoardMember[]>(cardId, "cards/{0}/members").Select(CreateUser);
        }

        public IEnumerable<CardChecklist> GetCardChecklists(string cardId)
        {
            return GetTrelloData<Checklist[]>(cardId, "cards/{0}/checklists").Select(CreateChecklist);
        }

        private T GetTrelloData<T>(string id, string format, Dictionary<string, string> queryString = null)
        {
            var parameters = TrelloParameters;
            if (queryString != null)
            {
                parameters = parameters.Union(queryString).ToDictionary(x => x.Key, x => x.Value);
            }

            return httpClient.SendGetAsync<T>(string.Format(string.Format("https://trello.com/1/{0}", format), id), parameters).Result;
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
            return action.Data.Card == null || action.Data.Board == null || action.Type >= ActionType.CreateList ? null
                       : new CardAction
                             {
                                 Id = action.Id,
                                 Date = action.Date,
                                 Initiator = CreateUser(action.MemberCreator),
                                 Type = action.Type,
                                 BoardId = action.Data.Board.Id,
                                 CardId = action.Data.Card.Id,
                                 Comment = action.Data.Text,
                                 AddedUser = action.Data.Member != null ? CreateUser(action.Data.Member) : null,
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