using System;

namespace SKBKontur.Treller.RepositoryHooks.BranchNotification
{
    public interface IOldBranchesModelBuilder
    {
        OldBranchesModel Build(TimeSpan oldBranchMinTimeSpan);
    }
}