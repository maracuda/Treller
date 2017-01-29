using SKBKontur.TaskManagerClient.Repository.BusinessObjects;

namespace SKBKontur.Treller.RepositoryHooks.BranchNotification
{
    public class OldBranchesModel
    {
        public int TotalNumber { get; set; }
        public Branch[] OldBracnhes { get; set; }
        public ReleasedBranch[] ReleasedBranches { get; set; }
        public ReleasedBranch[] MergedToRC { get; set; }
    }
}