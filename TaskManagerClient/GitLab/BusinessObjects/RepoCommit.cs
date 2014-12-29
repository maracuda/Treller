using System;

namespace SKBKontur.TaskManagerClient.GitLab.BusinessObjects
{
    public class RepoCommit
    {
        public string Id { get; set; }
        public string ShortId { get; set; }
        public string Title { get; set; }
        public string Author_name { get; set; }
        public string Author_email { get; set; }
        public DateTime Created_at { get; set; }
        public string Message { get; set; }
    }
}