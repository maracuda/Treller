using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;

namespace SKBKontur.TaskManagerClient
{
    public interface IRepositoryClient
    {
        RepoCommit[] SelectLastBranchCommits(string repoId, string branchName, int pageNumber, int pageSize);
        RepoBranch[] SelectAllBranches(string repoId);
    }
}