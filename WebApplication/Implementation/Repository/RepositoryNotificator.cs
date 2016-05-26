using System;
using System.Collections.Generic;
using SKBKontur.TaskManagerClient.Repository;
using SKBKontur.Treller.WebApplication.Implementation.Services.Notifications;

namespace SKBKontur.Treller.WebApplication.Implementation.Repository
{
    public class RepositoryNotificator : IRepositoryNotificator
    {
        private readonly IRepository repository;
        private readonly IRepositoryNotificationBuilder repositoryNotificationBuilder;
        private readonly INotificationService notificationService;

        public RepositoryNotificator(
            IRepository repository,
            IRepositoryNotificationBuilder repositoryNotificationBuilder,
            INotificationService notificationService)
        {
            this.repository = repository;
            this.repositoryNotificationBuilder = repositoryNotificationBuilder;
            this.notificationService = notificationService;
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
                notificationService.Send(notification);
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
                notificationService.Send(notification);
            }
        }
    }
}