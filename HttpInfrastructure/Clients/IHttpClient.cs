using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SKBKontur.HttpInfrastructure.Clients
{
    public interface IHttpClient
    {
        T SendGet<T>(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null);
        Task<T> SendGetAsync<T>(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null);
        string SendGetAsString(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null);

        void SendPost<T>(string url, T body, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null, string authorizationLogin = null, string authorizationPassword = null);
        T SendPost<T>(string url, Dictionary<string, string> queryParameters = null, HttpContent content = null, IEnumerable<Cookie> cookies = null, string authorizationLogin = null, string authorizationPassword = null);
        Task<CookieCollection> SendEncodedFormPostAsync(string url, Dictionary<string, string> formData);

        void SendDelete(string url, Dictionary<string, string> queryParameters, IEnumerable<Cookie> cookies = null);
    }
}