namespace SKBKontur.TaskManagerClient.GitLab.BusinessObjects
{
    public class RepoBranch
    {
        public string Name { get; set; }
        public RepoCommit LastCommit { get; set; }
    }
}