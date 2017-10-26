using System;
using Storage;
using TaskManagerClient.CredentialServiceAbstractions;
using TaskManagerClient.Repository.Clients.GitLab;
using TaskManagerClient.Trello.BusinessObjects;
using TaskManagerClient.Wiki.BusinessObjects;
using TaskManagerClient.Youtrack.BusinessObjects;

namespace WebApplication.Implementation.Infrastructure.Credentials
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

        public WikiCredential GetWikiCredentials()
        {
            return credentials.Value.WikiCredentials;
        }

        public DomainCredentials MessageBrokerCredentials => credentials.Value.NotificationCredentials;
        public YouTrackCredential YouTrackCredentials => credentials.Value.YouTrackCredentials;
    }
}