using System;
using System.Linq;
using System.Text;
using System.Web;
using MessageBroker;
using MessageBroker.Messages;
using TaskManagerClient.Repository;

namespace RepositoryHooks.BranchNotification
{
    public class BranchNotificator : IBranchNotificator
    {
        private static readonly TimeSpan branchDeadlinePeriod = TimeSpan.FromDays(50);
        private static readonly TimeSpan mergedBranchDeadlinePeriod = TimeSpan.FromDays(3);

        private readonly IRepository repository;
        private readonly IChat repoNotificationChat;
        private readonly Member me;

        public BranchNotificator(
            IRepository repository,
            IMessenger messenger)
        {
            this.repository = repository;
            repoNotificationChat = messenger.RegisterChat("Уведовления о репозитории", "Уведовления о состоянии репозитория проекта для разработчиков (например, об автоматическом удалении ветки).");
            me = messenger.RegisterBotMember(GetType());
        }

        public void NotifyCommitersAboutOldBranches()
        {
            var oldBranches = repository.SearchForOldBranches(branchDeadlinePeriod)
                                     .Where(b => !b.Merged)
                                     .ToArray();
            var branchesClassificator = BranchClassificator.Create(oldBranches);
            
            foreach (var commiterEmail in branchesClassificator.CommiterEmails)
            {
                var branchesPerCommiter = branchesClassificator
                    .GetOldBranchesBy(commiterEmail)
                    .Select(BuildBranchHref)
                    .ToArray();
                    
                if (branchesPerCommiter.Any())
                {
                    var bodyBuilder = new StringBuilder();
                    bodyBuilder.AppendLine("Дорогой разработчик!");
                    bodyBuilder.AppendLine();
                    bodyBuilder.AppendLine($"Спешу обратить твое внимание на старые ветки в репозитории: {string.Join(", ", branchesPerCommiter)}");
                    bodyBuilder.AppendLine("В них давно не ведется никакой работы, это похоже на разбитое окно. Пожалуйста, подумай какая ценность есть в этих ветках и как эту ценность доставить.");
                    bodyBuilder.AppendLine();
                    bodyBuilder.AppendLine("С любовью, твой автоматический уведомлятор.");

                    var message = new Email
                    {
                        Recipients = new[] {commiterEmail},
                        Title = "Уведомление о старых ветках",
                        Body = bodyBuilder.ToString(),
                        CopyTo = "hvorost@skbkontur.ru"
                    };
                    repoNotificationChat.Post(me, message);
                }
            }
        }

        private static string BuildBranchHref(string branchName)
        {
            var url = $"https://git.skbkontur.ru/billy/billy/branches/all?search={HttpUtility.UrlEncode(branchName)}";
            return $"<a href=\"{url}\">{branchName}</a>";
        }

        public void DeleteMergedBranchesAndNotifyCommiters()
        {
            var branchesToDelete = repository.SearchForOldBranches(mergedBranchDeadlinePeriod).Where(b => b.Merged).ToArray();
            foreach (var branch in branchesToDelete)
            {
                repository.DeleteBranch(branch.Name);
                var bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine("Дорогой разработчик!");
                bodyBuilder.AppendLine();
                bodyBuilder.AppendLine($"Спешу обратить твое внимание, что я удалил ветку {branch.Name}, потому что она была влита в релиз и прошло больше 3-х дней с момента последнего коммита в нее.");
                bodyBuilder.AppendLine();
                bodyBuilder.AppendLine("С любовью, твой автоматический уведомлятор.");

                var message = new Email
                {
                    Recipients = new[] { branch.Commit.Committer_email },
                    Title = "Уведомление об удаленной ветке",
                    Body = bodyBuilder.ToString(),
                    CopyTo = "hvorost@skbkontur.ru"
                };
                repoNotificationChat.Post(me, message);
            }
        }
    }
}