using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public interface INewsStorage
    {
        Maybe<CardNewsModel> FindNew(string cardId);
        CardNewsModel[] ReadAll();
        void UpdateAll(CardNewsModel[] news);
        void Update(CardNewsModel changedNew);
    }
}