using SKBKontur.Treller.WebApplication.Implementation.Services.Notifications;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsEmail
    {
        public string TechnicalEmail { get; set; }
        public string ReleaseEmail { get; set; }

        public static NewsEmail Default(INotificationCredentials notificationCredentials)
        {
            var defaultEmail = notificationCredentials.GetNotificationEmailAddress();
            return new NewsEmail { ReleaseEmail = defaultEmail, TechnicalEmail = defaultEmail };
        }

        public string GetEmail(bool technical)
        {
            return technical ? TechnicalEmail : ReleaseEmail;
        }
    }
}