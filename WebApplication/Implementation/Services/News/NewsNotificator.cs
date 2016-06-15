using SKBKontur.TaskManagerClient.Notifications;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsNotificator : INewsNotificator
    {
        private readonly INotificationService notificationService;

        public NewsNotificator(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public void NotifyAboutReleases(string recipient, NewsModel newsModel)
        {
            var body = $"{newsModel.NewsText}<br/><br/>Вы можете ответить на это письмо, если у вас возникли вопросы или комментарии касающиеся релизов<br/><br/>--<br/>С уважением, команда Контур.Биллинг";
            var notification = new Notification
            {
                Title = newsModel.NewsHeader,
                Body = body,
                IsHtml = true,
                Recipient = recipient,
                ReplyTo = "ask.billing@skbkontur.ru"
            };
            notificationService.Send(notification);
        }
    }
}