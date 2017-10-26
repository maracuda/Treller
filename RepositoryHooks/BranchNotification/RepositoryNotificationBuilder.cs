using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageBroker;

namespace RepositoryHooks.BranchNotification
{
    public class RepositoryNotificationBuilder : IRepositoryNotificationBuilder
    {
        public Message Build(string commiterEmail, IEnumerable<string> oldBranches)
        {
            if (!oldBranches.Any())
                throw new ArgumentException($"Fail to build message for {commiterEmail} with empty list of branches.");

            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine("Дорогой разработчик!");
            bodyBuilder.AppendLine();
            bodyBuilder.AppendLine("Спешу обратить твое внимание на старые ветки в репозитории.");
            bodyBuilder.AppendLine($"Старые ветки: {string.Join(",", oldBranches)}.");
            bodyBuilder.AppendLine("Пожалуйста, посмотри нельзя ли завершить работу с этими ветками (репозиторию очень тяжело).");
            bodyBuilder.AppendLine();
            bodyBuilder.AppendLine("С любовью твой автоматический уведомлятор.");

            return new Message
            {
                Recipients = new []{ commiterEmail },
                Title = "Уведомление о зарелизенных ветках",
                Body = bodyBuilder.ToString(),
                CopyTo = "hvorost@skbkontur.ru"
            };
        }
    }
}