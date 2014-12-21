using System.Collections.Generic;
using System.Threading.Tasks;
using SKBKontur.HttpInfrastructure.Clients;
using SKBKontur.TaskManagerClient.Abstractions;

namespace SKBKontur.Treller.WebApplication.Services.Abstractions
{
    public class HttpRequester : IHttpRequester
    {
        private readonly IHttpClient httpClient;

        public HttpRequester(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<T> SendGetAsync<T>(string url, Dictionary<string, string> queryParameters = null)
        {
            return httpClient.SendGetAsync<T>(url, queryParameters);
        }
    }
}