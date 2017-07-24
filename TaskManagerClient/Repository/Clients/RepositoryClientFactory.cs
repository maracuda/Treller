using HttpInfrastructure.Clients;
using TaskManagerClient.Repository.Clients.GitLab;

namespace TaskManagerClient.Repository.Clients
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