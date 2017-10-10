using System;
using Infrastructure.Common;
using MessageBroker;
using TaskManagerClient.Repository;

namespace RepositoryHooks.BranchNotification
{
    public class BranchNotificator : IBranchNotificator
    {
        private static readonly TimeSpan branchDeadlinePeriod = TimeSpan.FromDays(90);

        private readonly IRepository repository;
        private readonly IRepositoryNotificationBuilder repositoryNotificationBuilder;
        private readonly IMessageProducer messageProducer;
        private readonly IDateTimeFactory dateTimeFactory;

        public BranchNotificator(
            IRepository repository,
            IRepositoryNotificationBuilder repositoryNotificationBuilder,
            IMessageProducer messageProducer,
            IDateTimeFactory dateTimeFactory)
        {
            this.repository = repository;
            this.repositoryNotificationBuilder = repositoryNotificationBuilder;
            this.messageProducer = messageProducer;
            this.dateTimeFactory = dateTimeFactory;
        }

        public void NotifyCommitersAboutOldBranches()
        {
            var branches = repository.SearchForOldBranches(branchDeadlinePeriod);
            var branchesClassificator = BranchClassificator.Create(dateTimeFactory.Now.Subtract(branchDeadlinePeriod), branches);
            
            foreach (var commiterEmail in branchesClassificator.GetCommitersEmail)
            {
                var message = repositoryNotificationBuilder.Build(commiterEmail, branchesClassificator.GetMergedBranchesBy(commiterEmail), branchesClassificator.GetOldBranchesBy(commiterEmail));
                messageProducer.Publish(message);
            }
        }
    }
}