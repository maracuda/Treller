using System.IO;
using System.Web;
using Newtonsoft.Json;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.Abstractions;

namespace SKBKontur.Treller.WebApplication.Abstractions
{
    public class TrelloUserCredentialService : ITrelloUserCredentialService
    {
        private static readonly string LogInFilePath = Path.Combine(HttpRuntime.AppDomainAppPath, "LogIn.json");

        public TrelloCredential GetCredentials()
        {
            return JsonConvert.DeserializeObject<ClientsIntegrationCredentials>(File.ReadAllText(LogInFilePath)).TrelloClientCredentials;
        }
    }
}