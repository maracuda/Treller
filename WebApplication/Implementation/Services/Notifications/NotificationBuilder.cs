using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Notifications
{
    public class NotificationBuilder : INotificationBuilder
    {
        public Notification BuildForOldBranchNotification(string commiterEmail, IEnumerable<string> oldBranches)
        {
            var body = "Дорогой разработчик!\r\n\r\n" +
                       $"Спешу сообщить тебе, что у нас в репозитории есть очень старые ветки: {string.Join(",", oldBranches)}.\r\n" +
                       "По воле случая ты был последним, кто коммитил в эту ветку/и.\r\n" +
                       "Пожалуйста, посмотри нельзя ли закрыть эти ветки (репозиторию очень тяжело от большого количества веток).\r\n\r\n" +
                       "С любовью твой автоматический уведомлятор.\r\n";
            return new Notification
            {
                Recipient = commiterEmail,
                Title = "Уведомление о старых ветках",
                Body = body,
                CopyTo = "hvorost@skbkontur.ru",
                IsHtml = false
            };
        }
    }
}