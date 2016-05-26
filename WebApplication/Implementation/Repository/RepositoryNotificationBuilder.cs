using System.Collections.Generic;
using SKBKontur.TaskManagerClient.Notifications;

namespace SKBKontur.Treller.WebApplication.Implementation.Repository
{
    public class RepositoryNotificationBuilder : IRepositoryNotificationBuilder
    {
        public Notification BuildForOldBranch(string commiterEmail, IEnumerable<string> oldBranches)
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

        public Notification BuildForReleasedBranch(string commiterEmail, IEnumerable<string> releasedBranches)
        {
            var body = "Дорогой разработчик!\r\n\r\n" +
           $"Спешу сообщить тебе, что у нас в репозитории есть ветки влитые в релиз, но не удаленные: {string.Join(",", releasedBranches)}.\r\n" +
           "По воле случая ты был последним, кто коммитил в эту ветку/и.\r\n" +
           "Пожалуйста, посмотри нельзя ли удалить эти ветки (репозиторию очень тяжело от большого количества веток).\r\n\r\n" +
           "С любовью твой автоматический уведомлятор.\r\n";
            return new Notification
            {
                Recipient = commiterEmail,
                Title = "Уведомление о зарелизенных ветках",
                Body = body,
                CopyTo = "hvorost@skbkontur.ru",
                IsHtml = false
            };
        }
    }
}