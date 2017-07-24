using System;
using System.Collections.Generic;
using MessageBroker;
using TaskManagerClient.Repository;

namespace RepositoryHooks.BranchNotification
{
    public class BranchNotificator : IBranchNotificator
    {
        private readonly IRepository repository;
        private readonly IRepositoryNotificationBuilder repositoryNotificationBuilder;
        private readonly IMessageProducer messageProducer;

        public BranchNotificator(
            IRepository repository,
            IRepositoryNotificationBuilder repositoryNotificationBuilder,
            IMessageProducer messageProducer)
        {
            this.repository = repository;
            this.repositoryNotificationBuilder = repositoryNotificationBuilder;
            this.messageProducer = messageProducer;
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