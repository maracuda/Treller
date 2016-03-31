using System;
using SKBKontur.Infrastructure.Common;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects;
using SKBKontur.TaskManagerClient.Wiki.BusinessObjects;
using SKBKontur.TaskManagerClient.Youtrack.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Credentials
{
    public class UserCredentialService : ITrelloUserCredentialService, IGitLabCredentialService, IYouTrackCredentialService, IWikiCredentialService, IAdService, IStaffAdCredentialService
    {
        private readonly Lazy<ClientsIntegrationCredentials> credentials;

        public UserCredentialService(IFileSystemHandler fileSystemHandler)
        {
            credentials = new Lazy<ClientsIntegrationCredentials>(() => fileSystemHandler.FindSafeInJsonUtf8File<ClientsIntegrationCredentials>("LogIn.json"));
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

        public AdCredentials GetStaffCredentials()
        {
            return credentials.Value.StaffAdCredentials;
        }
    }
}