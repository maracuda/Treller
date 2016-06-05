using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SKBKontur.BlocksMapping.BlockExtenssions;
using SKBKontur.Infrastructure.CommonExtenssions;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService;
using SKBKontur.Treller.WebApplication.Implementation.Services.TaskCacher;
using SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsModelBuilder : INewsModelBuilder
    {
        private readonly INewsStorage newsStorage;
        private readonly INewsSettingsService newsSettingsService;
        private readonly IBoardsService boardsService;
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ITaskCacher taskCacher;
        private readonly ICardStateInfoBuilder cardStateInfoBuilder;

        public NewsModelBuilder(
            INewsStorage newsStorage,
            INewsSettingsService newsSettingsService,
            IBoardsService boardsService,
            ITaskManagerClient taskManagerClient,
            ITaskCacher taskCacher,
            ICardStateInfoBuilder cardStateInfoBuilder)
        {
            this.newsStorage = newsStorage;
            this.newsSettingsService = newsSettingsService;
            this.boardsService = boardsService;
            this.taskManagerClient = taskManagerClient;
            this.taskCacher = taskCacher;
            this.cardStateInfoBuilder = cardStateInfoBuilder;
        }

        public NewsViewModel BuildViewModel()
        {
            var news = newsStorage.ReadAll();
            return new NewsViewModel
            {
                NewsToPublish = BuildNewsModel(news, false),
                TechnicalNewsToPublish = BuildNewsModel(news, true),
                NotActualCards = news.Where(x => x.IsPublished() || x.IsDeleted).ToArray(),
                CardsWihoutNews = news.Where(x => !x.IsPublished() && !x.IsDeleted && !x.IsNewsExists()).ToArray(),
                ActualCards = news.Where(x => !x.IsPublished() && !x.IsDeleted && x.IsNewsExists()).ToArray(),
                Settings = newsSettingsService.GetOrRead()
            };
        }

        private static NewsModel BuildNewsModel(CardNewsModel[] cards, bool isTechnicalNews)
        {
            var news = new StringBuilder();
            DateTime releaseDate = new DateTime();
            var cardsForSend = new List<CardNewsModel>();

            cards = cards.Where(x => !x.IsPublished(isTechnicalNews) && !x.IsDeleted) // && x.PublishDate >= DateTime.Today
                         .OrderBy(x => x.CardReleaseDate)
                         .ToArray();

            foreach (var card in cards)
            {
                var newsText = isTechnicalNews ? card.TechnicalNewsText : card.NewsText;
                if (string.IsNullOrWhiteSpace(newsText))
                {
                    continue;
                }

                if (card.CardReleaseDate != releaseDate)
                {
                    news.Append(card.CardReleaseDate >= DateTime.Today
                        ? $"Вечером {card.CardReleaseDate.ToString("D", new CultureInfo("ru-RU", false))} будем релизить<br/><br/>"
                        : $"Вечером {card.CardReleaseDate.ToString("D", new CultureInfo("ru-RU", false))} состоялся релиз<br/><br/>");
                }
                releaseDate = card.CardReleaseDate;
                news.Append($"<b>{card.CardName}</b>{"<br/>"}{newsText}<br/><br/>");
                cardsForSend.Add(card);
            }

            if (news.Length == 0)
            {
                return null;
            }

            return new NewsModel
            {
                Cards = cardsForSend.ToArray(),
                NewsHeader = isTechnicalNews ? "Технические релизы Контур.Биллинг" : "Релизы Контур.Биллинг",
                NewsText = news.ToString()
            };
        }

        public CardNewsModel[] BuildNewsModel()
        {
            var boardIds = boardsService.SelectKanbanBoards(false).Select(x => x.Id).ToArray();
            var cards = taskCacher.GetCached(boardIds, strings => taskManagerClient.GetBoardCardsAsync(strings).Result, TaskCacherStoredTypes.BoardCards);
            var boardLists = taskCacher.GetCached(boardIds, ids => taskManagerClient.GetBoardListsAsync(ids).Result, TaskCacherStoredTypes.BoardLists).ToLookup(x => x.BoardId);
            var cardActions = taskCacher.GetCached(boardIds, strings => taskManagerClient.GetActionsForBoardCardsAsync(strings).Result, TaskCacherStoredTypes.BoardActions).ToLookup(x => x.CardId);

            return cards
                .Where(x => x.LastActivity.Date > DateTime.Now.Date.AddDays(-30))
                .Select(card =>
                {
                    var cardStateInfo = cardStateInfoBuilder.Build(cardActions[card.Id].ToArray(), boardLists.ToDictionary(x => x.Key, x => x.ToArray()));
                    var cardReleaseDate = (card.DueDate ?? cardStateInfo.States.SafeGet(CardState.Released).IfNotNull(x => (DateTime?) x.BeginDate) ?? DateTime.Now).Date;
                    return new CardNewsModel
                    {
                        CardId = card.Id,
                        CardName = card.Name,
                        Labels = card.Labels.OrderBy(x => x.Color).ToArray(),
                        CardDescription = card.Description,
                        State = cardStateInfo.CurrentState,
                        DueDate = card.DueDate,
                        CardReleaseDate = cardReleaseDate,
                        PublishDate = DateTime.Now.Date
                    };
                })
                .Where(x =>((x.State == CardState.ReleaseWaiting || x.State == CardState.Testing) && x.DueDate.HasValue) || (x.State == CardState.Released))
                .Where(x => x.CardReleaseDate >= DateTime.Now.Date.AddDays(-14))
                .ToArray();
        }
    }
}