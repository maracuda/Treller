using System;
using System.IO;
using SKBKontur.Infrastructure.Common;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Notifications;
using SKBKontur.TaskManagerClient.Repository.Clients.GitLab;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects;
using SKBKontur.TaskManagerClient.Wiki.BusinessObjects;
using SKBKontur.TaskManagerClient.Youtrack.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Credentials
{
    public class UserCredentialService : ITrelloUserCredentialService, IGitLabCredentialService, IYouTrackCredentialService, IWikiCredentialService, INotificationCredentialsService, IStaffAdCredentialService
    {
        private readonly IFileSystemHandler fileSystemHandler;
        private readonly Lazy<ClientsIntegrationCredentials> credentials;

        public UserCredentialService(IFileSystemHandler fileSystemHandler)
        {
            this.fileSystemHandler = fileSystemHandler;
            credentials = new Lazy<ClientsIntegrationCredentials>(LoadCredendials);
        }

        private ClientsIntegrationCredentials LoadCredendials()
        {
            var result = fileSystemHandler.FindSafeInJsonUtf8File<ClientsIntegrationCredentials>("LogIn.json");
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

        public DomainCredentials GetStaffCredentials()
        {
            return credentials.Value.StaffCredentials;
        }
    }
}