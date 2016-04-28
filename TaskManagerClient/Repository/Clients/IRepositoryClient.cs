using SKBKontur.TaskManagerClient.Repository.BusinessObjects;

namespace SKBKontur.TaskManagerClient.Repository.Clients
{
    public interface IRepositoryClient
    {
        Commit[] SelectLastBranchCommits(string branchName, int pageNumber, int pageSize);
        Branch[] SelectAllBranches();
    }
}