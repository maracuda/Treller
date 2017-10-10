using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerClient.Repository.BusinessObjects;

namespace RepositoryHooks.BranchNotification
{
    public class BranchClassificator
    {
        private class BranchClassification
        {
            public BranchClassification()
            {
                OldBranches = new List<Branch>();
                MergedBranches = new List<Branch>();
            }

            public List<Branch> OldBranches { get; }
            public List<Branch> MergedBranches { get; }
        }

        private readonly DateTime branchDeadlineDate;
        private readonly Dictionary<string, BranchClassification> index;

        private BranchClassificator(DateTime branchDeadlineDate)
        {
            this.branchDeadlineDate = branchDeadlineDate;
            index = new Dictionary<string, BranchClassification>();
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
                index.Add(branch.Commit.Committer_email, new BranchClassification());
            }

            if (branch.Merged)
            {
                index[branch.Commit.Committer_email].MergedBranches.Add(branch);
            }
            if (branch.Commit.Committed_date < branchDeadlineDate)
            {
                index[branch.Commit.Committer_email].OldBranches.Add(branch);
            }
        }

        public IEnumerable<string> GetCommitersEmail => index.Keys;

        public IEnumerable<string> GetMergedBranchesBy(string commiterEmail)
        {
            return index[commiterEmail].MergedBranches.Select(b => b.Name);
        }

        public IEnumerable<string> GetOldBranchesBy(string commiterEmail)
        {
            return index[commiterEmail].OldBranches.Select(b => b.Name);
        }
    }
}