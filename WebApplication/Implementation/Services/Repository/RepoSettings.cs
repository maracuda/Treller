using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Repository
{
    public class RepoSettings : IRepoSettings
    {
        public RepoSettings()
        {
            NotTrackedBrancheNames = new HashSet<string>(new[]
            {
                ReleaseCandidateBranchName, ReleaseBranchName, "hotfixes", "Autotests"
            });
        }

        public string GitLabRepositoryId => "584";
        public string ReleaseCandidateBranchName => "RC";
        public string ReleaseBranchName => "release";
        public HashSet<string> NotTrackedBrancheNames { get; }
    }
}