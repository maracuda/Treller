using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HttpInfrastructure.Clients
{
    public interface IHttpClient
    {
        T SendGet<T>(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null);
        string SendGetAsString(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null);
        Task<T> SendGetAsync<T>(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null);
        Task<CookieCollection> SendEncodedFormPostAsync(string url, Dictionary<string, string> formData);
        void SendDelete(string url, Dictionary<string, string> queryParameters, IEnumerable<Cookie> cookies = null);
    }
}