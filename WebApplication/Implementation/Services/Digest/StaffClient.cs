using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using SKBKontur.HttpInfrastructure.Clients;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Abstractions;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Credentials;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Digest
{
    public class StaffClient : ISocialNetworkClient
    {
        private readonly IHttpClient httpClient;
        private readonly IAdService adService;
        private static StaffAuthoricationInfo _authoricationInfo;

        public StaffClient(IHttpClient httpClient, IAdService adService)
        {
            this.httpClient = httpClient;
            this.adService = adService;
        }

        public void Feed(string message)
        {
            var authData = GetAuthorizationData();
            httpClient.SendPost("https://staff.skbkontur.ru/api/feed/group_3423", new StaffFeedModel(message), null, null, authData.Login, authData.Password);
        }

        public class StaffFeedModel
        {
            public StaffFeedModel(string message)
            {
                Entry = new StaffEntry { Message = message, Type = "post", Items = new string[0] };
                Options = new StaffOptions { PostAsOwner = true };
            }

            public StaffEntry Entry { get; set; }
            public StaffOptions Options { get; set; }
        }

        public class StaffEntry
        {
            [JsonProperty("$type")]
            public string Type { get; set; }
            public string Message { get; set; }
            public string[] Items { get; set; }
        }

        public class StaffOptions
        {
            public bool PostAsOwner { get; set; }
        }

        private StaffAuthoricationInfo GetAuthorizationData()
        {
            if (_authoricationInfo == null || !_authoricationInfo.IsActive)
            {
                var credentials = adService.GetDeliveryCredentials();
                var form = new FormUrlEncodedContent(new Dictionary<string,string>
                {
                    {"grant_type", "password"},
                    {"scope","profiles"},
                    {"username", credentials.DomainLogin},
                    {"password", credentials.Password},
                });

                var authorizationData = httpClient.SendPost<StaffAuthorizationData>("https://passport.skbkontur.ru/authz/staff/oauth/token", null, form, null, "Basic YmlsbHlfdHJlbGxlcjpOa0VTdU1PRm05aDdJdEZHZVJtcHV5RGpPbkY1MFFuZWgxRktsQ3RTbEnigIs=");
                _authoricationInfo = new StaffAuthoricationInfo(authorizationData);
            }

            return _authoricationInfo;
        }

        public class StaffAuthorizationData
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

        public class StaffAuthoricationInfo
        {
            private readonly DateTime estimateTime;

            public StaffAuthoricationInfo(StaffAuthorizationData data)
            {
                Data = data;
                estimateTime = DateTime.Now.AddSeconds(data.expires_in);
            }

            private StaffAuthorizationData Data { get; set; }
            public bool IsActive { get { return DateTime.Now > estimateTime; } }
            public string Login { get { return Data.token_type; } }
            public string Password { get { return Data.access_token; } }
        }
    }
}