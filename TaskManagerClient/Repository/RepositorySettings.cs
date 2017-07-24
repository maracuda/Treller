using System.Collections.Generic;

namespace TaskManagerClient.Repository
{
    public class RepositorySettings : IRepositorySettings
    {
        public RepositorySettings()
        {
            NotTrackedBrancheNames = new HashSet<string>(new[]
            {
                ReleaseCandidateBranchName, ReleaseBranchName, "hotfixes", "Autotests", "autotests/trunk"
            });
        }

        public string RepositoryId => "584";
        public string ReleaseCandidateBranchName => "RC";
        public string ReleaseBranchName => "release";
        public HashSet<string> NotTrackedBrancheNames { get; }
    }
}