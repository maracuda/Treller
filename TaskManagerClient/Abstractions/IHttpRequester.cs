using System.Collections.Generic;
using System.Threading.Tasks;

namespace SKBKontur.TaskManagerClient.Abstractions
{
    // TODO: Move into infrastructure
    public interface IHttpRequester
    {
        Task<T> SendGetAsync<T>(string url, Dictionary<string, string> queryParameters = null);
    }
}