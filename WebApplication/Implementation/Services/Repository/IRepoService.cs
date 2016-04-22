using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SKBKontur.TaskManagerClient.Repository.Clients.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Repository
{
    public interface IRepoService
    {
        RepoBranchModel[] SelectBranchesMergedToReleaseCandidate();
        Task<RepoBranchModel[]> SelectBranchesMergedToReleaseCandidateAsync();
        RepoBranch[] SearchForOldBranches(TimeSpan olderThan);
        Dictionary<string, bool> CheckForReleased(RepoBranchModel[] rcBranchesModel);
    }
}