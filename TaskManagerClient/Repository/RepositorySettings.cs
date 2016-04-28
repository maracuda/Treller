using System.Collections.Generic;

namespace SKBKontur.TaskManagerClient.Repository
{
    public class RepositorySettings : IRepositorySettings
    {
        public RepositorySettings()
        {
            NotTrackedBrancheNames = new HashSet<string>(new[]
            {
                ReleaseCandidateBranchName, ReleaseBranchName, "hotfixes", "Autotests"
            });
        }

        public string RepositoryId => "584";
        public string ReleaseCandidateBranchName => "RC";
        public string ReleaseBranchName => "release";
        public HashSet<string> NotTrackedBrancheNames { get; }
    }
}