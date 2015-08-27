using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKBKontur.Infrastructure.CommonExtenssions;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.Builders;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Services.Settings;
using SKBKontur.Treller.WebApplication.Services.TaskCacher;
using SKBKontur.Treller.WebApplication.Storages;

namespace SKBKontur.Treller.WebApplication.Services.News
{
    public class NewsService : INewsService
    {
        private readonly ICachedFileStorage cachedFileStorage;
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ITaskCacher taskCacher;
        private readonly ISettingService settingService;
        private readonly ICardStageInfoBuilder cardStageInfoBuilder;
        private const string TechNewsStoreName = "NewsTechCards";
        private const string NewsStoreName = "NewsCards";

        public NewsService(
            ICachedFileStorage cachedFileStorage,
            ITaskManagerClient taskManagerClient,
            ITaskCacher taskCacher,
            ISettingService settingService,
            ICardStageInfoBuilder cardStageInfoBuilder)
        {
            this.cachedFileStorage = cachedFileStorage;
            this.taskManagerClient = taskManagerClient;
            this.taskCacher = taskCacher;
            this.settingService = settingService;
            this.cardStageInfoBuilder = cardStageInfoBuilder;
        }

        public NewsViewModel GetAllNews()
        {
            var currentTechNews = cachedFileStorage.Find<Dictionary<DateTime, NewsModel>>(TechNewsStoreName) ?? new Dictionary<DateTime, NewsModel>();
            var currentNews = cachedFileStorage.Find<Dictionary<DateTime, NewsModel>>(NewsStoreName) ?? new Dictionary<DateTime, NewsModel>();

            return new NewsViewModel
            {
                News = currentNews,
                TechnicalNews = currentTechNews
            };
        }

        public bool TryRefresh(DateTime timestamp)
        {
            var boardIds = settingService.GetDevelopingBoards().Select(x => x.Id).ToArray();
            var actions = taskManagerClient.GetActionsForBoardCardsAsync(boardIds, timestamp).Result.ToArray();

            if (actions.Length <= 0)
            {
                return true;
            }

            var cardActions = actions.Where(action =>
                                            action.Type == ActionType.CreateCard
                                            || action.Type == ActionType.MoveCardToBoard
                                            || action.Type == ActionType.MoveCardFromBoard
                                            || action.Type == ActionType.UpdateCard
                                            || action.Type == ActionType.CopyCard
                                            || !string.IsNullOrEmpty(action.ToListId)
                                            || !string.IsNullOrEmpty(action.FromListId))
                                     .ToArray();

            var currentTechNews = cachedFileStorage.Find<Dictionary<DateTime, NewsModel>>(TechNewsStoreName) ?? new Dictionary<DateTime, NewsModel>();
            var currentNews = cachedFileStorage.Find<Dictionary<DateTime, NewsModel>>(NewsStoreName) ?? new Dictionary<DateTime, NewsModel>();

            var publishedTechNewsCardIds = new HashSet<string>(currentTechNews.Where(x => x.Value.IsPublished).SelectMany(x => x.Value.CardIds), StringComparer.OrdinalIgnoreCase);
            var publishedNewsCardIds = new HashSet<string>(currentNews.Where(x => x.Value.IsPublished).SelectMany(x => x.Value.CardIds), StringComparer.OrdinalIgnoreCase);

            var cardsByDate = BuildCards(cardActions)
                                .GroupBy(x => x.DueDate ?? DateTime.Now.Date)
                                .Where(x => x.Key >= DateTime.Now.Date)
                                .ToDictionary(x => x.Key, x => x.ToArray());

            var technicalNews = cardsByDate.Select(card => BuildNewsModel(card.Key, card.Value, true, publishedTechNewsCardIds)).Where(x => x != null).ToArray();
            var news = cardsByDate.Select(card => BuildNewsModel(card.Key, card.Value, false, publishedNewsCardIds)).Where(x => x != null).ToArray();

            foreach (var oneTechNews in technicalNews)
            {
                currentTechNews[oneTechNews.ReleaseDate] = oneTechNews;
            }

            foreach (var oneNews in news)
            {
                currentNews[oneNews.ReleaseDate] = oneNews;
            }

            cachedFileStorage.Write(TechNewsStoreName, currentTechNews);
            cachedFileStorage.Write(NewsStoreName, currentNews);

            return true;
        }

