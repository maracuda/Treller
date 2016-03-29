﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKBKontur.BlocksMapping.BlockExtenssions;
using SKBKontur.Infrastructure.CommonExtenssions;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Blocks.Builders;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Extensions;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;
using SKBKontur.Treller.WebApplication.Services.Settings;
using SKBKontur.Treller.WebApplication.Services.TaskCacher;

namespace SKBKontur.Treller.WebApplication.Services.Digest
{
    public class DigestService : IDigestService
    {
        private const string DigestFileName = "DigestCache";

        #region init

        private readonly ISocialNetworkClient socialNetworkClient;
        private readonly ISettingService settingService;
        private readonly ITaskCacher taskCacher;
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ICardStateInfoBuilder cardStateInfoBuilder;
        private readonly ICachedFileStorage cachedFileStorage;
        private readonly IWikiClient wikiClient;
        private readonly IBugTrackerClient bugTrackerClient;

        public DigestService(
            ISocialNetworkClient socialNetworkClient,
            ISettingService settingService,
            ITaskCacher taskCacher,
            ITaskManagerClient taskManagerClient,
            ICardStateInfoBuilder cardStateInfoBuilder,
            ICachedFileStorage cachedFileStorage,
            IWikiClient wikiClient,
            IBugTrackerClient bugTrackerClient)
        {
            this.socialNetworkClient = socialNetworkClient;
            this.settingService = settingService;
            this.taskCacher = taskCacher;
            this.taskManagerClient = taskManagerClient;
            this.cardStateInfoBuilder = cardStateInfoBuilder;
            this.cachedFileStorage = cachedFileStorage;
            this.wikiClient = wikiClient;
            this.bugTrackerClient = bugTrackerClient;
        }
        #endregion

        public void SendAllToDigest()
        {
            var boardSettings = settingService.GetDevelopingBoards().ToDictionary(x => x.Id);
            var boardIds = boardSettings.Select(x => x.Key).ToArray();
            var cards = taskCacher.GetCached(boardIds, strings => taskManagerClient.GetBoardCardsAsync(strings).Result, TaskCacherStoredTypes.BoardCards);
            var boardLists = taskCacher.GetCached(boardIds, ids => taskManagerClient.GetBoardListsAsync(ids).Result, TaskCacherStoredTypes.BoardLists).ToLookup(x => x.BoardId);
            var cardActions = taskCacher.GetCached(boardIds, strings => taskManagerClient.GetActionsForBoardCardsAsync(strings).Result, TaskCacherStoredTypes.BoardActions).ToLookup(x => x.CardId);
            var users = taskCacher.GetCached(boardIds, strings => taskManagerClient.GetBoardUsersAsync(strings).Result, TaskCacherStoredTypes.BoardUsers).ToDictionary(x => x.Id);

            var actualCards = cards
                .Where(x => !x.Name.Contains("Автотесты", StringComparison.OrdinalIgnoreCase) && x.LastActivity.Date > DateTime.Now.Date.AddDays(-3))
                .Select(card =>
                {
                    var cardStateInfo = cardStateInfoBuilder.Build(cardActions[card.Id].ToArray(), boardSettings, boardLists.ToDictionary(x => x.Key, x => x.ToArray()));
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
                        DevelopPeriod = string.Format("{0:dd.MM.yyyy} - {1:dd.MM.yyyy}", analyticDate != null ? analyticDate.BeginDate : DateTime.Today, DateTime.Today),
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