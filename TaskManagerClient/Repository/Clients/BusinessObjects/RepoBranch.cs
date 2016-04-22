namespace SKBKontur.TaskManagerClient.Repository.Clients.BusinessObjects
{
    public class RepoBranch
    {
        public string Name { get; set; }
        public RepoCommit LastCommit { get; set; }
    }
}