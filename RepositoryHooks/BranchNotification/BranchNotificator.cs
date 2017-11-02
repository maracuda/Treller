using System;
using System.Linq;
using MessageBroker;
using TaskManagerClient.Repository;

namespace RepositoryHooks.BranchNotification
{
    public class BranchNotificator : IBranchNotificator
    {
        private static readonly TimeSpan branchDeadlinePeriod = TimeSpan.FromDays(90);
        private static readonly TimeSpan mergedBranchDeadlinePeriod = TimeSpan.FromDays(3);

        private readonly IRepository repository;
        private readonly IRepositoryMessageBuilder repositoryMessageBuilder;
        private readonly IEmailMessageProducer emailMessageProducer;

        public BranchNotificator(
            IRepository repository,
            IRepositoryMessageBuilder repositoryMessageBuilder,
            IEmailMessageProducer emailMessageProducer)
        {
            this.repository = repository;
            this.repositoryMessageBuilder = repositoryMessageBuilder;
            this.emailMessageProducer = emailMessageProducer;
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
                    var message = repositoryMessageBuilder.CreateOldBranchesMessage(commiterEmail, branchesPerCommiter);
                    emailMessageProducer.Publish(message);
                }
            }
        }

        public void DeleteMergedBranchesAndNotifyCommiters()
        {
            var branchesToDelete = repository.SearchForOldBranches(mergedBranchDeadlinePeriod).Where(b => b.Merged).ToArray();
            foreach (var branch in branchesToDelete)
            {
                repository.DeleteBranch(branch.Name);
                var message = repositoryMessageBuilder.CreateBranchDeletedMessage(branch.Commit.Committer_email, branch.Name);
                emailMessageProducer.Publish(message);
            }
        }
    }
}