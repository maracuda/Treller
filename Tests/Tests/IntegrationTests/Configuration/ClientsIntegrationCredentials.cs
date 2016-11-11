using SKBKontur.TaskManagerClient.Repository.Clients.GitLab;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Configuration
{
    public class ClientsIntegrationCredentials
    {
        public TrelloCredential TrelloClientCredentials { get; set; }

        public GitLabCredential GitLabClientCredentials { get; set; }
    }
}