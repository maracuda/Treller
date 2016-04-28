using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SKBKontur.Infrastructure.CommonExtenssions;
using SKBKontur.TaskManagerClient.Repository.Clients;
using SKBKontur.TaskManagerClient.Repository.Clients.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Repository
{
    public class RepoService : IRepoService
    {
        private readonly IRepositoryClient repositoryClient;
        private readonly IRepoSettings repoSettings;

        public RepoService(
            IRepoSettings repoSettings,
            IRepositoryClientFactory repositoryClientFactory
            )
        {
            this.repoSettings = repoSettings;
            repositoryClient = repositoryClientFactory.CreateGitLabClient(repoSettings.GitLabRepositoryId);
        }

        public RepoBranchModel[] SelectBranchesMergedToReleaseCandidate()
        {
            var pageNumber = 0;
            var result = new Dictionary<string, RepoBranchModel>(StringComparer.OrdinalIgnoreCase);
            var branches = repositoryClient.SelectAllBranches().Select(x => x.Name).ToArray();

            RepoCommit releaseCandidateBranchedCommit = null;
            while (releaseCandidateBranchedCommit == null)
            {
                var repoCommits = repositoryClient.SelectLastBranchCommits(repoSettings.ReleaseCandidateBranchName, pageNumber++, 100);
                foreach (var repoCommit in repoCommits)
                {
                    if (!repoCommit.IsMerge())
                    {
                        continue;
                    }

                    if (repoCommit.IsMerge(repoSettings.ReleaseCandidateBranchName, repoSettings.ReleaseBranchName))
                    {
                        releaseCandidateBranchedCommit = repoCommit;
                        break;
                    }

                    var mergedBranch = branches.FirstOrDefault(branch => repoCommit.IsMerge(branch, repoSettings.ReleaseCandidateBranchName));
                    if (mergedBranch != null && !result.ContainsKey(mergedBranch))
                    {
                        result.Add(mergedBranch, new RepoBranchModel
                                                     {
                                                         Name = mergedBranch,
                                                         LastCommit = repoCommit
                                                     });
                    }
                }
            }
            return result.Select(x => x.Value).Where(x => !repoSettings.NotTrackedBrancheNames.Contains(x.Name)).ToArray();
        }

        public Task<RepoBranchModel[]> SelectBranchesMergedToReleaseCandidateAsync()
        {
            return Task.FromResult(SelectBranchesMergedToReleaseCandidate());
        }

        public RepoBranch[] SearchForOldBranches(TimeSpan olderThan)
        {
            var minLastActivityDate = DateTime.Now.Subtract(olderThan);
            return repositoryClient.SelectAllBranches()
                                   .Where(x => x.Commit.Committed_date < minLastActivityDate)
                                   .ToArray();
        }

        public Dictionary<string, bool> CheckForReleased(RepoBranchModel[] rcBranchesModel)
        {
            var result = rcBranchesModel.DistinctBy(x => x.Name).ToDictionary(x => x.Name, x => false);
            foreach (var branch in rcBranchesModel)
            {
                var repoCommits = repositoryClient.SelectLastBranchCommits(branch.Name, 0, 10);
                if (repoCommits.Where(repoCommit => repoCommit.IsMerge()).Any(repoCommit => repoCommit.IsMerge(branch.Name, repoSettings.ReleaseBranchName)))
                {
                    result[branch.Name] = true;
                }
            }
            return result;
        }
    }
}