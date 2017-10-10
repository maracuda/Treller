namespace TaskManagerClient.Repository.BusinessObjects
{
    public class Branch
    {
        public string Name { get; set; }
        public bool Merged { get; set; }
        public BranchLastCommit Commit { get; set; }
    }
}