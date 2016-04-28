using System;

namespace SKBKontur.TaskManagerClient.Repository.Clients.BusinessObjects
{
    public class RepoCommit
    {
        public string Id { get; set; }
        public string Short_id { get; set; }
        public string Title { get; set; }
        public string Author_name { get; set; }
        public string Author_email { get; set; }
        public DateTime Created_at { get; set; }
        public string Message { get; set; }

        public bool IsMerge()
        {
            return Title.StartsWith("Merge branch", StringComparison.OrdinalIgnoreCase)
                || Title.StartsWith("Merge remote-tracking branch", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsMerge(string fromBranch, string toBranch = null)
        {
            var convertedTitle = Title.Replace("origin/", "").Replace(" remote-tracking ", " ");
            return string.Equals(convertedTitle, $"Merge branch '{fromBranch}' into {toBranch}", StringComparison.OrdinalIgnoreCase)
                   || (   convertedTitle.StartsWith($"Merge branch '{toBranch}' of ", StringComparison.OrdinalIgnoreCase)
                       && convertedTitle.EndsWith($"into {fromBranch}", StringComparison.OrdinalIgnoreCase));
        }
    }
}