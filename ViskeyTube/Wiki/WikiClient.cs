using System;
using System.IO;
using System.Net;
using System.Text;
using Serialization;

namespace ViskeyTube.Wiki
{
    public class WikiClient : IWikiClient
    {
        private enum HttpMehtod
        {
            Get = 0,
            Put = 1
        }

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

        private const string ContentUrlPrefix = "https://wiki.skbkontur.ru/rest/api/content/";

        private static string BuildContentUrl(string url)
        {
            return $"{ContentUrlPrefix}{url}";
        }

        private static string BuildExpandedContentUrl(string pageId)
        {
            return BuildContentUrl($"{pageId}?type=page&expand=body.storage,children,body.view,body.styled_view,version,space");
        }

        public WikiPage GetPage(string pageId)
        {
            return Execute<WikiPage>(BuildExpandedContentUrl(pageId));
        }

        public string GetPageSource(string pageId)
        {
            return Execute(BuildExpandedContentUrl(pageId));
        }

        public WikiPageLight[] GetChildren(string pageId)
        {
            return Execute<WikiPageSearchResult>(BuildContentUrl($"search?cql=parent={pageId}")).Results;
        }

        public WikiPage UpdateTitleAndGetNewPage(string pageId, string newTitle)
        {
            var page = Execute<WikiPageLight>(BuildExpandedContentUrl(pageId));
            if (page == null)
                throw new Exception($"Cant find page {pageId}");

            if (page.Title == newTitle)
                return GetPage(pageId);

            page.Title = newTitle;
            page.Version.Number++;

            return Execute<WikiPage>(BuildExpandedContentUrl(pageId), HttpMehtod.Put, jsonSerializer.Serialize(page));
        }

        private T Execute<T>(string url, HttpMehtod httpMehtod = HttpMehtod.Get, string data = null) where T : class
        {
            var result = Execute(url, httpMehtod, data);
            return result != null ? jsonSerializer.Deserialize<T>(result) : null;
        }

        private string Execute(string url, HttpMehtod httpMehtod = HttpMehtod.Get, string data = null)
        {
            try
            {
                var webRequest = WebRequest.Create(url);
                webRequest.Headers.Add("Authorization", $"Basic {authHeader}");
                webRequest.ContentType = "application/json";
                webRequest.Method = httpMehtod.ToString();

                if (!string.IsNullOrWhiteSpace(data))
                {
                    using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        streamWriter.Write(data);
                    }
                }

                var response = webRequest.GetResponse();

                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                if ((e.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }
    }
}