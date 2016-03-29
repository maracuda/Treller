using System.Threading.Tasks;
using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;

namespace SKBKontur.TaskManagerClient
{
    public interface IRepositoryClient
    {
        RepoCommit[] SelectLastBranchCommits(string repoId, string branchName, int pageNumber, int pageSize);
        RepoBranch[] SelectAllBranches(string repoId);

        Task<RepoBranch[]> SelectAllBranchesAsync(string repoId);
    }
}