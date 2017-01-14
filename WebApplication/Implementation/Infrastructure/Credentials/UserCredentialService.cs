using System;
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
            var result = keyValueStorage.Read<ClientsIntegrationCredentials>("LogIn");
            if (result == null)
                throw new Exception($"Loaded credentials are empty.");
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