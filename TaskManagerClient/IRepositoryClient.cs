﻿using System.Threading.Tasks;
using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;

namespace SKBKontur.TaskManagerClient
{
    public interface IRepositoryClient
    {
        RepoCommit[] SelectLastBranchCommits(string branchName, int pageNumber, int pageSize);
        RepoBranch[] SelectAllBranches();
        Task<RepoBranch[]> SelectAllBranchesAsync();
    }
}