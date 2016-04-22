using System;
using System.Collections.Generic;
using SKBKontur.TaskManagerClient.Repository.Clients.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Repository
{
    public interface IRepoService
    {
        RepoBranchModel[] SelectBranchesMergedToReleaseCandidate();
        RepoBranch[] SearchForOldBranches(TimeSpan olderThan);
        Dictionary<string, bool> CheckForReleased(RepoBranchModel[] rcBranchesModel);
    }
}