using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Repository
{
    public interface IReleaseCandidateService
    {
        SimpleRepoBranch[] WhatBranchesInReleaseCandidate();
        Dictionary<string, bool> CheckForReleased(SimpleRepoBranch[] rcBranches);
    }
}