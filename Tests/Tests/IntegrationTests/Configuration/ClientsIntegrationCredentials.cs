using TaskManagerClient.CredentialServiceAbstractions;
using TaskManagerClient.Repository.Clients.GitLab;
using TaskManagerClient.Trello.BusinessObjects;

namespace Tests.Tests.IntegrationTests.Configuration
{
    public class ClientsIntegrationCredentials
    {
        public TrelloCredential TrelloClientCredentials { get; set; }

        public GitLabCredential GitLabClientCredentials { get; set; }

        public DomainCredentials NotificationCredentials { get; set; }
    }
}