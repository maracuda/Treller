using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Repository
{
    public interface IRepoService
    {
        RepoBranchModel[] WhatBranchesInReleaseCandidate();
        Dictionary<string, bool> CheckForReleased(RepoBranchModel[] rcBranchesModel);
    }
}