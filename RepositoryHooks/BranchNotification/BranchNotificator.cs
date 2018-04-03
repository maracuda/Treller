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
            repoNotificationChat = messenger.RegisterChat("����������� � �����������", "����������� � ��������� ����������� ������� ��� ������������� (��������, �� �������������� �������� �����).");
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
                    bodyBuilder.AppendLine("������� �����������!");
                    bodyBuilder.AppendLine();
                    bodyBuilder.AppendLine($"����� �������� ���� �������� �� ������ ����� � �����������: {string.Join(", ", branchesPerCommiter)}");
                    bodyBuilder.AppendLine("� ��� ����� �� ������� ������� ������, ��� ������ �� �������� ����. ����������, ������� ����� �������� ���� � ���� ������ � ��� ��� �������� ���������.");
                    bodyBuilder.AppendLine();
                    bodyBuilder.AppendLine("� �������, ���� �������������� �����������.");

                    var message = new Email
                    {
                        Recipients = new[] {commiterEmail},
                        Title = "����������� � ������ ������",
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
                bodyBuilder.AppendLine("������� �����������!");
                bodyBuilder.AppendLine();
                bodyBuilder.AppendLine($"����� �������� ���� ��������, ��� � ������ ����� {branch.Name}, ������ ��� ��� ���� ����� � ����� � ������ ������ 3-� ���� � ������� ���������� ������� � ���.");
                bodyBuilder.AppendLine();
                bodyBuilder.AppendLine("� �������, ���� �������������� �����������.");

                var message = new Email
                {
                    Recipients = new[] { branch.Commit.Committer_email },
                    Title = "����������� �� ��������� �����",
                    Body = bodyBuilder.ToString(),
                    CopyTo = "hvorost@skbkontur.ru"
                };
                repoNotificationChat.Post(me, message);
            }
        }
    }
}