        private CardNewsModel[] BuildCards(CardAction[] actions)
        {
            var actionBoardIds = actions.Select(x => x.BoardId).Distinct().ToArray();
            var actionCardIds = new HashSet<string>(actions.Select(x => x.CardId), StringComparer.OrdinalIgnoreCase);

            var cards = taskCacher.GetCached(actionBoardIds, ids => taskManagerClient.GetBoardCardsAsync(ids).Result, TaskCacherStoredTypes.BoardCards)
                .Where(x => actionCardIds.Contains(x.Id))
                .ToArray();

            var boardLists = taskCacher.GetCached(actionBoardIds, ids => taskManagerClient.GetBoardListsAsync(ids).Result, TaskCacherStoredTypes.BoardLists)
                .ToLookup(x => x.BoardId);

            var boardSettings = settingService.GetDevelopingBoards().Where(x => actionBoardIds.Contains(x.Id)).ToDictionary(x => x.Id);

            var cardActions = taskCacher.GetCached(actionBoardIds, ids => taskManagerClient.GetActionsForBoardCardsAsync(ids).Result, TaskCacherStoredTypes.BoardActions)
                .ToLookup(x => x.CardId);

            var checklists = taskCacher.GetCached(actionBoardIds, ids => taskManagerClient.GetBoardChecklistsAsync(ids).Result, TaskCacherStoredTypes.BoardChecklists).ToLookup(x => x.CardId);

            return cards.Where(x => !x.Name.Contains("Автотесты", StringComparison.OrdinalIgnoreCase))
                .Select(card => BuildCard(boardLists, boardSettings, cardActions, card, checklists))
                .Where(x =>
                    (x.State == CardState.ReleaseWaiting && x.DueDate.HasValue)
                    || x.State == CardState.Archived)
                .ToArray();
        }

        private CardNewsModel BuildCard(ILookup<string, BoardList> boardLists, Dictionary<string, BoardSettings> boardSettings, ILookup<string, CardAction> cardActions, BoardCard card, ILookup<string, CardChecklist> cardChecklists)
        {
            var stageInfo = cardStageInfoBuilder.Build(card, cardActions[card.Id].ToArray(),
                cardChecklists[card.Id].ToArray(),
                boardSettings[card.BoardId],
                boardLists[card.BoardId].ToArray());

            return new CardNewsModel
            {
                CardId = card.Id,
                CardName = card.Name,
                Labels = card.Labels.OrderBy(x => x.Color).ToArray(),
                CardDescription = card.Description,
                State = stageInfo.State,
                DueDate = card.DueDate
            };
        }

        private static NewsModel BuildNewsModel(DateTime releaseDate, CardNewsModel[] cards, bool isTechnicalNews, HashSet<string> publishedCardIds)
        {
            var marker = isTechnicalNews ? "**Технические новости**" : "**Новости**";
            var news = new StringBuilder();

            foreach (var card in cards)
            {
                if (publishedCardIds.Contains(card.CardId) || string.IsNullOrWhiteSpace(card.CardDescription))
                {
                    continue;
                }

                var newsIndex = card.CardDescription.IndexOf(marker, 0, StringComparison.OrdinalIgnoreCase);
                if (newsIndex < 0)
                {
                    continue;
                }
                newsIndex += marker.Length + 1;

                var postNewsIndex = card.CardDescription.IndexOf("**", newsIndex, StringComparison.OrdinalIgnoreCase);
                var newsLength = (postNewsIndex < 0 ? card.CardDescription.Length - 1 : postNewsIndex) - newsIndex;

                if (newsLength == 0)
                {
                    continue;
                }

                var cardNews = card.CardDescription.Substring(newsIndex, newsLength);

                if (string.IsNullOrEmpty(cardNews))
                {
                    continue;
                }

                cardNews = cardNews.Trim();

                if (news.Length > 0)
                {
                    news.Insert(0, "Ожидается релиз нескольких фич:" + Environment.NewLine);
                }

                news.Append(cardNews);
                news.Append(Environment.NewLine);
            }

            if (news.Length == 0)
            {
                return null;
            }

            return new NewsModel
            {
                ReleaseDate = releaseDate,
                CardIds = cards.Select(x => x.CardId).ToArray(),
                IsTechnicalNews = isTechnicalNews,
                NewsHeader = isTechnicalNews ? "Технические новости" : "Релизы",
                NewsText = news.ToString()
            };
        }
    }
}