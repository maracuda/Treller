using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Repository
{
    public interface IOldBranchesModelBuilder
    {
        OldBranchesModel Build(TimeSpan oldBranchMinTimeSpan, TimeSpan veryOldBracnhesTimeSpan);
    }
}