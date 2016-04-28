namespace SKBKontur.TaskManagerClient.Repository.BusinessObjects
{
    public class ReleasedBranch
    {
        public string Name { get; set; }
        public bool IsReleased { get; set; }
        public Commit LastCommit { get; set; }
    }
}