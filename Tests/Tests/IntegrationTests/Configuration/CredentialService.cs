using System.IO;
using Serialization;
using Storage;
using TaskManagerClient.CredentialServiceAbstractions;
using TaskManagerClient.Repository.Clients.GitLab;
using TaskManagerClient.Trello.BusinessObjects;

namespace Tests.Tests.IntegrationTests.Configuration
{
    public class CredentialService : ITrelloUserCredentialService, IGitLabCredentialService
    {
        private readonly string logInFilePath;

        public CredentialService(IEnvironment environment)
        {
            logInFilePath = Path.Combine(environment.BasePath, "..\\TrellerData\\LogIn.json");
        }

        private ClientsIntegrationCredentials Credentials => new JsonSerializer().Deserialize<ClientsIntegrationCredentials>(File.ReadAllText(logInFilePath));
        public TrelloCredential GetCredentials()
        {
            return Credentials.TrelloClientCredentials;
        }

        public GitLabCredential GetGitLabCredentials()
        {
            return Credentials.GitLabClientCredentials;
        }
    }
}