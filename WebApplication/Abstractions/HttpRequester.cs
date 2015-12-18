using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SKBKontur.HttpInfrastructure.Clients;
using SKBKontur.TaskManagerClient.Abstractions;
using System.Linq;

namespace SKBKontur.Treller.WebApplication.Abstractions
{
    public class HttpRequester : IHttpRequester
    {
        private readonly IHttpClient httpClient;

        public HttpRequester(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<T> SendGetAsync<T>(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null)
        {
            CookieContainer cookieContainer = null;
            if (cookies != null)
            {
                cookieContainer = new CookieContainer();
                foreach (var cookie in cookies)
                {
                    cookieContainer.Add(cookie);
                }
            }
            return httpClient.SendGetAsync<T>(url, queryParameters, cookieContainer);
        }

        public T SendGet<T>(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null)
        {
            CookieContainer cookieContainer = null;
            if (cookies != null)
            {
                cookieContainer = new CookieContainer();
                foreach (var cookie in cookies)
                {
                    cookieContainer.Add(cookie);
                }
            }
            return httpClient.SendGet<T>(url, queryParameters, cookieContainer);
        }

        public async Task<IEnumerable<Cookie>> SendPostEncodedAsync(string url, Dictionary<string, string> formUrlEncodedContent = null)
        {
            var cookies = await httpClient.SendEncodedFormPostAsync(url, formUrlEncodedContent);
            return cookies.OfType<Cookie>().Where(x => x != null);
        }
    }
}