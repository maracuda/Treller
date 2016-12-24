using System;
using System.IO;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Repository.Clients.GitLab;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects;
using SKBKontur.TaskManagerClient.Wiki.BusinessObjects;
using SKBKontur.TaskManagerClient.Youtrack.BusinessObjects;
using SKBKontur.Treller.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Credentials
{
    public class UserCredentialService : ICredentialService
    {
        private readonly IKeyValueStorage keyValueStorage;
        private readonly Lazy<ClientsIntegrationCredentials> credentials;

        public UserCredentialService(IKeyValueStorage keyValueStorage)
        {
            this.keyValueStorage = keyValueStorage;
            credentials = new Lazy<ClientsIntegrationCredentials>(LoadCredendials);
        }

        private ClientsIntegrationCredentials LoadCredendials()
        {
            var result = keyValueStorage.Find<ClientsIntegrationCredentials>("LogIn");
            if (result == null)
                throw new Exception($"Fail to load credentials from file LogIn.json at directory {Directory.GetCurrentDirectory()}");
            return result;
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

        public DomainCredentials MessageBrokerCredentials => credentials.Value.NotificationCredentials;
    }
}