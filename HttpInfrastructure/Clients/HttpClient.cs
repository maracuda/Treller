using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SKBKontur.HttpInfrastructure.Clients
{
    public class HttpClient : IHttpClient
    {
        public async Task SendGetAsync(string url, Dictionary<string, string> queryParameters = null)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var requestUri = GetFullUrl(url, queryParameters);
                using (var response = await client.GetAsync(requestUri))
                {
                    ValidateResponse(response);
                }
            }
        }

        private static void ValidateResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpClientException(response);
            }
        }

        public async Task<T> SendGetAsync<T>(string url, Dictionary<string, string> queryParameters)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var requestUri = GetFullUrl(url, queryParameters);
                using (var response = await client.GetAsync(requestUri))
                {
                    ValidateResponse(response);
                    return await response.Content.ReadAsAsync<T>();
                }
            }
        }

        public async Task SendPostAsync<TSerialized>(string url, TSerialized body, Dictionary<string, string> queryParameters = null)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var requestUri = GetFullUrl(url, queryParameters);
                var response = await client.PostAsJsonAsync(requestUri, body);
                if (!response.IsSuccessStatusCode)
                {
                    ValidateResponse(response);
                }
            }
        }

        public async Task SendPostAsync(string url, Dictionary<string, string> queryParameters = null)
        {
            await SendPostAsync(url, (string)null, queryParameters);
        }

        public async Task<TResult> SendPostAsync<TSerialized, TResult>(string url, TSerialized body, Dictionary<string, string> queryParameters = null)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var requestUri = GetFullUrl(url, queryParameters);
                var response = await client.PostAsJsonAsync(requestUri, body);
                if (!response.IsSuccessStatusCode)
                {
                    ValidateResponse(response);
                }
                return await response.Content.ReadAsAsync<TResult>();
            }
        }

        public async Task<TResult> SendPostAsync<TResult>(string url, Dictionary<string, string> queryParameters = null)
        {
            return await SendPostAsync<string, TResult>(url, null, queryParameters);
        }

        private static string GetFullUrl(string url, Dictionary<string, string> queryParameters)
        {
            if (queryParameters == null || queryParameters.Count == 0)
            {
                return url;
            }

            var stringBuilder = new StringBuilder(url, 2048);
            stringBuilder.Append("?");
            foreach (var parameter in queryParameters)
            {
                stringBuilder.Append("&");
                stringBuilder.Append(string.Format("{0}={1}", parameter.Key, HttpUtility.UrlEncode(parameter.Value)));
            }

            stringBuilder.Remove(url.Length + 1, 1);
            return stringBuilder.ToString();
        }
    }
}