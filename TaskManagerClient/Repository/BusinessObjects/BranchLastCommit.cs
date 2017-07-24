using System;

namespace TaskManagerClient.Repository.BusinessObjects
{
    public class BranchLastCommit
    {
        public string Id { get; set; }
        public string Message { get; set; }

        public string Committer_name { get; set; }
        public string Committer_email { get; set; }
        public DateTime Committed_date { get; set; }
    }
}