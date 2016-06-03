using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SKBKontur.HttpInfrastructure.Clients;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.TaskManagerClient.Caching;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects.Boards;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects.Cards;
using Action = SKBKontur.TaskManagerClient.Trello.BusinessObjects.Actions.Action;
using Board = SKBKontur.TaskManagerClient.BusinessObjects.TaskManager.Board;
using BoardList = SKBKontur.TaskManagerClient.BusinessObjects.TaskManager.BoardList;
using SKBKontur.TaskManagerClient.Extensions;

namespace SKBKontur.TaskManagerClient.Trello
{
    public class TrelloClient : ITaskManagerClient
    {
        private readonly IHttpClient httpClient;
        private readonly IMemoryCache responseCache;
        private readonly Dictionary<string, string> credentials;
        private const int MaxActionsLimitCount = 1000;

        public TrelloClient(
            IHttpClient httpClient, 
            ITrelloUserCredentialService trelloUserCredentialService,
            ICacheFactory cacheFactory)
        {
            this.httpClient = httpClient;
            responseCache = cacheFactory.CreateMemoryCache(GetType().Name, TimeSpan.FromMinutes(30));
            var trelloCredentials = trelloUserCredentialService.GetCredentials();
            credentials = new Dictionary<string, string>
                                   {
                                       {"key", trelloCredentials.UserKey},
                                       {"token", trelloCredentials.UserToken}
                                   };
        }

        public Task<Board[]> GetOpenBoardsAsync(string organizationIdOrName)
        {
            return ReadAsync<BusinessObjects.Boards.Board[]>($"organizations/{organizationIdOrName}/boards", new Dictionary<string, string>{{"filter", "open"}})
                    .Await(x => x.Select(Board.ConvertFrom).ToArray());
        }

        public Board[] GetOpenBoards(string organizationIdOrName)
        {
            return AsyncHelpers.RunSync(() => GetOpenBoardsAsync(organizationIdOrName));
        }

        public Board[] GetAllBoards(string organizationIdOrName)
        {
            return Read<BusinessObjects.Boards.Board[]>($"organizations/{organizationIdOrName}/boards", new Dictionary<string, string> {{"filter", "all"}})
                    .Select(Board.ConvertFrom)
                    .ToArray();
        }

        public Task<Board[]> GetBoardsAsync(string[] boardIds)
        {
            return boardIds.Select(id => ReadAsync<BusinessObjects.Boards.Board>($"boards/{id}"))
                           .Await(Board.ConvertFrom);
        }

        public Task<BoardList[]> GetBoardListsAsync(params string[] boardIds)
        {
            return boardIds.Select(id => ReadAsync<BusinessObjects.Boards.BoardList[]>($"boards/{id}/lists"))
                           .Await(x => new BoardList { Id = x.Id, BoardId = x.IdBoard, Name = x.Name, Position = x.Pos });

        }

        public BoardList[] GetBoardLists(params string[] boardIds)
        {
            return boardIds
                .SelectMany(boardId => Read<BusinessObjects.Boards.BoardList[]>($"boards/{boardId}/lists"))
                .Select(x => new BoardList {Id = x.Id, BoardId = x.IdBoard, Name = x.Name, Position = x.Pos})
                .ToArray();
        }

        public Task<BoardCard[]> GetBoardCardsAsync(string[] boardIds)
        {
            return boardIds.Select(id => ReadAsync<Card[]>($"boards/{id}/cards"))
                           .Await(BoardCard.ConvertFrom);
        }

        public Task<User[]> GetBoardUsersAsync(string[] boardIds)
        {
            var queryString = new Dictionary<string, string> { { "fields", "all" } };
            return boardIds.Select(id => ReadAsync<BoardMember[]>($"boards/{id}/members", queryString))
                           .Await(User.ConvertFrom, result => result.GroupBy(x => x.Id, StringComparer.OrdinalIgnoreCase).Select(x => x.First()));
        }

        public Task<CardChecklist[]> GetBoardChecklistsAsync(string[] boardIds)
        {
            return boardIds.Select(id => ReadAsync<Checklist[]>($"boards/{id}/checklists"))
                           .Await(CardChecklist.ConvertFrom);
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
                    var boardResult = Read<Action[]>($"boards/{boardId}/actions", queryString);
                    foreach (var action in boardResult.Where(x => x.Date < toUtc))
                    {
                        result.AddLast(CardAction.ConvertFrom(action));
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
            return ReadAsync<Action[]>($"cards/{cardId}/actions", new Dictionary<string, string> {{"filter", "all"}, {"limit", "1000"}})
                    .Await(CardAction.ConvertFrom, result => result.Where(x => x != null));
        }

        public Task<CardAction[]> GetActionsForBoardCardsAsync(string[] boardIds, DateTime? fromUtc, int limit)
        {
            var queryString = new Dictionary<string, string> { { "filter", "all" }, { "limit", limit.ToString() } };
            if (fromUtc.HasValue)
            {
                queryString.Add("since", fromUtc.Value.ToString("O"));
            }

            return boardIds.Select(id => ReadAsync<Action[]>($"boards/{id}/actions", queryString))
                           .Await(CardAction.ConvertFrom, result => result.Where(x => x != null));
        }

        public Task<BoardCard> GetCardAsync(string cardId)
        {
            return ReadAsync<Card>($"cards/{cardId}").Await(BoardCard.ConvertFrom);
        }

        public Task<User[]> GetCardUsersAsync(string cardId)
        {
            return ReadAsync<BoardMember[]>($"cards/{cardId}/members").Await(User.ConvertFrom);
        }

        public Task<CardChecklist[]> GetCardChecklistsAsync(string cardId)
        {
            return ReadAsync<Checklist[]>($"cards/{cardId}/checklists").Await(CardChecklist.ConvertFrom);
        }

        private T ReadOrGetCached<T>(string path)
        {
            return responseCache.GetOrLoad(path, () => AsyncHelpers.RunSync(() => httpClient.SendGetAsync<T>($"https://trello.com/1/{path}", credentials)));
        }


        private Task<T> ReadAsync<T>(string path, Dictionary<string, string> queryString = null)
        {
            var parameters = queryString == null ? credentials : credentials.Union(queryString).ToDictionary(x => x.Key, x => x.Value);
            return httpClient.SendGetAsync<T>($"https://trello.com/1/{path}", parameters);
        }

        private T Read<T>(string path, Dictionary<string, string> queryString = null)
        {
            return AsyncHelpers.RunSync(() => ReadAsync<T>(path, queryString));
        }
    }
}