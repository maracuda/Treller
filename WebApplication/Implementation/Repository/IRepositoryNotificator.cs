using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Repository
{
    public interface IRepositoryNotificator
    {
        void NotifyCommitersAboutMergedBranches(TimeSpan maxMergingTimeSpan);
        void NotifyCommitersAboutIdlingBranches(TimeSpan branchIdlingMinTimeSpan);
    }
}