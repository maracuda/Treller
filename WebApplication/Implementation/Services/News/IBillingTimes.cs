using WebApplication.Implementation.Services.News.NewsFeed;

namespace WebApplication.Implementation.Services.News
{
    public interface IBillingTimes
    {
        void LookForNews();
        bool TryToRequestNew(string aboutCardId);

        TaskNewModel[] SelectAll();
        TaskNewModel Read(string taskId);

        void Publish(string taskId, PublishStrategy publishStrategy);
    }
}