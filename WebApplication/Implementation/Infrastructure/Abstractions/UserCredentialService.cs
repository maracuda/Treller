using System;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects;
using SKBKontur.TaskManagerClient.Wiki.BusinessObjects;
using SKBKontur.TaskManagerClient.Youtrack.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Abstractions
{
    public class UserCredentialService : ITrelloUserCredentialService, IGitLabCredentialService, IYouTrackCredentialService, IWikiCredentialService, IAdService
    {
        private static readonly string LogInFilePath = Path.Combine(HttpRuntime.AppDomainAppPath, "LogIn.json");
        private readonly Lazy<ClientsIntegrationCredentials> credentials;

        public UserCredentialService()
        {
            credentials = new Lazy<ClientsIntegrationCredentials>(() => JsonConvert.DeserializeObject<ClientsIntegrationCredentials>(File.ReadAllText(LogInFilePath)));
        }

        public TrelloCredential GetCredentials()
        {
            
            return credentials.Value.TrelloClientCredentials;
        }

        public GitLabCredential GetGitLabCredentials()
        {
            return credentials.Value.GitLabClientCredentials;
        }

        public YouTrackCredential GetYouTrackCredentials()
        {
            return credentials.Value.YouTrackCredentials;
        }

        public WikiCredential GetWikiCredentials()
        {
            return credentials.Value.WikiCredentials;
        }

        public AdCredentials GetDeliveryCredentials()
        {
            return credentials.Value.AdCredentials;
        }
    }
}