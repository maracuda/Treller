using System;
using System.Net;
using System.Text;
using Serialization;

namespace ViskeyTube.Wiki
{
    public class WikiClient : IWikiClient
    {
        private readonly IJsonSerializer jsonSerializer;
        private readonly string authHeader;

        public WikiClient(IJsonSerializer jsonSerializer, string userName, string password)
        {
            this.jsonSerializer = jsonSerializer;
            authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
        }

        public WikiClient(IJsonSerializer jsonSerializer, string authHeader)
        {
            this.jsonSerializer = jsonSerializer;
            this.authHeader = authHeader;
        }

        public string GetPage(string pageId)
        {
            return Execute($"https://wiki.skbkontur.ru/rest/api/content/{pageId}?type=page");
        }

        private T Execute<T>(string url)
        {
            return jsonSerializer.Deserialize<T>("");
        }

        private string Execute(string url)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Authorization", $"Basic {authHeader}");
                return client.DownloadString(url);
            }
        }
    }
}