﻿namespace RepositoryHooks.BranchNotification
{
    public interface IBranchNotificator
    {
        void NotifyCommitersAboutOldBranches();
    }
}