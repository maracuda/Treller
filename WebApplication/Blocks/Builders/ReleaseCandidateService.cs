using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.Infrastructure.CommonExtenssions;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class ReleaseCandidateService : IReleaseCandidateService
    {
        private readonly IRepositoryClient repositoryClient;
        private const string GitLabRepositoryId = "584";
        private const string ReleaseCandidateBranchName = "RC";
        private const string ReleaseBranchName = "release";
        private static HashSet<string> noTrackedBrancheNames = 
            new HashSet<string>(new[]
            {
                ReleaseCandidateBranchName, ReleaseBranchName, "hotfixes", "Autotests"
            });


        public ReleaseCandidateService(IRepositoryClient repositoryClient)
        {
            this.repositoryClient = repositoryClient;
        }

        public SimpleRepoBranch[] WhatBranchesInReleaseCandidate()
        {
            var pageNumber = 0;
            var result = new Dictionary<string, SimpleRepoBranch>(StringComparer.OrdinalIgnoreCase);
            // todo : Remove hardcode
            var branches = repositoryClient.SelectAllBranches(GitLabRepositoryId).Select(x => x.Name).ToArray();

            RepoCommit releaseCandidateBranchedCommit = null;
            while (releaseCandidateBranchedCommit == null)
            {
                var repoCommits = repositoryClient.SelectLastBranchCommits(GitLabRepositoryId, ReleaseCandidateBranchName, pageNumber++, 100);
                foreach (var repoCommit in repoCommits)
                {
                    if (!IsBranchMergeOperation(repoCommit.Title))
                    {
                        continue;
                    }

                    if (IsBranchMergeOperation(repoCommit.Title, ReleaseCandidateBranchName, ReleaseBranchName))
                    {
                        releaseCandidateBranchedCommit = repoCommit;
                        break;
                    }

                    var mergedBranch = branches.FirstOrDefault(branch => IsBranchMergeOperation(repoCommit.Title, branch, ReleaseCandidateBranchName));
                    if (mergedBranch != null && !result.ContainsKey(mergedBranch))
                    {
                        result.Add(mergedBranch, new SimpleRepoBranch
                                                     {
                                                         Name = mergedBranch,
                                                         LastCommitTime = repoCommit.Created_at
                                                     });
                    }
                }
            }
            return result.Select(x => x.Value).Where(x => !noTrackedBrancheNames.Contains(x.Name)).ToArray();
        }

        public Dictionary<string, bool> CheckForReleased(SimpleRepoBranch[] rcBranches)
        {
            var result = rcBranches.DistinctBy(x => x.Name).ToDictionary(x => x.Name, x => false);
            foreach (var branch in rcBranches)
            {
                var repoCommits = repositoryClient.SelectLastBranchCommits(GitLabRepositoryId, branch.Name, 0, 10);
                if (repoCommits.Where(repoCommit => IsBranchMergeOperation(repoCommit.Title)).Any(repoCommit => IsBranchMergeOperation(repoCommit.Title, branch.Name, ReleaseBranchName)))
                {
                    result[branch.Name] = true;
                }
            }

            return result;
        }

        private static bool IsBranchMergeOperation(string repoCommitMessage)
        {
            return repoCommitMessage.StartsWith("Merge branch", StringComparison.OrdinalIgnoreCase)
                   || repoCommitMessage.StartsWith("Merge remote-tracking branch", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsBranchMergeOperation(string repoCommitMessage, string fromBranch, string toBranch = null)
        {
            repoCommitMessage = repoCommitMessage.Replace("origin/", "").Replace(" remote-tracking ", " ");
            return string.Equals(repoCommitMessage, string.Format("Merge branch '{0}' into {1}", fromBranch, toBranch), StringComparison.OrdinalIgnoreCase)
                   || (   repoCommitMessage.StartsWith(string.Format("Merge branch '{0}' of ", toBranch), StringComparison.OrdinalIgnoreCase)
                          && repoCommitMessage.EndsWith(string.Format("into {0}", fromBranch), StringComparison.OrdinalIgnoreCase));
        }
    }
}