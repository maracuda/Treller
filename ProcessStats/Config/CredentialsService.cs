using System;
using System.IO;
using Serialization;
using TaskManagerClient.CredentialServiceAbstractions;
using TaskManagerClient.Trello.BusinessObjects;

namespace ProcessStats.Config
{
    public class CredentialService : ITrelloUserCredentialService
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
    }

    public class Credentials
    {
        public TrelloCredential TrelloClientCredentials { get; set; }
        public DomainCredentials NotificationCredentials { get; set; }
    }
}