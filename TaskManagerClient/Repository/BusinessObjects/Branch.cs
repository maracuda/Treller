namespace SKBKontur.TaskManagerClient.Repository.BusinessObjects
{
    public class Branch
    {
        public string Name { get; set; }
        public BranchLastCommit Commit { get; set; }
    }
}