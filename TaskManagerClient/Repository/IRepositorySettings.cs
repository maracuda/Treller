using System.Collections.Generic;

namespace TaskManagerClient.Repository
{
    public interface IRepositorySettings
    {
        string RepositoryId { get; }
        string ReleaseCandidateBranchName { get; }
        string ReleaseBranchName { get; }
        HashSet<string> NotTrackedBrancheNames { get; }
    }
}