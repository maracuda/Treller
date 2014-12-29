using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class ReleaseCandidateService : IReleaseCandidateService
    {
        private readonly IRepositoryClient repositoryClient;

        public ReleaseCandidateService(IRepositoryClient repositoryClient)
        {
            this.repositoryClient = repositoryClient;
        }

        public SimpleRepoBranch[] WhatBranchesInReleaseCandidate()
        {
            var pageNumber = 0;
            var result = new Dictionary<string, SimpleRepoBranch>(StringComparer.OrdinalIgnoreCase);
            var branches = repositoryClient.SelectAllBranches("584").Select(x => x.Name).ToArray();

            RepoCommit releaseCandidateBranchedCommit = null;
            while (releaseCandidateBranchedCommit == null)
            {
                var repoCommits = repositoryClient.SelectLastBranchCommits("584", "RC", pageNumber++, 100);
                foreach (var repoCommit in repoCommits)
                {
                    if (!IsBranchMergeOperation(repoCommit.Title))
                    {
                        continue;
                    }

                    if (IsBranchMergeOperation(repoCommit.Title, "RC", "release") || IsBranchMergeOperation(repoCommit.Title, "Billy_2.5.13", "release"))
                    {
                        releaseCandidateBranchedCommit = repoCommit;
                    }

                    var mergedBranch = branches.FirstOrDefault(branch => IsBranchMergeOperation(repoCommit.Title, branch, "RC"));
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
            return result.Select(x => x.Value).ToArray();
        }

        private static bool IsBranchMergeOperation(string repoCommitMessage)
        {
            return repoCommitMessage.StartsWith("Merge branch", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsBranchMergeOperation(string repoCommitMessage, string fromBranch, string toBranch = null)
        {
            return string.Equals(repoCommitMessage, string.Format("Merge branch '{0}' into {1}", fromBranch, toBranch), StringComparison.OrdinalIgnoreCase)
                   || (   repoCommitMessage.StartsWith(string.Format("Merge branch '{0}' of ", toBranch), StringComparison.OrdinalIgnoreCase)
                          && repoCommitMessage.EndsWith(string.Format("into {0}", fromBranch), StringComparison.OrdinalIgnoreCase));
        }
    }
}