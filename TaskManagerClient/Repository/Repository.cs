using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.Infrastructure.Common;
using SKBKontur.TaskManagerClient.Repository.BusinessObjects;
using SKBKontur.TaskManagerClient.Repository.Clients;

namespace SKBKontur.TaskManagerClient.Repository
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

        public int BranchesNumber => repositoryClient.SelectAllBranches().Length;

        public ReleasedBranch[] SelectBranchesMergedToReleaseCandidate()
        {
            var pageNumber = 0;
            var result = new Dictionary<string, ReleasedBranch>(StringComparer.OrdinalIgnoreCase);
            var branches = repositoryClient.SelectAllBranches().Select(x => x.Name).ToArray();

            Commit releaseCandidateBranchedCommit = null;
            while (releaseCandidateBranchedCommit == null)
            {
                var repoCommits = repositoryClient.SelectLastBranchCommits(repositorySettings.ReleaseCandidateBranchName, pageNumber++, 100);
                foreach (var repoCommit in repoCommits)
                {
                    if (!repoCommit.IsMerge())
                    {
                        continue;
                    }

                    if (repoCommit.IsMerge(repositorySettings.ReleaseCandidateBranchName, repositorySettings.ReleaseBranchName))
                    {
                        releaseCandidateBranchedCommit = repoCommit;
                        break;
                    }

                    var mergedBranch = branches.FirstOrDefault(branch => repoCommit.IsMerge(branch, repositorySettings.ReleaseCandidateBranchName));
                    if (mergedBranch != null && !result.ContainsKey(mergedBranch))
                    {
                        result.Add(mergedBranch, new ReleasedBranch
                                                     {
                                                         Name = mergedBranch,
                                                         LastCommit = repoCommit
                                                     });
                    }
                }
            }
            return result.Select(x => x.Value).Where(x => !repositorySettings.NotTrackedBrancheNames.Contains(x.Name)).ToArray();
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

        public ReleasedBranch[] SearchForMergedToReleaseBranches(TimeSpan notOlderThan)
        {
            var lastCommitDate = dateTimeFactory.Now.Subtract(notOlderThan);
            var branchNames = SelectAllBranchesExceptNotTracked().Select(x => x.Name).ToArray();

            var pageNumber = 0;
            var result = new List<ReleasedBranch>();

            while (true)
            {
                var commits = repositoryClient.SelectLastBranchCommits(repositorySettings.ReleaseBranchName, pageNumber++, 100)
                              .Where(c => c.Created_at > lastCommitDate)
                              .ToArray();
                foreach (var commit in commits)
                {
                    if (!commit.IsMerge())
                        continue;

                    if (commit.IsMerge(repositorySettings.ReleaseBranchName))
                    {
                        var branchName = commit.ParseFromBranchName();
                        if (branchName.HasNoValue)
                        {
                            //TODO: hande this case
                            continue;
                        }

                        if (branchNames.Contains(branchName.Value, StringComparer.OrdinalIgnoreCase))
                        {
                            result.Add(new ReleasedBranch
                            {
                                Name = branchName.Value,
                                LastCommit = commit,
                                IsReleased = true
                            });
                        }
                    }
                }

                if (commits.Length < 100)
                {
                    return result.ToArray();
                }
            }
        }

        private IEnumerable<Branch> SelectAllBranchesExceptNotTracked()
        {
            return repositoryClient.SelectAllBranches()
                                   .Where(x => !repositorySettings.NotTrackedBrancheNames.Contains(x.Name));
        }
    }
}