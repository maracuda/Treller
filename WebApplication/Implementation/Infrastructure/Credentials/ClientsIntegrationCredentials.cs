using TaskManagerClient.CredentialServiceAbstractions;
using TaskManagerClient.Repository.Clients.GitLab;
using TaskManagerClient.Trello.BusinessObjects;
using TaskManagerClient.Wiki.BusinessObjects;
using TaskManagerClient.Youtrack.BusinessObjects;

namespace WebApplication.Implementation.Infrastructure.Credentials
{
    public class ClientsIntegrationCredentials
    {
        public TrelloCredential TrelloClientCredentials { get; set; }
        public GitLabCredential GitLabClientCredentials { get; set; }
        public YouTrackCredential YouTrackCredentials { get; set; }
        public WikiCredential WikiCredentials { get; set; }
        public DomainCredentials NotificationCredentials { get; set; }
    }
}