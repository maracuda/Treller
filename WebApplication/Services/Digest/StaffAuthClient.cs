using System.Text;
using Newtonsoft.Json;
using SKBKontur.HttpInfrastructure.Clients;

namespace SKBKontur.Treller.WebApplication.Services.Digest
{
    public interface IStaffAuthClient
    {
        string Authenticate();
    }

    public class StaffAuthClient : IStaffAuthClient
    {
        private readonly IHttpClient httpClient;
        private long expiredTicks = 0;
        private string token;
        private object locker = new object();

        private const string serviceName = "StaffApiAuth";
        private const string autorizationLogin = "billing";
        private const string autorizationPassword = "OeDon5Zey2bAnxur6EZ1RYxgnxfr499C";
        private const string authorizationBody = "grant_type=client_credentials&scope=profiles";

        public StaffAuthClient(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public string Authenticate()
        {
//            if (CheckTokenExpires())
//            {
//                lock (locker)
//                {
//                    if (CheckTokenExpires())
//                    {
//                        var headers = new Dictionary<string, string>
//                                      {
//                                          {"Content-Type", "application/x-www-form-urlencoded"}
//                                      };
//                        var body = ToUtf8Bytes(authorizationBody);
//                        var auth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(autorizationLogin + ":" + autorizationPassword));
//
//                        httpClient.SendPostAsync<AuthorizationResponse>("https://staff.skbkontur.ru/authz/staff/oauth/token", body, new Dictionary<string, string>{{"Authorization", auth}})
//                        
//
//                        expiredTicks = dateTimeService.UtcTicks + TimeSpan.TicksPerSecond * response.ExpiresIn;
//                        token = response.AccessToken;
//                    }
//                }
//            }
//
//            return token;
            return null;
        }

        private static byte[] ToUtf8Bytes(string value)
        {
            return string.IsNullOrEmpty(value) ? null : Encoding.UTF8.GetBytes(value);
        }
//
//        private bool CheckTokenExpires()
//        {
//            return expiredTicks <= dateTimeService.UtcTicks || string.IsNullOrEmpty(token);
//        }

        private class AuthorizationResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
            [JsonProperty("expires_in")]
            public long ExpiresIn { get; set; }
        }
    }
}