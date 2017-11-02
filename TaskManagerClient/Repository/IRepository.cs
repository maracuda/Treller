using System;
using TaskManagerClient.Repository.BusinessObjects;

namespace TaskManagerClient.Repository
{
    public interface IRepository
    {
        Branch[] SearchForOldBranches(TimeSpan olderThan, TimeSpan? notOlderThan = null);
        void DeleteBranch(string branchName);
    }
}