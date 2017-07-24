using System.Threading.Tasks;
using TaskManagerClient.Repository.BusinessObjects;

namespace TaskManagerClient.Repository.Clients
{
    public interface IRepositoryClient
    {
        Commit[] SelectLastBranchCommits(string branchName, int pageNumber, int pageSize);
        Branch[] SelectAllBranches();
        Task<Branch[]> SelectAllBranchesAsync();
    }
}