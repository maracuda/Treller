using System;
using System.Collections.Generic;
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

        public void NotifyCommitersAboutMergedBranches(TimeSpan maxMergingTimeSpan)
        {
            var commiterIndex = new Dictionary<string, List<string>>();
            var releasedBranches = repository.SearchForMergedToReleaseBranches(maxMergingTimeSpan);
            foreach (var releasedBranch in releasedBranches)
            {
                if (!commiterIndex.ContainsKey(releasedBranch.LastCommit.Author_email))
                {
                    commiterIndex.Add(releasedBranch.LastCommit.Author_email, new List<string>());
                }
                commiterIndex[releasedBranch.LastCommit.Author_email].Add(releasedBranch.Name);
            }

            foreach (var emailToBranchesPair in commiterIndex)
            {
                var notification = repositoryNotificationBuilder.BuildForReleasedBranch(emailToBranchesPair.Key, emailToBranchesPair.Value);
                messageProducer.Publish(notification);
            }
        }

        public void NotifyCommitersAboutIdlingBranches(TimeSpan branchIdlingMinTimeSpan)
        {
            var commiterIndex = new Dictionary<string, List<string>>();
            var oldBranches = repository.SearchForOldBranches(branchIdlingMinTimeSpan);
            foreach (var veryOldBranch in oldBranches)
            {
                if (!commiterIndex.ContainsKey(veryOldBranch.Commit.Committer_email))
                {
                    commiterIndex.Add(veryOldBranch.Commit.Committer_email, new List<string>());
                }
                commiterIndex[veryOldBranch.Commit.Committer_email].Add(veryOldBranch.Name);
            }

            foreach (var emailToBranchesPair in commiterIndex)
            {
                var notification = repositoryNotificationBuilder.BuildForOldBranch(emailToBranchesPair.Key, emailToBranchesPair.Value);
                messageProducer.Publish(notification);
            }
        }
    }
}