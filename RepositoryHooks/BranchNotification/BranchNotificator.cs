using System;
using System.Linq;
using System.Text;
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
            repoNotificationChat = messenger.RegisterChat("”ведовлени€ о репозитории", "”ведовлени€ о состо€нии репозитори€ проекта дл€ разработчиков (например, об автоматическом удалении ветки).");
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
                var branchesPerCommiter = branchesClassificator.GetOldBranchesBy(commiterEmail);
                if (branchesPerCommiter.Any())
                {
                    var bodyBuilder = new StringBuilder();
                    bodyBuilder.AppendLine("ƒорогой разработчик!");
                    bodyBuilder.AppendLine();
                    bodyBuilder.AppendLine($"—пешу обратить твое внимание на старые ветки в репозитории: {string.Join(", ", branchesPerCommiter)}");
                    bodyBuilder.AppendLine("¬ них давно не ведетс€ никакой работы, это похоже на разбитое окно. ѕожалуйста, подумай кака€ ценность есть в этих ветках и как эту ценность доставить.");
                    bodyBuilder.AppendLine();
                    bodyBuilder.AppendLine("— любовью, твой автоматический уведомл€тор.");

                    var message = new Email
                    {
                        Recipients = new[] {commiterEmail},
                        Title = "”ведомление о старых ветках",
                        Body = bodyBuilder.ToString(),
                        CopyTo = "hvorost@skbkontur.ru"
                    };
                    repoNotificationChat.Post(me, message);
                }
            }
        }

        public void DeleteMergedBranchesAndNotifyCommiters()
        {
            var branchesToDelete = repository.SearchForOldBranches(mergedBranchDeadlinePeriod).Where(b => b.Merged).ToArray();
            foreach (var branch in branchesToDelete)
            {
                repository.DeleteBranch(branch.Name);
                var bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine("ƒорогой разработчик!");
                bodyBuilder.AppendLine();
                bodyBuilder.AppendLine($"—пешу обратить твое внимание, что € удалил ветку {branch.Name}, потому что она была влита в релиз и прошло больше 3-х дней с момента последнего коммита в нее.");
                bodyBuilder.AppendLine();
                bodyBuilder.AppendLine("— любовью, твой автоматический уведомл€тор.");

                var message = new Email
                {
                    Recipients = new[] { branch.Commit.Committer_email },
                    Title = "”ведомление об удаленной ветке",
                    Body = bodyBuilder.ToString(),
                    CopyTo = "hvorost@skbkontur.ru"
                };
                repoNotificationChat.Post(me, message);
            }
        }
    }
}