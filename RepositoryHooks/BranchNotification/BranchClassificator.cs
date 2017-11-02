using System.Collections.Generic;
using TaskManagerClient.Repository.BusinessObjects;

namespace RepositoryHooks.BranchNotification
{
    public class BranchClassificator
    {
        private readonly Dictionary<string, HashSet<string>> index;

        private BranchClassificator()
        {
            index = new Dictionary<string, HashSet<string>>();
        }

        public static BranchClassificator Create(Branch[] branches)
        {
            var result = new BranchClassificator();
            foreach (var branch in branches)
            {
                result.Classify(branch);
            }
            return result;
        }

        private void Classify(Branch branch)
        {
            if (!index.ContainsKey(branch.Commit.Committer_email))
            {
                index.Add(branch.Commit.Committer_email, new HashSet<string>());
            }
            index[branch.Commit.Committer_email].Add(branch.Name);
        }

        public IEnumerable<string> CommiterEmails => index.Keys;

        public IEnumerable<string> GetOldBranchesBy(string commiterEmail)
        {
            return index[commiterEmail];
        }
    }
}