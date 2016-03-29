using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public interface IReleaseCandidateService
    {
        SimpleRepoBranch[] WhatBranchesInReleaseCandidate();
        Dictionary<string, bool> CheckForReleased(SimpleRepoBranch[] rcBranches);
    }
}