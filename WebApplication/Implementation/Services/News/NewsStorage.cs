using System.Linq;
using SKBKontur.Infrastructure.Sugar;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsStorage : INewsStorage
    {
        private readonly ICachedFileStorage cachedFileStorage;
        private const string DataFileName = "CardNews";

        public NewsStorage(
            ICachedFileStorage cachedFileStorage)
        {
            this.cachedFileStorage = cachedFileStorage;
        }

        public Maybe<CardNewsModel> FindNew(string cardId)
        {
            return ReadAll().FirstOrDefault(x => x.CardId == cardId);
        }

        public CardNewsModel[] ReadAll()
        {
            return cachedFileStorage.Find<CardNewsModel[]>(DataFileName) ?? new CardNewsModel[0];
        }

        public void UpdateAll(CardNewsModel[] news)
        {
            cachedFileStorage.Write(DataFileName, news);
        }

        public void Update(CardNewsModel changedNew)
        {
            var news = ReadAll();
            for (var index = 0; index < news.Length; index++)
            {
                var newsModel = news[index];
                if (newsModel.CardId.Equals(changedNew.CardId))
                {
                    news[index] = changedNew;
                }
            }
            UpdateAll(news);
        }
    }
}