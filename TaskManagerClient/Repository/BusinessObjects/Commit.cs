using System;
using Infrastructure.Sugar;

namespace TaskManagerClient.Repository.BusinessObjects
{
    public class Commit
    {
        public string Id { get; set; }
        public string Short_id { get; set; }
        public string Title { get; set; }
        public string Author_name { get; set; }
        public string Author_email { get; set; }
        public DateTime Created_at { get; set; }
        public string Message { get; set; }

        private const string IntoPattern = "into";
        private const string MergePattern = "Merge branch";

        public bool IsMerge()
        {
            return Title.StartsWith(MergePattern, StringComparison.OrdinalIgnoreCase)
                || Title.StartsWith("Merge remote-tracking branch", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsMerge(string fromBranch, string toBranch)
        {
            var preprocessedTitle = PreprocessTitle();
            return preprocessedTitle.StartsWith($"{MergePattern} '{fromBranch}'", StringComparison.OrdinalIgnoreCase) &&
                   IsMerge(toBranch);
        }

        public bool IsMerge(string toBranch)
        {
            var preprocessedTitle = PreprocessTitle();
            return preprocessedTitle.EndsWith($"{IntoPattern} {toBranch}", StringComparison.OrdinalIgnoreCase) ||
                   preprocessedTitle.EndsWith($"{IntoPattern} '{toBranch}'", StringComparison.OrdinalIgnoreCase);
        }

        public Maybe<string> ParseFromBranchName()
        {
            var preprocessedTitle = PreprocessTitle();
            var indexOfPattern = preprocessedTitle.IndexOf(IntoPattern, StringComparison.OrdinalIgnoreCase);
            if (indexOfPattern == -1)
                return null;
            if (!preprocessedTitle.StartsWith(MergePattern, StringComparison.OrdinalIgnoreCase))
                return null;
            var fromBranchSubstring = preprocessedTitle.Substring(MergePattern.Length, indexOfPattern - MergePattern.Length).Trim();
            if (fromBranchSubstring.StartsWith("'") && fromBranchSubstring.EndsWith("'"))
                return fromBranchSubstring.Substring(1, fromBranchSubstring.Length - 2);
            return fromBranchSubstring;
        }
        private string PreprocessTitle()
        {
            return Title.Replace("origin/", "").Replace(" remote-tracking ", " ");
        }
    }
}