using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Repository
{
    public interface IRepoSettings
    {
        string GitLabRepositoryId { get; }
        string ReleaseCandidateBranchName { get; }
        string ReleaseBranchName { get; }
        HashSet<string> NotTrackedBrancheNames { get; }
    }
}