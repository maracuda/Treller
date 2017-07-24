using System;

namespace RepositoryHooks.BranchNotification
{
    public interface IBranchNotificator
    {
        void NotifyCommitersAboutMergedBranches(TimeSpan maxMergingTimeSpan);
        void NotifyCommitersAboutIdlingBranches(TimeSpan branchIdlingMinTimeSpan);
    }
}