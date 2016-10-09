using SKBKontur.TaskManagerClient.Notifications;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Publisher
{
    public class NewsNotificator : INewsNotificator
    {
        private readonly INotificationService notificationService;

        public NewsNotificator(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public void NotifyAboutReleases(string mailingList, string title, string text)
        {
            var body = $"{text}<br/><br/>Вы можете ответить на это письмо, если у вас возникли вопросы или комментарии касающиеся релизов<br/><br/>--<br/>С уважением, команда Контур.Биллинг";
            var notification = new Notification
            {
                Title = title,
                Body = body,
                IsHtml = true,
                Recipient = mailingList,
                ReplyTo = "ask.billing@skbkontur.ru"
            };
            notificationService.Send(notification);
        }
    }
}