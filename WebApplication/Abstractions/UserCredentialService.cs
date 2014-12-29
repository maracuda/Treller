using System;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.Abstractions;
using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Abstractions
{
    public class UserCredentialService : ITrelloUserCredentialService, IGitLabCredentialService
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
    }
}