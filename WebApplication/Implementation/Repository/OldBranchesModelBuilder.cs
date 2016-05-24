using System;
using SKBKontur.TaskManagerClient.Repository;

namespace SKBKontur.Treller.WebApplication.Implementation.Repository
{
    public class OldBranchesModelBuilder : IOldBranchesModelBuilder
    {
        private readonly IRepository repository;

        public OldBranchesModelBuilder(IRepository repository)
        {
            this.repository = repository;
        }

        public OldBranchesModel Build(TimeSpan oldBranchMinTimeSpan)
        {
            return new OldBranchesModel
            {
                TotalNumber = repository.BranchesNumber,
                OldBracnhes = repository.SearchForOldBranches(oldBranchMinTimeSpan),
                ReleasedBranches = repository.SearchForMergedToReleaseBranches(oldBranchMinTimeSpan),
                MergedToRC = repository.SelectBranchesMergedToReleaseCandidate()
            };
        }
    }
}