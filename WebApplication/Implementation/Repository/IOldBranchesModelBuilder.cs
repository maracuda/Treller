using System;
using System.Threading.Tasks;

namespace SKBKontur.Treller.WebApplication.Implementation.Repository
{
    public interface IOldBranchesModelBuilder
    {
        OldBranchesModel Build(TimeSpan oldBranchMinTimeSpan, TimeSpan veryOldBracnhesTimeSpan);
        Task<OldBranchesModel> BuildAsync(TimeSpan oldBranchMinTimeSpan, TimeSpan veryOldBracnhesTimeSpan);
    }
}