using System;
using System.Linq;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsService : INewsService
    {
        private readonly INewsSettingsService newsSettingsService;
        private readonly INewsStorage newsStorage;
        private readonly INewsNotificator newsNotificator;
        private readonly INewsModelBuilder newsModelBuilder;
        private readonly IErrorService errorService;
        private readonly IDateTimeFactory dateTimeFactory;

        public NewsService(INewsSettingsService newsSettingsService,
                           INewsStorage newsStorage,
                           INewsNotificator newsNotificator,
                           INewsModelBuilder newsModelBuilder,
                           IErrorService errorService,
                           IDateTimeFactory dateTimeFactory)
        {
            this.newsSettingsService = newsSettingsService;
            this.newsStorage = newsStorage;
            this.newsNotificator = newsNotificator;
            this.newsModelBuilder = newsModelBuilder;
            this.errorService = errorService;
            this.dateTimeFactory = dateTimeFactory;
        }

        public void Refresh()
        {
            var actualsNews = newsModelBuilder.BuildNewsModel();
            //TODO: move it to storage layer
            var idToNewMap = newsStorage.ReadAll().ToDictionary(x => x.CardId);
            foreach (var card in actualsNews.Where(card => idToNewMap.ContainsKey(card.CardId)))
            {
                var oldCard = idToNewMap[card.CardId];
                card.PublishDate = oldCard.PublishDate;
                card.IsDeleted = oldCard.IsDeleted;
                card.IsTechnicalNewsPublished = oldCard.IsTechnicalNewsPublished;
                card.IsNewsPublished = oldCard.IsNewsPublished;
            }
            newsStorage.UpdateAll(actualsNews);
        }

        public CardNewsModel[] GetAllNews()
        {
            return newsStorage.ReadAll();
        }

        public void DeleteCard(string cardId)
        {
            UpdateNew(cardId, n => n.MarkDeleted());
        }

        public void RestoreCard(string cardId)
        {
            UpdateNew(cardId, n => n.ResetState(dateTimeFactory.Today));
        }

        private void UpdateNew(string cardId, Action<CardNewsModel> updateAction)
        {
            var newsModel = newsStorage.FindNew(cardId);
            if (newsModel.HasNoValue)
            {
                errorService.SendError($"Fail to find newsModel with cardId {cardId}.", new Exception());
                return;
            }

            updateAction(newsModel.Value);
            newsStorage.Update(newsModel.Value);
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
            var cards = newsStorage.ReadAll();
            var newsModel = technical ? newsModelBuilder.BuildViewModel().TechnicalNewsToPublish : newsModelBuilder.BuildViewModel().NewsToPublish;
            if (newsModel == null || newsModel.Cards.Length == 0)
            {
                return;
            }

            newsNotificator.NotifyAboutReleases(technical ? newsSettingsService.GetOrRead().TechMailingList : newsSettingsService.GetOrRead().PublicMailingList, newsModel);
            foreach (var card in newsModel.Cards)
            {
                card.MarkPublished(technical);
            }
            newsStorage.UpdateAll(cards);
        }

        public bool IsAnyNewsExists()
        {
            return newsStorage.ReadAll().Any(x => x.IsNewsExists() && !x.IsPublished() && !x.IsDeleted);
        }
    }
}