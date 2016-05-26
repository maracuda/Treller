using System;
using SKBKontur.TaskManagerClient.Repository.BusinessObjects;

namespace SKBKontur.TaskManagerClient.Repository
{
    public interface IRepository
    {
        int BranchesNumber { get; }
        ReleasedBranch[] SelectBranchesMergedToReleaseCandidate();
        Branch[] SearchForOldBranches(TimeSpan olderThan, TimeSpan? notOlderThan = null);
        ReleasedBranch[] SearchForMergedToReleaseBranches(TimeSpan notOlderThan);
    }
}