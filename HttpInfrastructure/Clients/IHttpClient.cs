using System.Collections.Generic;
using System.Threading.Tasks;

namespace SKBKontur.HttpInfrastructure.Clients
{
    public interface IHttpClient
    {
        Task SendGetAsync(string url, Dictionary<string, string> queryParameters = null);
        Task<T> SendGetAsync<T>(string url, Dictionary<string, string> queryParameters = null);
        Task SendPostAsync<TSerialized>(string url, TSerialized body, Dictionary<string, string> queryParameters = null);
        Task SendPostAsync(string url, Dictionary<string, string> queryParameters = null);
        Task<TResult> SendPostAsync<TSerialized, TResult>(string url, TSerialized body, Dictionary<string, string> queryParameters = null);
        Task<TResult> SendPostAsync<TResult>(string url, Dictionary<string, string> queryParameters = null);
    }
}