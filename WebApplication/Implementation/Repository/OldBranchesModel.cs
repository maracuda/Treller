using SKBKontur.TaskManagerClient.Repository.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.Repository
{
    public class OldBranchesModel
    {
        public int TotalNumber { get; set; }
        public Branch[] OldBracnhes { get; set; }
        public Branch[] MergedBranches { get; set; }
    }
}