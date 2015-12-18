using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SKBKontur.TaskManagerClient.Abstractions
{
    // TODO: Move into infrastructure
    public interface IHttpRequester
    {
        Task<T> SendGetAsync<T>(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null);
        T SendGet<T>(string url, Dictionary<string, string> queryParameters = null, IEnumerable<Cookie> cookies = null);
        Task<IEnumerable<Cookie>> SendPostEncodedAsync(string url, Dictionary<string, string> formUrlEncodedContent);
    }
}