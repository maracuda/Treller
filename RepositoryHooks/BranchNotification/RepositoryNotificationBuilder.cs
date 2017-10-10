using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageBroker;

namespace RepositoryHooks.BranchNotification
{
    public class RepositoryNotificationBuilder : IRepositoryNotificationBuilder
    {
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