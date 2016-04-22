using SKBKontur.TaskManagerClient.Repository.Clients.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Repository
{
    public class RepoBranchModel
    {
        public string Name { get; set; }
        public bool IsReleased { get; set; }
        public RepoCommit LastCommit { get; set; }
    }
}