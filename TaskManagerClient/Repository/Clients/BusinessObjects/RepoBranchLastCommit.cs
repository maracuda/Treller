using System;

namespace SKBKontur.TaskManagerClient.Repository.Clients.BusinessObjects
{
    public class RepoBranchLastCommit
    {
        public string Id { get; set; }
        public string Message { get; set; }

        public string Committer_name { get; set; }
        public string Committer_email { get; set; }
        public DateTime Committed_date { get; set; }
    }
}