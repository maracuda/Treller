using System;

namespace SKBKontur.Treller.RepositoryHooks.BranchNotification
{
    public interface IBranchNotificator
    {
        void NotifyCommitersAboutMergedBranches(TimeSpan maxMergingTimeSpan);
        void NotifyCommitersAboutIdlingBranches(TimeSpan branchIdlingMinTimeSpan);
    }
}