using System.Collections.Generic;
using System.Globalization;
using SKBKontur.TaskManagerClient.Abstractions;
using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;

namespace SKBKontur.TaskManagerClient.GitLab
{
    public class GitLabClient : IRepositoryClient
    {
        private readonly IHttpRequester httpClient;
        private readonly Dictionary<string, string> credentialParameters;

        public GitLabClient(IHttpRequester httpClient, IGitLabCredentialService gitLabCredentialService)
        {
            this.httpClient = httpClient;
            var credentials = gitLabCredentialService.GetGitLabCredentials();
            credentialParameters = new Dictionary<string, string>
                                   {
                                       {"private_token", credentials.PrivateToken},
                                       {"sudo", credentials.UserName}
                                   };
        }

        public RepoCommit[] SelectLastBranchCommits(string repoId, string branchName, int pageNumber, int pageSize)
        {
            var parameters = new Dictionary<string, string>(credentialParameters)
                                 {
                                     {"ref_name", branchName},
                                     {"page", pageNumber.ToString(CultureInfo.InvariantCulture)},
                                     {"per_page", pageSize.ToString(CultureInfo.InvariantCulture)},
                                 };

            return httpClient.SendGetAsync<RepoCommit[]>(string.Format("https://git.skbkontur.ru/api/v3/projects/{0}/repository/commits", repoId), parameters).Result;
        }

        public RepoBranch[] SelectAllBranches(string repoId)
        {
            var parameters = new Dictionary<string, string>(credentialParameters)
                                 {
                                     {"per_page", "1000"},
                                 };

            return httpClient.SendGetAsync<RepoBranch[]>(string.Format("https://git.skbkontur.ru/api/v3/projects/{0}/repository/branches", repoId), parameters).Result;
        }
    }
}