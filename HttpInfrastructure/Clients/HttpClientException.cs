using System;
using System.Net.Http;

namespace SKBKontur.HttpInfrastructure.Clients
{
    public class HttpClientException : Exception
    {
        public HttpClientException(HttpResponseMessage response)
            : base(string.Format("StatusCode:{0};Reason:{1};Query:{2};HttpMethod:{3}", response.StatusCode, response.ReasonPhrase, response.RequestMessage.RequestUri, response.RequestMessage.Method))
        {
        }
    }
}