using System;
using System.Collections.Generic;
using TaskManagerClient.Repository.BusinessObjects;

namespace RepositoryHooks.BranchNotification
{
    public class BranchClassificator
    {
        private readonly DateTime branchDeadlineDate;
        private readonly Dictionary<string, HashSet<string>> index;

        private BranchClassificator(DateTime branchDeadlineDate)
        {
            this.branchDeadlineDate = branchDeadlineDate;
            index = new Dictionary<string, HashSet<string>>();
        }

        public static BranchClassificator Create(DateTime branchDeadlineDate, Branch[] branches)
        {
            var result = new BranchClassificator(branchDeadlineDate);
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

            if (branch.Merged)
            {
                index[branch.Commit.Committer_email].Add(branch.Name);
            }
            if (branch.Commit.Committed_date < branchDeadlineDate)
            {
                index[branch.Commit.Committer_email].Add(branch.Name);
            }
        }

        public IEnumerable<string> GetCommitersEmail => index.Keys;

        public IEnumerable<string> GetOldBranchesBy(string commiterEmail)
        {
            return index[commiterEmail];
        }
    }
}