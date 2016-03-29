using System;
using System.IO;
using Newtonsoft.Json;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects;

namespace SKBKontur.Treller.TestTreller
{
    public class TrelloUserCredentialService : ITrelloUserCredentialService
    {
        private static readonly string LogInFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogIn.json");

        public TrelloCredential GetCredentials()
        {
            return JsonConvert.DeserializeObject<ClientsIntegrationCredentials>(File.ReadAllText(LogInFilePath)).TrelloClientCredentials;
        }
    }
}