using System;

namespace RepositoryHooks.BranchNotification
{
    public interface IOldBranchesModelBuilder
    {
        OldBranchesModel Build(TimeSpan oldBranchMinTimeSpan);
    }
}