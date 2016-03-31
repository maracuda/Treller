using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Ajax.Utilities;
using SKBKontur.BlocksMapping.BlockExtenssions;
using SKBKontur.Infrastructure.CommonExtenssions;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;
using SKBKontur.Treller.WebApplication.Implementation.Services.Notifications;
using SKBKontur.Treller.WebApplication.Implementation.Services.Settings;
using SKBKontur.Treller.WebApplication.Implementation.Services.TaskCacher;
using SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsService : INewsService
    {
        private const string CardNewsName = "CardNews";
        private const string NewsEmailsStoreName = "NewsEmailsToSend";

        #region init


        private readonly ICachedFileStorage cachedFileStorage;
        private readonly ISettingService settingService;
        private readonly ITaskCacher taskCacher;
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ICardStateInfoBuilder cardStateInfoBuilder;
        private readonly INotificationService notificationService;

        public NewsService(ICachedFileStorage cachedFileStorage,
                           ISettingService settingService,
                           ITaskCacher taskCacher,
                           ITaskManagerClient taskManagerClient,
                           ICardStateInfoBuilder cardStateInfoBuilder,
                           INotificationService notificationService)
        {
            this.cachedFileStorage = cachedFileStorage;
            this.settingService = settingService;
            this.taskCacher = taskCacher;
            this.taskManagerClient = taskManagerClient;
            this.cardStateInfoBuilder = cardStateInfoBuilder;
            this.notificationService = notificationService;
        }
        #endregion

        public void Refresh()
        {
            var boardSettings = settingService.GetDevelopingBoards().ToDictionary(x => x.Id);
            var boardIds = boardSettings.Select(x => x.Key).ToArray();
            var cards = taskCacher.GetCached(boardIds, strings => taskManagerClient.GetBoardCardsAsync(strings).Result, TaskCacherStoredTypes.BoardCards);
            var boardLists = taskCacher.GetCached(boardIds, ids => taskManagerClient.GetBoardListsAsync(ids).Result, TaskCacherStoredTypes.BoardLists).ToLookup(x => x.BoardId);
            var cardActions = taskCacher.GetCached(boardIds, strings => taskManagerClient.GetActionsForBoardCardsAsync(strings).Result, TaskCacherStoredTypes.BoardActions).ToLookup(x => x.CardId);

            var actualCards = cards
                .Where(x => !x.Name.Contains("Автотесты", StringComparison.OrdinalIgnoreCase) && x.LastActivity.Date > DateTime.Now.Date.AddDays(-30))
                .Select(card =>
                {
                    var cardStateInfo = cardStateInfoBuilder.Build(cardActions[card.Id].ToArray(), boardSettings, boardLists.ToDictionary(x => x.Key, x => x.ToArray()));
                    var cardReleaseDate = (card.DueDate ?? cardStateInfo.States.SafeGet(CardState.Released).IfNotNull(x => (DateTime?)x.BeginDate) ?? DateTime.Now).Date;
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
                .Where(x => ((x.State == CardState.ReleaseWaiting || x.State == CardState.Testing) && x.DueDate.HasValue) || (x.State == CardState.Released))
                .Where(x => x.CardReleaseDate >= DateTime.Now.Date.AddDays(-14))
                .ToArray();

            var currentModels = (cachedFileStorage.Find<CardNewsModel[]>(CardNewsName) ?? new CardNewsModel[0]).ToDictionary(x => x.CardId);
            foreach (var card in actualCards.Where(card => currentModels.ContainsKey(card.CardId)))
            {
                var oldCard = currentModels[card.CardId];
                card.PublishDate = oldCard.PublishDate;
                card.IsDeleted = oldCard.IsDeleted;
                card.IsTechnicalNewsPublished = oldCard.IsTechnicalNewsPublished;
                card.IsNewsPublished = oldCard.IsNewsPublished;
            }
            cachedFileStorage.Write(CardNewsName, actualCards);
        }

        public NewsViewModel GetNews()
        {
            var cardsForNews = cachedFileStorage.Find<CardNewsModel[]>(CardNewsName) ?? new CardNewsModel[0];
            var emails = cachedFileStorage.Find<NewsEmail>(NewsEmailsStoreName) ?? CreateDefaultEmail();

            return new NewsViewModel
            {
                ReleaseEmail = emails.ReleaseEmail,
                TechnicalEmail = emails.TechnicalEmail,
                NewsToPublish = BuildNewsModel(cardsForNews, false),
                TechnicalNewsToPublish = BuildNewsModel(cardsForNews, true),
                NotActualCards = cardsForNews.Where(x => x.IsPublished() || x.IsDeleted).ToArray(),
                CardsWihoutNews = cardsForNews.Where(x => !x.IsPublished() && !x.IsDeleted && !x.IsNewsExists()).ToArray(),
                ActualCards = cardsForNews.Where(x => !x.IsPublished() && !x.IsDeleted && x.IsNewsExists()).ToArray()
            };
        }

        private NewsEmail CreateDefaultEmail()
        {
            return new NewsEmail
            {
                TechnicalEmail = notificationService.GetNotificationRecipient(),
                ReleaseEmail = notificationService.GetNotificationRecipient()
            };
        }

        private static NewsModel BuildNewsModel(CardNewsModel[] cards, bool isTechnicalNews, bool inHtmlStyle = false)
        {
            var newLine = inHtmlStyle ? "<br/>" : Environment.NewLine;
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
                    news.AppendFormat("Вечером {1:D} {0}:", card.CardReleaseDate >= DateTime.Today ? "будем релизить" : "состоялся релиз", card.CardReleaseDate);
                    news.Append(newLine);
                    news.Append(newLine);
                }
                releaseDate = card.CardReleaseDate;

                news.Append(inHtmlStyle ? "<b>" : "Задача: ");
                news.Append(card.CardName);
                news.Append(inHtmlStyle ? "</b>" : "");
                news.Append(newLine);
                news.Append(newsText);
                news.Append(newLine);
                news.Append(newLine);
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

        public void DeleteCard(string cardId)
        {
            UpdateCard(cardId, card => card.IsDeleted = true);
        }

        public void RestoreCard(string cardId)
        {
            UpdateCard(cardId, card =>
            {
                card.IsTechnicalNewsPublished = false;
                card.IsNewsPublished = false;
                card.IsDeleted = false;
                card.PublishDate = DateTime.Today;
            });
        }

        private void UpdateCard(string cardId, Action<CardNewsModel> updateCardAction)
        {
            var newsCards = cachedFileStorage.Find<CardNewsModel[]>(CardNewsName) ?? new CardNewsModel[0];
            var cardToDelete = newsCards.FirstOrDefault(x => x.CardId == cardId);

            if (cardToDelete == null)
            {
                return;
            }

            updateCardAction(cardToDelete);
            cachedFileStorage.Write(CardNewsName, newsCards);
        }

        public void SendTechnicalNews()
        {
            SendNews(true);
        }

        public void SendNews()
        {
            SendNews(false);
        }

        private void SendNews(bool technical)
        {
            var emails = cachedFileStorage.Find<NewsEmail>(NewsEmailsStoreName) ?? CreateDefaultEmail();
            var cards = cachedFileStorage.Find<CardNewsModel[]>(CardNewsName) ?? new CardNewsModel[0];
            var inHtmlStyle = true;
            var newsModel = BuildNewsModel(cards, technical, inHtmlStyle);
            if (newsModel == null || newsModel.Cards.Length == 0)
            {
                return;
            }

            var body = string.Format("{1}{0}{0}Вы можете ответить на это письмо, если у вас возникли вопросы или комментарии касающиеся релизов{0}{0}--{0}С уважением, команда Контур.Биллинг", inHtmlStyle ? "<br/>" : Environment.NewLine, newsModel.NewsText);
            notificationService.SendMessage(emails.GetEmail(technical), newsModel.NewsHeader, body, inHtmlStyle);

            foreach (var card in newsModel.Cards)
            {
                card.Publish(technical);
            }
            
            cachedFileStorage.Write(CardNewsName, cards);
        }

        public void UpdateEmail(string technicalEmail, string releaseEmail)
        {
            var emails = cachedFileStorage.Find<NewsEmail>(NewsEmailsStoreName) ?? CreateDefaultEmail();

            emails.ReleaseEmail = releaseEmail;
            emails.TechnicalEmail = technicalEmail;
            cachedFileStorage.Write(NewsEmailsStoreName, emails);
        }

        public void UpdateEmailToBattleValues()
        {
            UpdateEmail("tech.news.billing@skbkontur.ru", "news.billing@skbkontur.ru");
        }

        public bool IsAnyNewsExists()
        {
            return (cachedFileStorage.Find<CardNewsModel[]>(CardNewsName) ?? new CardNewsModel[0]).Any(x => x.IsNewsExists() && !x.IsPublished() && !x.IsDeleted);
        }
    }
}