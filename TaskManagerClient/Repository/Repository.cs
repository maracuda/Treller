using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Common;
using TaskManagerClient.Repository.BusinessObjects;
using TaskManagerClient.Repository.Clients;

namespace TaskManagerClient.Repository
{
    public class Repository : IRepository
    {
        private readonly IRepositoryClient repositoryClient;
        private readonly IRepositorySettings repositorySettings;
        private readonly IDateTimeFactory dateTimeFactory;

        public Repository(
            IRepositorySettings repositorySettings,
            IRepositoryClientFactory repositoryClientFactory,
            IDateTimeFactory dateTimeFactory
            )
        {
            this.repositorySettings = repositorySettings;
            this.dateTimeFactory = dateTimeFactory;
            repositoryClient = repositoryClientFactory.CreateGitLabClient(repositorySettings.RepositoryId);
        }

        public Branch[] SearchForOldBranches(TimeSpan olderThan, TimeSpan? notOlderThan = null)
        {
            if (notOlderThan.HasValue && notOlderThan.Value < olderThan)
                throw new ArgumentException($"Parameter notOlderThan ({notOlderThan.Value}) can't be lesser than olderThan parameter ({olderThan})");

            var now = dateTimeFactory.Now;
            var minLastActivityDate = now.Subtract(olderThan);
            var maxLastActivityDate = notOlderThan.HasValue ? now.Subtract(notOlderThan.Value) : DateTime.MinValue;
            return SelectAllBranchesExceptNotTracked()
                    .Where(x => maxLastActivityDate < x.Commit.Committed_date && x.Commit.Committed_date < minLastActivityDate)
                    .ToArray();
        }

        private IEnumerable<Branch> SelectAllBranchesExceptNotTracked()
        {
            return repositoryClient.SelectAllBranches()
                                   .Where(x => !repositorySettings.NotTrackedBrancheNames.Contains(x.Name));
        }
    }
}