using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageBroker;

namespace RepositoryHooks.BranchNotification
{
    public class RepositoryNotificationBuilder : IRepositoryNotificationBuilder
    {
        public Message BuildForOldBranch(string commiterEmail, IEnumerable<string> oldBranches)
        {
            var body = "Дорогой разработчик!\r\n\r\n" +
                       $"Спешу сообщить тебе, что у нас в репозитории есть очень старые ветки: {string.Join(",", oldBranches)}.\r\n" +
                       "По воле случая ты был последним, кто коммитил в эту ветку/и.\r\n" +
                       "Пожалуйста, посмотри нельзя ли закрыть эти ветки (репозиторию очень тяжело от большого количества веток).\r\n\r\n" +
                       "С любовью твой автоматический уведомлятор.\r\n";
            return new Message
            {
                Recipient = commiterEmail,
                Title = "Уведомление о старых ветках",
                Body = body,
                CopyTo = "hvorost@skbkontur.ru"
            };
        }

        public Message BuildForReleasedBranch(string commiterEmail, IEnumerable<string> releasedBranches)
        {
            var body = "Дорогой разработчик!\r\n\r\n" +
           $"Спешу сообщить тебе, что у нас в репозитории есть ветки влитые в релиз, но не удаленные: {string.Join(",", releasedBranches)}.\r\n" +
           "По воле случая ты был последним, кто коммитил в эту ветку/и.\r\n" +
           "Пожалуйста, посмотри нельзя ли удалить эти ветки (репозиторию очень тяжело от большого количества веток).\r\n\r\n" +
           "С любовью твой автоматический уведомлятор.\r\n";
            return new Message
            {
                Recipient = commiterEmail,
                Title = "Уведомление о зарелизенных ветках",
                Body = body,
                CopyTo = "hvorost@skbkontur.ru"
            };
        }

        public Message Build(string commiterEmail, IEnumerable<string> mergedBranches, IEnumerable<string> oldBranches)
        {
            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine("Дорогой разработчик!");
            bodyBuilder.AppendLine();
            bodyBuilder.AppendLine("Спешу сообщить тебе, что в репозитории есть ветки, которые требуют твоего внимания.");
            if (mergedBranches.Any())
            {
                bodyBuilder.AppendLine($"Влитые ветки: {string.Join(",", mergedBranches)}.");
            }
            if (oldBranches.Any())
            {
                bodyBuilder.AppendLine($"Старые ветки: {string.Join(",", mergedBranches)}.");
            }
            bodyBuilder.AppendLine("Пожалуйста, посмотри нельзя ли завершить работу с этими ветками (репозиторию очень тяжело от большого количества веток).");
            bodyBuilder.AppendLine();
            bodyBuilder.AppendLine("С любовью твой автоматический уведомлятор.");

            return new Message
            {
                Recipient = "hvorost@skbkontur.ru",//commiterEmail,
                Title = "Уведомление о зарелизенных ветках",
                Body = bodyBuilder.ToString(),
                CopyTo = "hvorost@skbkontur.ru"
            };
        }
    }
}