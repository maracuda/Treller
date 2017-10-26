using System.IO;
using Serialization;
using Storage;
using TaskManagerClient.CredentialServiceAbstractions;
using TaskManagerClient.Repository.Clients.GitLab;
using TaskManagerClient.Trello.BusinessObjects;
using TaskManagerClient.Youtrack.BusinessObjects;

namespace Tests.Tests.IntegrationTests.Configuration
{
    public class CredentialService : ITrelloUserCredentialService, IGitLabCredentialService, IYouTrackCredentialService, ISpreadsheetsCredentialService
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

        public DomainCredentials MessageBrokerCredentials => Credentials.NotificationCredentials;
        public YouTrackCredential YouTrackCredentials => Credentials.YouTrackCredentials;
        public string ClientSecret => Credentials.GoogleSpreadsheetsCredentials;
    }
}