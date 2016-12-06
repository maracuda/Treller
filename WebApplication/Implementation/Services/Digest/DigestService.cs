using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKBKontur.BlocksMapping.BlockExtenssions;
using SKBKontur.Infrastructure.CommonExtenssions;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Extensions;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;
using SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService;
using SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Digest
{
    public class DigestService : IDigestService
    {
        private const string DigestFileName = "DigestCache";

        #region init

        private readonly ISocialNetworkClient socialNetworkClient;
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ICardStateInfoBuilder cardStateInfoBuilder;
        private readonly ICachedFileStorage cachedFileStorage;
        private readonly IWikiClient wikiClient;
        private readonly IBugTrackerClient bugTrackerClient;
        private readonly IBoardsService boardsService;

        public DigestService(
            ISocialNetworkClient socialNetworkClient,
            ITaskManagerClient taskManagerClient,
            ICardStateInfoBuilder cardStateInfoBuilder,
            ICachedFileStorage cachedFileStorage,
            IWikiClient wikiClient,
            IBugTrackerClient bugTrackerClient,
            IBoardsService boardsService)
        {
            this.socialNetworkClient = socialNetworkClient;
            this.taskManagerClient = taskManagerClient;
            this.cardStateInfoBuilder = cardStateInfoBuilder;
            this.cachedFileStorage = cachedFileStorage;
            this.wikiClient = wikiClient;
            this.bugTrackerClient = bugTrackerClient;
            this.boardsService = boardsService;
        }
        #endregion

        public void SendAllToDigest()
        {
            var boardIds = boardsService.SelectKanbanBoards(false).Select(x => x.Id).ToArray();
            var cards = taskManagerClient.GetBoardCardsAsync(boardIds).Result;
            var boardLists = taskManagerClient.GetBoardListsAsync(boardIds).Result.ToLookup(x => x.BoardId);
            var cardActions = taskManagerClient.GetActionsForBoardCardsAsync(boardIds).Result.ToLookup(x => x.CardId);
            var users = taskManagerClient.GetBoardUsersAsync(boardIds).Result.ToDictionary(x => x.Id);

            var actualCards = cards
                .Where(x => !x.Name.Contains("Автотесты", StringComparison.OrdinalIgnoreCase) && x.LastActivity.Date > DateTime.Now.Date.AddDays(-3))
                .Select(card =>
                {
                    var cardStateInfo = cardStateInfoBuilder.Build(cardActions[card.Id].ToArray(), boardLists.ToDictionary(x => x.Key, x => x.ToArray()));
                    var analyticDate = cardStateInfo.States.OrderBy(x => x.Key).Select(x => x.Value).LastOrDefault(x => x.State < CardState.Develop);
                    var cardUsers = card.UserIds.Select(x => users.SafeGet(x)).Where(x => x != null).Select(x => x.FullName).ToArray();

                    return new
                    {
                        CardId = card.Id,
                        CardName = card.Name,
                        CardDescription = card.Description,
                        State = cardStateInfo.CurrentState,
                        AnalyticLink = card.GetAnalyticLink(wikiClient.GetBaseUrl(), bugTrackerClient.GetBaseUrl()),
                        BranchName = card.GetCardBranchName(),
                        DevelopPeriod = $"{analyticDate?.BeginDate ?? DateTime.Today:dd.MM.yyyy} - {DateTime.Today:dd.MM.yyyy}",
                        Users = cardUsers,
                        card.Url
                    };
                })
                .Where(x => (x.State >= CardState.Released))
                .ToArray();

            var digestedCards = cachedFileStorage.Find<List<string>>(DigestFileName) ?? new List<string>();
            var inDigest = new HashSet<string>(digestedCards);

            foreach (var card in actualCards.Where(card => !inDigest.Contains(card.CardId)))
            {
                var message = new StringBuilder();
                message.AppendFormat("Карточка: <b>[{0}]({1})</b>", card.CardName, card.Url);
                message.AppendLine();
                message.AppendFormat("Период разработки: <b>{0}</b>", card.DevelopPeriod);
                message.AppendLine();
                message.AppendLine();

                if (!string.IsNullOrWhiteSpace(card.AnalyticLink))
                {
                    message.AppendFormat("Аналитика: {0}", card.AnalyticLink);
                    message.AppendLine();
                }

                if (!string.IsNullOrWhiteSpace(card.BranchName))
                {
                    message.AppendFormat("Ветка: {0}", card.BranchName);
                    message.AppendLine();
                }

                if (card.Users.Length > 0)
                {
                    message.AppendLine();
                    message.AppendLine("Команда:");
                    foreach (var user in card.Users)
                    {
                        message.AppendLine(user);
                    }
                }

                socialNetworkClient.Feed(message.ToString());
                digestedCards.Add(card.CardId);
            }

            cachedFileStorage.Write(DigestFileName, digestedCards);
        }
    }
}