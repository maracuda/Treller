using System;
using System.IO;
using Newtonsoft.Json;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Repository.Clients.GitLab;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Configuration
{
    public class CredentialService : ITrelloUserCredentialService, IGitLabCredentialService
    {
        private static readonly string LogInFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogIn.json");

        public TrelloCredential GetCredentials()
        {
            return JsonConvert.DeserializeObject<ClientsIntegrationCredentials>(File.ReadAllText(LogInFilePath)).TrelloClientCredentials;
        }

        public GitLabCredential GetGitLabCredentials()
        {
            return JsonConvert.DeserializeObject<ClientsIntegrationCredentials>(File.ReadAllText(LogInFilePath)).GitLabClientCredentials;
        }
    }
}