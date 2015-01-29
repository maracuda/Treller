using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;
using SKBKontur.TaskManagerClient.Youtrack.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Abstractions
{
    public class ClientsIntegrationCredentials
    {
        public TrelloCredential TrelloClientCredentials { get; set; }
        public GitLabCredential GitLabClientCredentials { get; set; }
        public YouTrackCredential YouTrackCredentials { get; set; }
    }
}