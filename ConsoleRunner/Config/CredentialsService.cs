using System;
using System.IO;
using Serialization;
using TaskManagerClient.CredentialServiceAbstractions;
using TaskManagerClient.Repository.Clients.GitLab;
using TaskManagerClient.Trello.BusinessObjects;
using TaskManagerClient.Youtrack.BusinessObjects;

namespace ConsoleRunner.Config
{
    public class CredentialService : ITrelloUserCredentialService, IGitLabCredentialService, IYouTrackCredentialService, ISpreadsheetsCredentialService
    {
        private readonly string logInFilePath;

        public CredentialService()
        {
            logInFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\TrellerData\\LogIn.json");
        }

        private Credentials Credentials => new JsonSerializer().Deserialize<Credentials>(File.ReadAllText(logInFilePath));
        public TrelloCredential GetCredentials()
        {
            return Credentials.TrelloClientCredentials;
        }

        public DomainCredentials MessageBrokerCredentials => Credentials.NotificationCredentials;
        public GitLabCredential GetGitLabCredentials()
        {
            return Credentials.GitLabClientCredentials;
        }
        public YouTrackCredential YouTrackCredentials => Credentials.YouTrackCredentials;
        public string ClientSecret => Credentials.GoogleSpreadsheetsCredentials;
    }

    public class Credentials
    {
        public TrelloCredential TrelloClientCredentials { get; set; }
        public GitLabCredential GitLabClientCredentials { get; set; }
        public YouTrackCredential YouTrackCredentials { get; set; }
        public DomainCredentials NotificationCredentials { get; set; }
        public string GoogleSpreadsheetsCredentials { get; set; }
    }
}