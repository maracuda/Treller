using System;

namespace RepositoryHooks.BranchNotification
{
    public interface IBranchNotificator
    {
        void NotifyCommitersAboutOldBranches();
        void NotifyCommitersAboutMergedBranches(TimeSpan maxMergingTimeSpan);
        void NotifyCommitersAboutIdlingBranches(TimeSpan branchIdlingMinTimeSpan);
    }
}