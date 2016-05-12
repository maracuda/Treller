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

        public OldBranchesModel Build(TimeSpan oldBranchMinTimeSpan, TimeSpan veryOldBracnhesTimeSpan)
        {
            return new OldBranchesModel
            {
                TotalNumber = repository.BranchesNumber,
                OldBracnhes = repository.SearchForOldBranches(oldBranchMinTimeSpan, veryOldBracnhesTimeSpan),
                VeryOldBracnhes = repository.SearchForOldBranches(veryOldBracnhesTimeSpan)
            };
        }
    }
}