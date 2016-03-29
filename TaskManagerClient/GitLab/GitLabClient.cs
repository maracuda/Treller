using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using SKBKontur.HttpInfrastructure.Clients;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;

namespace SKBKontur.TaskManagerClient.GitLab
{
    public class GitLabClient : IRepositoryClient
    {
        private readonly IHttpClient httpClient;
        private readonly Dictionary<string, string> credentialParameters;
        private readonly string gitLabDefaultUrl;

        public GitLabClient(IHttpClient httpClient, IGitLabCredentialService gitLabCredentialService)
        {
            this.httpClient = httpClient;
            var credentials = gitLabCredentialService.GetGitLabCredentials();
            gitLabDefaultUrl = credentials.DefaultUrl;
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

            return httpClient.SendGetAsync<RepoCommit[]>(string.Format("{0}/api/v3/projects/{1}/repository/commits", gitLabDefaultUrl, repoId), parameters).Result;
        }

        public RepoBranch[] SelectAllBranches(string repoId)
        {
            var parameters = new Dictionary<string, string>(credentialParameters)
                                 {
                                     {"per_page", "1000"},
                                 };

            return httpClient.SendGetAsync<RepoBranch[]>(string.Format("{0}/api/v3/projects/{1}/repository/branches", gitLabDefaultUrl, repoId), parameters).Result;
        }

        public Task<RepoBranch[]> SelectAllBranchesAsync(string repoId)
        {
            var parameters = new Dictionary<string, string>(credentialParameters)
                                 {
                                     {"per_page", "1000"},
                                 };

            return httpClient.SendGetAsync<RepoBranch[]>(string.Format("{0}/api/v3/projects/{1}/repository/branches", gitLabDefaultUrl, repoId), parameters);
        }
    }
}