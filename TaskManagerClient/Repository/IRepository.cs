using System;
using TaskManagerClient.Repository.BusinessObjects;

namespace TaskManagerClient.Repository
{
    public interface IRepository
    {
        int BranchesNumber { get; }
        ReleasedBranch[] SelectBranchesMergedToReleaseCandidate();
        Branch[] SearchForOldBranches(TimeSpan olderThan, TimeSpan? notOlderThan = null);
        ReleasedBranch[] SearchForMergedToReleaseBranches(TimeSpan notOlderThan);
    }
}