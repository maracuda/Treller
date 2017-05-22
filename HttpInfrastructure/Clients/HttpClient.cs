using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SKBKontur.HttpInfrastructure.Clients
{
    public class HttpClient : IHttpClient
    {
        public T SendGet<T>(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null)
        {
            return AsyncHelpers.RunSync(() => SendGetAsync<T>(url, queryParameters, cookies));
        }
        public string SendGetAsString(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null)
        {
            return AsyncHelpers.RunSync(() => SendGetStringAsync(url, queryParameters, cookies));
        }

        public async Task<T> SendGetAsync<T>(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null)
        {
            using (var client = CreateHttpClient(CreateCookieContainer(cookies)))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await client.GetAsync(GetFullUrl(url, queryParameters), HttpCompletionOption.ResponseContentRead))
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return default(T);
                    if (!response.IsSuccessStatusCode)
                        throw HttpClientException.Create(response);
                    return await response.Content.ReadAsAsync<T>().ConfigureAwait(false);
                }
            }
        }

        private async Task<string> SendGetStringAsync(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null)
        {
            using (var client = CreateHttpClient(CreateCookieContainer(cookies)))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await client.GetAsync(GetFullUrl(url, queryParameters), HttpCompletionOption.ResponseContentRead))
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return default(string);
                    if (!response.IsSuccessStatusCode)
                        throw HttpClientException.Create(response);
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task<CookieCollection> SendEncodedFormPostAsync(string url, Dictionary<string, string> formData)
        {
            var cookies = new CookieContainer();
            using (var client = CreateHttpClient(cookies))
            {
                using (var response = await client.PostAsync(url, new FormUrlEncodedContent(formData)))
                {
                    if (!response.IsSuccessStatusCode)
                        throw HttpClientException.Create(response);
                    return cookies.GetCookies(new Uri(url));
                }
            }
        }

        public void SendDelete(string url, Dictionary<string, string> queryParameters, IEnumerable<Cookie> cookies = null)
        {
            using (var client = CreateHttpClient(CreateCookieContainer(cookies)))
            {
                using (var response = client.DeleteAsync(GetFullUrl(url, queryParameters)).Result)
                {
                    if (!response.IsSuccessStatusCode)
                        throw HttpClientException.Create(response);
                }
            }
        }

        private static CookieContainer CreateCookieContainer(IEnumerable<Cookie> cookies)
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
            return cookieContainer;
        }

        private static System.Net.Http.HttpClient CreateHttpClient(CookieContainer cookieContainer = null, ICredentials credentials = null)
        {
            var handler = new HttpClientHandler();
            
            if (cookieContainer != null)
            {
                handler.CookieContainer = cookieContainer;
                handler.UseCookies = true;
            }

            if (credentials != null)
            {
                handler.Credentials = credentials;
            }

            return new System.Net.Http.HttpClient(handler, true);
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
                stringBuilder.Append($"{parameter.Key}={HttpUtility.UrlEncode(parameter.Value)}");
            }

            stringBuilder.Remove(url.Length + 1, 1);
            return stringBuilder.ToString();
        }
    }
}