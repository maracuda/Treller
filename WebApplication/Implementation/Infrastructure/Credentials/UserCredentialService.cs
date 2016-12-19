using System;
using System.IO;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Notifications;
using SKBKontur.TaskManagerClient.Repository.Clients.GitLab;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects;
using SKBKontur.TaskManagerClient.Wiki.BusinessObjects;
using SKBKontur.TaskManagerClient.Youtrack.BusinessObjects;
using SKBKontur.Treller.Serialization;
using SKBKontur.Treller.Storage.FileStorage;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Credentials
{
    public class UserCredentialService : ITrelloUserCredentialService, IGitLabCredentialService, IYouTrackCredentialService, IWikiCredentialService, INotificationCredentialsService
    {
        private readonly IFileSystemHandler fileSystemHandler;
        private readonly IJsonSerializer jsonSerializer;
        private readonly Lazy<ClientsIntegrationCredentials> credentials;

        public UserCredentialService(
            IFileSystemHandler fileSystemHandler,
            IJsonSerializer jsonSerializer)
        {
            this.fileSystemHandler = fileSystemHandler;
            this.jsonSerializer = jsonSerializer;
            credentials = new Lazy<ClientsIntegrationCredentials>(LoadCredendials);
        }

        private ClientsIntegrationCredentials LoadCredendials()
        {
            var serializedResult = fileSystemHandler.ReadUTF8("Store_LogIn.json");
            var result = jsonSerializer.Deserialize<ClientsIntegrationCredentials>(serializedResult);
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

        public DomainCredentials GetNotificationCredentials()
        {
            return credentials.Value.NotificationCredentials;
        }
    }
}