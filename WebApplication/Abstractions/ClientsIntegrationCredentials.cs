using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects;
using SKBKontur.TaskManagerClient.Wiki.BusinessObjects;
using SKBKontur.TaskManagerClient.Youtrack.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Abstractions
{
    public class ClientsIntegrationCredentials
    {
        public TrelloCredential TrelloClientCredentials { get; set; }
        public GitLabCredential GitLabClientCredentials { get; set; }
        public YouTrackCredential YouTrackCredentials { get; set; }
        public WikiCredential WikiCredentials { get; set; }
    }
}