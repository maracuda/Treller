using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageBroker;

namespace RepositoryHooks.BranchNotification
{
    public class RepositoryMessageBuilder : IRepositoryMessageBuilder
    {
        public EmailMessage CreateOldBranchesMessage(string commiterEmail, IEnumerable<string> oldBranchNames)
        {
            if (!oldBranchNames.Any())
                throw new ArgumentException($"Fail to build message for {commiterEmail} with empty list of branches.");

            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine("Дорогой разработчик!");
            bodyBuilder.AppendLine();
            bodyBuilder.AppendLine($"Спешу обратить твое внимание на старые ветки в репозитории: {string.Join(", ", oldBranchNames)}");
            bodyBuilder.AppendLine("Пожалуйста, посмотри нельзя ли завершить работу с этими ветками (репозиторию очень тяжело).");
            bodyBuilder.AppendLine();
            bodyBuilder.AppendLine("С любовью, твой автоматический уведомлятор.");

            return new EmailMessage
            {
                Recipients = new []{ commiterEmail },
                Title = "Уведомление о старых ветках",
                Body = bodyBuilder.ToString(),
                CopyTo = "hvorost@skbkontur.ru"
            };
        }

        public EmailMessage CreateBranchDeletedMessage(string commiterEmail, string deletedBranchName)
        {
            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine("Дорогой разработчик!");
            bodyBuilder.AppendLine();
            bodyBuilder.AppendLine($"Спешу обратить твое внимание, что я удалил ветку {deletedBranchName}, потому что она была влита в релиз и прошло больше 3-х дней с момента последнего коммита в нее.");
            bodyBuilder.AppendLine();
            bodyBuilder.AppendLine("С любовью, твой автоматический уведомлятор.");

            return new EmailMessage
            {
                Recipients = new[] { commiterEmail },
                Title = "Уведомление об удаленной ветке",
                Body = bodyBuilder.ToString(),
                CopyTo = "hvorost@skbkontur.ru"
            };
        }
    }
}