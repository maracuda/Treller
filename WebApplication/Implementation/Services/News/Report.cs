using System;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Publisher;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class Report
    {
        public string TaskId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public PublishStrategy PublishStrategy { get; set; }
        public DateTime? DoNotDeliverUntil { get; set; }
        public DateTime? PublishDate { get; set; }

        public bool TryPublish(INewsNotificator notificator, DateTime now)
        {
            if (DoNotDeliverUntil.HasValue && DoNotDeliverUntil.Value > now)
                return false;

            if (PublishDate.HasValue)
                return false;

            Publish(notificator, now);
            return true;
        }

        private void Publish(INewsNotificator notificator, DateTime now)
        {
            var mailingList = ChooseMailingList();
            notificator.NotifyAboutReleases(mailingList, Title, Message);
            PublishDate = now;
        }

        private string ChooseMailingList()
        {
            switch (PublishStrategy)
            {
                case PublishStrategy.Customer:
                    return "news.billing@skbkontur.ru";
                case PublishStrategy.Support:
                    return "tech.news.billing@skbkontur.ru";
                case PublishStrategy.Team:
                    return "hvorost@skbkontur.ru";
                default:
                    throw new Exception($"Fail to find mailing list for publish strategy {PublishStrategy}.");
            }
        }
    }
}