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
            using (var client = CreateHttpClient(CreateCookieContainer(cookies)))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = client.GetAsync(GetFullUrl(url, queryParameters), HttpCompletionOption.ResponseContentRead).Result)
                {
                    ValidateResponse(response);
                    return response.Content.ReadAsAsync<T>().Result;
                }
            }
        }

        public async Task<T> SendGetAsync<T>(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null)
        {
            using (var client = CreateHttpClient(CreateCookieContainer(cookies)))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await client.GetAsync(GetFullUrl(url, queryParameters), HttpCompletionOption.ResponseContentRead))
                {
                    ValidateResponse(response);
                    return await response.Content.ReadAsAsync<T>();
                }
            }
        }

        public string SendGetAsString(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null)
        {
            using (var client = CreateHttpClient(CreateCookieContainer(cookies)))
            {
                using (var response = client.GetAsync(GetFullUrl(url, queryParameters), HttpCompletionOption.ResponseContentRead).Result)
                {
                    ValidateResponse(response);
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }

        public void SendPost<T>(string url, T body, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null, string authorizationLogin = null, string authorizationPassword = null)
        {
            using (var client = CreateHttpClient(CreateCookieContainer(cookies)))
            {
                UpdateHttpBasicCredentials(client, authorizationLogin, authorizationPassword);

                using (var response = client.PostAsJsonAsync(GetFullUrl(url, queryParameters), body).Result)
                {
                    ValidateResponse(response);
                }
            }
        }

        public T SendPost<T>(string url, Dictionary<string, string> queryParameters = null, HttpContent content = null, IEnumerable<Cookie> cookies = null, string authorizationLogin = null, string authorizationPassword = null)
        {
            using (var client = CreateHttpClient(CreateCookieContainer(cookies)))
            {
                UpdateHttpBasicCredentials(client, authorizationLogin, authorizationPassword);

                using (var response = client.PostAsync(GetFullUrl(url, queryParameters), content).Result)
                {
                    ValidateResponse(response);
                    return response.Content.ReadAsAsync<T>().Result;
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
                    ValidateResponse(response);
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
                    ValidateResponse(response);
                }
            }
        }

        private static void UpdateHttpBasicCredentials(System.Net.Http.HttpClient client, string authorizationLogin, string authorizationPassword)
        {
            if (!string.IsNullOrWhiteSpace(authorizationLogin) && !string.IsNullOrWhiteSpace(authorizationPassword))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorizationLogin, authorizationPassword);
            }
        }

        private static void ValidateResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            string additionalErrorMessage;
            try
            {    
                additionalErrorMessage = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {
                additionalErrorMessage = null;
            }

            throw new HttpClientException(response, additionalErrorMessage);
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
                stringBuilder.Append(string.Format("{0}={1}", parameter.Key, HttpUtility.UrlEncode(parameter.Value)));
            }

            stringBuilder.Remove(url.Length + 1, 1);
            return stringBuilder.ToString();
        }
    }
}