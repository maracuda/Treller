using SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public interface IBillingTimes
    {
        void LookForNews();
        bool TryToRequestNew(string aboutCardId);

        TaskNewModel[] SelectAll();

        void Publish(string taskNewId);
    }
}