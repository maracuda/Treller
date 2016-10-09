﻿using SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Publisher;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Reporters;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
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
            return result.HasValue;
        }

        public TaskNewModel[] SelectAll()
        {
            return newsFeed.SelectAll();
        }

        public void Publish(string taskNewId)
        {
            publisher.Publish(taskNewId);
        }
    }
}