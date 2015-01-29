using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SKBKontur.HttpInfrastructure.Clients
{
    // TODO: make fluent!
    public interface IHttpClient
    {
        Task<T> SendGetAsync<T>(string url, Dictionary<string, string> queryParameters = null, CookieContainer cookies = null);

        Task SendPostAsync<T>(string url, T body, Dictionary<string, string> queryParameters = null);
        Task SendPostAsync(string url, Dictionary<string, string> queryParameters = null);
        Task<CookieCollection> SendEncodedFormPostAsync(string url, Dictionary<string, string> formData);

        Task<TResult> SendPostAsync<T, TResult>(string url, T body, Dictionary<string, string> queryParameters = null);
        Task<TResult> SendPostAsync<TResult>(string url, Dictionary<string, string> queryParameters = null);
    }
}