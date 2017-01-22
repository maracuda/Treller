using System.IO;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Repository.Clients.GitLab;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects;
using SKBKontur.Treller.Serialization;
using SKBKontur.Treller.Storage;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Configuration
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