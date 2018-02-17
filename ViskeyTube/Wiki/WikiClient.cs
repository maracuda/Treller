﻿using System;
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

        public WikiPage GetPage(string pageId)
        {
            return Execute<WikiPage>($"https://wiki.skbkontur.ru/rest/api/content/{pageId}?type=page&expand=body.storage");
        }

        private T Execute<T>(string url)
        {
            return jsonSerializer.Deserialize<T>(Execute(url));
        }

        private string Execute(string url)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Authorization", $"Basic {authHeader}");
                return ConvertFrom1251(client.DownloadString(url));
            }
        }

        private static string ConvertFrom1251(string src)
        {
            var utf8 = Encoding.UTF8;
            var win1251 = Encoding.GetEncoding("Windows-1251");

            var utf8Bytes = win1251.GetBytes(src);
            var win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);

            return win1251.GetString(win1251Bytes);
        }
    }
}