using System;
using SKBKontur.TaskManagerClient.Repository;
using SKBKontur.TaskManagerClient.Repository.BusinessObjects;

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
                MergedBranches = new Branch[0]
            };
        }
    }
}