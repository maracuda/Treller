using WebApplication.Implementation.Services.News.NewsFeed;
using WebApplication.Implementation.Services.News.Publisher;
using WebApplication.Implementation.Services.News.Reporters;

namespace WebApplication.Implementation.Services.News
{
    public class BillingTimes : IBillingTimes
    {
        private readonly INewsFeed newsFeed;
        private readonly IReporter reporter;
        private readonly IPublisher publisher;

        public BillingTimes(
            INewsFeed newsFeed,
            IReporter reporter,
            IPublisher publisher)
        {
            this.newsFeed = newsFeed;
            this.reporter = reporter;
            this.publisher = publisher;
        }

        public void LookForNews()
        {
            newsFeed.AddNews(reporter.MakeReport());
        }

        public bool TryToRequestNew(string aboutCardId)
        {
            var result = reporter.TryToMakeReport(aboutCardId);
            if (result.HasValue)
            {
                newsFeed.AddNews(result.Value);
            }
            return result.HasValue;
        }

        public TaskNewModel[] SelectAll()
        {
            return newsFeed.SelectAll();
        }

        public TaskNewModel Read(string taskId)
        {
            return newsFeed.Read(taskId);
        }

        public void Publish(string taskId, PublishStrategy publishStrategy)
        {
            publisher.Publish(taskId, publishStrategy);
        }
    }
}