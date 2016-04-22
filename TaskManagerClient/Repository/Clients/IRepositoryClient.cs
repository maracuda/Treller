using System.Threading.Tasks;
using SKBKontur.TaskManagerClient.Repository.Clients.BusinessObjects;

namespace SKBKontur.TaskManagerClient.Repository.Clients
{
    public interface IRepositoryClient
    {
        RepoCommit[] SelectLastBranchCommits(string branchName, int pageNumber, int pageSize);
        RepoBranch[] SelectAllBranches();
        Task<RepoBranch[]> SelectAllBranchesAsync();
    }
}