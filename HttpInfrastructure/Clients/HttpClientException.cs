using System;
using System.Linq;
using System.Net.Http;

namespace SKBKontur.HttpInfrastructure.Clients
{
    public class HttpClientException : Exception
    {
        public HttpClientException(HttpResponseMessage response, string resultMessage = null)
            : base(string.Format("StatusCode:{0};Reason:{1};Query:{2};HttpMethod:{3};ResultMessage:{4};Headers:({5})", response.StatusCode, response.ReasonPhrase, response.RequestMessage.RequestUri, response.RequestMessage.Method, resultMessage, string.Join(",", response.RequestMessage.Headers.Select(x => string.Format("{0}:{1}", x.Key, x.Value)))))
        {
        }
    }
}