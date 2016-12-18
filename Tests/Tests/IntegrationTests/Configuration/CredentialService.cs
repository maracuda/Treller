using System;
using System.IO;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Repository.Clients.GitLab;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects;
using SKBKontur.Treller.Serialization;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Configuration
{
    public class CredentialService : ITrelloUserCredentialService, IGitLabCredentialService
    {
        private static readonly string LogInFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogIn.json");
        private static ClientsIntegrationCredentials Credentials => new JsonSerializer().Deserialize<ClientsIntegrationCredentials>(File.ReadAllText(LogInFilePath));
        public TrelloCredential GetCredentials()
        {
            return Credentials.TrelloClientCredentials;
        }

        public GitLabCredential GetGitLabCredentials()
        {
            return Credentials.GitLabClientCredentials;
        }
    }
}