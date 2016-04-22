using SKBKontur.HttpInfrastructure.Clients;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.GitLab;

namespace SKBKontur.TaskManagerClient
{
    public class RepositoryClientFactory : IRepositoryClientFactory
    {
        private readonly IHttpClient httpClient;
        private readonly IGitLabCredentialService gitLabCredentialService;

        public RepositoryClientFactory(
            IHttpClient httpClient,
            IGitLabCredentialService gitLabCredentialService)
        {
            this.httpClient = httpClient;
            this.gitLabCredentialService = gitLabCredentialService;
        }

        public IRepositoryClient CreateGitLabClient(string repoId)
        {
            return new GitLabClient(repoId, httpClient, gitLabCredentialService);
        }
    }
}