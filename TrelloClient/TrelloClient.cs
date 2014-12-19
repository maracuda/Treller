using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.HttpInfrastructure.Clients;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects;
using TrelloNet;
using Action = TrelloNet.Action;
using Board = SKBKontur.TaskManagerClient.BusinessObjects.Board;

namespace SKBKontur.Treller.TrelloClient
{
    public class TrelloClient : ITaskManagerClient
    {
        #region initialization
        private readonly IHttpClient httpClient;

        private const string UserKey = "4349fda675a2a387d7da63a457acdf19";
        private const string TokenKey = "29e9b978138d709e5ab3f5476e70c48a84f30724cc352a5d3d083e04a6ae82da";

        private static readonly Dictionary<string, string> TrelloParameters = new Dictionary<string, string>
                                                                         {
                                                                             {"key", UserKey},
                                                                             {"token", TokenKey}
                                                                         };

        private Lazy<Trello> trello;

        public TrelloClient(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
            trello = new Lazy<Trello>(GetTrelloInstance);
        }

        private static Trello GetTrelloInstance()
        {
//            // TO det authorization token need execute:
//            // return trello.Value.GetAuthorizationUrl("Treller", Scope.ReadWrite, Expiration.Never);
//
            var result = new Trello(UserKey);
            result.Authorize(TokenKey);
            return result;
        } 
        #endregion

        public IEnumerable<Board> GetBoards(string[] boardIds)
        {
            return boardIds.Select(id => GetTrelloData<TrelloNet.Board>(id, "boards/{0}"))
                                        .Select(x => new Board
                                                        {
                                                            Id = x.Id,
                                                            OrganizationId = x.IdOrganization,
                                                            Name = x.Name,
                                                            Url = x.Url
                                                        });
        }

        public IEnumerable<BoardList> GetBoardLists(string[] boardIds)
        {
            return boardIds.SelectMany(id => GetTrelloData<List[]>(id, "boards/{0}/lists"))
                           .Select(x => new BoardList
                                            {
                                                Id = x.Id,
                                                BoardId = x.IdBoard,
                                                Name = x.Name,
                                                Position = x.Pos
                                            });

        }

        public IEnumerable<BoardCard> GetBoardCards(string[] boardIds)
        {
            return boardIds.SelectMany(id => GetTrelloData<Card[]>(id, "boards/{0}/cards")).Select(CreateBoardCard);
        }

        private static BoardCard CreateBoardCard(Card x)
        {
            var checklists = x.Checklists != null ? x.Checklists.Select(c => new CardChecklist
                                                          {
                                                              Id = c.Id,
                                                              Name = c.Name,
                                                              CardId = x.Id,
                                                              Position = c.Pos,
                                                              Items = c.CheckItems.Select(i => new ChecklistItem
                                                                                                   {
                                                                                                       Id = i.Id,
                                                                                                       Description =
                                                                                                           i.Name,
                                                                                                       Position = i.Pos,
                                                                                                       IsChecked =
                                                                                                           i.Checked
                                                                                                   }).ToArray()
                                                          }).ToArray() : new CardChecklist[0];
            return new BoardCard
                       {
                           Id = x.Id,
                           Url = x.Url,
                           DueDate = x.Due,
                           BoardId = x.IdBoard,
                           Name = x.Name,
                           Position = x.Pos,
                           BoardListId = x.IdList,
                           Description = x.Desc,
                           Labels = x.Labels.Select(l => new CardLabel { Name = l.Name, Color = Cast(l.Color) }).ToArray(),
                           LastActivity = x.DateLastActivity,
                           UserIds = x.IdMembers.ToArray(),
                           CheckLists = checklists
                       };
        }

        private static CardLabelColor Cast(Color cardColor)
        {
            CardLabelColor result;
            return Enum.TryParse(cardColor.ColorName, true, out result) 
                    ? result 
                    : CardLabelColor.Undefined;
        }

        public IEnumerable<User> GetBoardUsers(string[] boardIds)
        {
            return boardIds.SelectMany(id => GetTrelloData<Member[]>(id, "boards/{0}/members"))
                           .GroupBy(x => x.Id, StringComparer.OrdinalIgnoreCase)
                           .Select(x => x.First())
                           .Select(CreateUser);
        }

        public IEnumerable<CardAction> GetCardActions(string cardId)
        {
            //GetTrelloData<Action[]>(cardId, "cards/{0}/actions")
            return trello.Value.Actions.ForCard(new CardId(cardId)).Select(CreateCardAction).Where(x => x != null);
        }

        private static CardAction CreateCardAction(Action action)
        {
            var result = new CardAction
                       {
                           Id = action.Id,
                           Date = action.Date,
                           Initiator = CreateUser(action.MemberCreator)
                       };

            if (!action.GetType().Name.Contains("Card"))
            {
                return null;
            }

            result.CardId = ((dynamic) action).Data.Card.Id;
            result.BoardId = ((dynamic) action).Data.Board.Id;
            if (string.IsNullOrEmpty(result.CardId) || string.IsNullOrEmpty(result.BoardId))
            {
                return null;
            }

            var create = action as CreateCardAction;
            if (create != null)
            {
                result.ListId = create.Data.List.Id;
                return result;
            }

            var move = action as MoveCardToBoardAction;
            if (move != null)
            {
                result.ListId = move.Data.List.Id;
                return result;
            }
            
            var addMemberAction = action as AddMemberToCardAction;
            if (addMemberAction != null)
            {
                result.AddedUser = CreateUser(addMemberAction.Member);
                return result;
            }

            var commentAction = action as CommentCardAction;
            if (commentAction != null)
            {
                result.Comment = commentAction.Data.Text;
                return result;
            }

            var updateList = action as UpdateCardMoveAction;
            if (updateList != null)
            {
                result.ListId = updateList.Data.ListBefore.Id;
                result.ToListId = updateList.Data.ListAfter.Id;
                return result;
            }

            return result;
        }

        public IEnumerable<CardAction> GetActionsForBoardCards(string[] boardIds)
        {
            //GetTrelloData<Action[]>(id, "boards/{0}/actions")
            return boardIds.SelectMany(id => trello.Value.Actions.ForBoard(new BoardId(id))).Select(CreateCardAction).Where(x => x != null);
        }

        private static User CreateUser(Member b)
        {
            return new User
                       {
                           Id = b.Id,
                           Name = b.Username,
                           FullName = b.FullName,
                           AvatarInfo = b.AvatarHash,
                           Initials = b.Initials,
                           UserUrl = b.Url
                       };
        }

        private static User CreateUser(Action.ActionMember actionUser)
        {
            return new User
            {
                Id = actionUser.Id,
                Name = actionUser.Username,
                FullName = actionUser.FullName,
                AvatarInfo = actionUser.AvatarHash,
                Initials = actionUser.Initials
            };
        }

        public BoardCard GetCard(string cardId)
        {
            return CreateBoardCard(GetTrelloData<Card>(cardId, "cards/{0}"));
        }

        public IEnumerable<User> GetCardUsers(string cardId)
        {
            return GetTrelloData<Member[]>(cardId, "cards/{0}/members").Select(CreateUser);
        }

        private T GetTrelloData<T>(string id, string format)
        {
            return httpClient.SendGetAsync<T>(string.Format(string.Format("https://trello.com/1/{0}", format), id), TrelloParameters).Result;
        }
    }
}