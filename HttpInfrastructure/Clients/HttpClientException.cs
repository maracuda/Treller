using System;
using System.Linq;
using System.Net.Http;

namespace HttpInfrastructure.Clients
{
    public class HttpClientException : Exception
    {
        private HttpClientException(HttpResponseMessage response, string resultMessage = null)
            : base($"StatusCode:{response.StatusCode};Reason:{response.ReasonPhrase};Query:{response.RequestMessage.RequestUri};HttpMethod:{response.RequestMessage.Method};ResultMessage:{resultMessage};Headers:({string.Join(",", response.RequestMessage.Headers.Select(x => $"{x.Key}:{x.Value}"))})"
            )
        {
        }

        public static HttpClientException Create(HttpResponseMessage response)
        {
            try
            {
                var contentString = response.Content.ReadAsStringAsync().Result;
                return new HttpClientException(response, contentString);
            }
            catch (Exception e)
            {
                return new HttpClientException(response, $"Fail to extract content string. The reason is {e.Message}.");
            }
        }
    }
}