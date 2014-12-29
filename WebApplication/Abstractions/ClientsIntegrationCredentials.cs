using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Abstractions
{
    public class ClientsIntegrationCredentials
    {
        public TrelloCredential TrelloClientCredentials { get; set; }
        public GitLabCredential GitLabClientCredentials { get; set; }
    }
}