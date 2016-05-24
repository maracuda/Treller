using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Infrastructure.CommonExtenssions;
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

        public Task<ReleasedBranch[]> SelectBranchesMergedToReleaseCandidateAsync()
        {
            return Task.FromResult(SelectBranchesMergedToReleaseCandidate());
        }

        public Branch[] SearchForOldBranches(TimeSpan olderThan, TimeSpan? notOlderThan = null)
        {
            if (notOlderThan.HasValue && notOlderThan.Value < olderThan)
                throw new ArgumentException($"Parameter notOlderThan ({notOlderThan.Value}) can't be lesser than olderThan parameter ({olderThan})");

            var now = dateTimeFactory.Now;
            var minLastActivityDate = now.Subtract(olderThan);
            var maxLastActivityDate = notOlderThan.HasValue ? now.Subtract(notOlderThan.Value) : DateTime.MinValue;
            return repositoryClient.SelectAllBranches()
                                   .Where(x => maxLastActivityDate < x.Commit.Committed_date && x.Commit.Committed_date < minLastActivityDate)
                                   .ToArray();
        }

        public ReleasedBranch[] SearchForMergedToReleaseBranches(TimeSpan notOlderThan)
        {
            var lastCommitDate = dateTimeFactory.Now.Subtract(notOlderThan);
            var branchNames = repositoryClient.SelectAllBranches().Select(x => x.Name).ToArray();

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

        public Dictionary<string, bool> CheckForReleased(ReleasedBranch[] rcBranches)
        {
            var result = rcBranches.DistinctBy(x => x.Name).ToDictionary(x => x.Name, x => false);
            foreach (var branch in rcBranches)
            {
                var repoCommits = repositoryClient.SelectLastBranchCommits(branch.Name, 0, 10);
                if (repoCommits.Where(repoCommit => repoCommit.IsMerge()).Any(repoCommit => repoCommit.IsMerge(branch.Name, repositorySettings.ReleaseBranchName)))
                {
                    result[branch.Name] = true;
                }
            }
            return result;
        }
    }
}