using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SKBKontur.TaskManagerClient.Repository.BusinessObjects;

namespace SKBKontur.TaskManagerClient.Repository
{
    public interface IRepository
    {
        ReleasedBranch[] SelectBranchesMergedToReleaseCandidate();
        Task<ReleasedBranch[]> SelectBranchesMergedToReleaseCandidateAsync();
        Branch[] SearchForOldBranches(TimeSpan olderThan);
        Dictionary<string, bool> CheckForReleased(ReleasedBranch[] rcBranches);
    }
}