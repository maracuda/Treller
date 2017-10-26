using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using HttpInfrastructure.Clients;
using TaskManagerClient.BusinessObjects.BugTracker;
using TaskManagerClient.CredentialServiceAbstractions;
using TaskManagerClient.Youtrack.BusinessObjects;

namespace TaskManagerClient.Youtrack
{
    public class YouTrackClient : IBugTrackerClient
    {
        private readonly IHttpClient httpClient;
        private readonly string youTrackDefaultUrl;
        private const string bugsIssueRestStartsString = "rest/issue";
        private const string userLoginString = "rest/user/login";
        private readonly Lazy<IEnumerable<Cookie>> authCookies;

        public YouTrackClient(IHttpClient httpClient, IYouTrackCredentialService youTrackCredentialService)
        {
            this.httpClient = httpClient;
            youTrackDefaultUrl = youTrackCredentialService.YouTrackCredentials.DefaultUrl;
            authCookies = new Lazy<IEnumerable<Cookie>>(() => GetAuthCookies(youTrackCredentialService.YouTrackCredentials, httpClient));
        }

        public Issue[] GetFiltered(string filter)
        {
            var parameters = new Dictionary<string, string>
                                 {
                                     {"filter", filter},
                                     {"max", "1000"}
                                 };
            var result = httpClient.SendGetAsync<YouTrackIssues>(BuildUrl(bugsIssueRestStartsString), parameters, authCookies.Value).Result;
            return result.Issue.Select(x =>
                                           {
                                               var lastComment = x.Comment.LastOrDefault();
                                               var created = x.SafeGetDateFromMilleseconds("created");
                                               var updated = x.SafeGetDateFromMilleseconds("updated");
                                               return new Issue
                                                           {
                                                               Id = x.Id,
                                                               CommentsCount = x.Comment.Length,
                                                               LastComment = lastComment?.Text,
                                                               Created = created ?? DateTime.Now,
                                                               Updated = updated ?? DateTime.Now,
                                                               Description = x.SafeGet<string>("description"),
                                                               Summary = x.SafeGet<string>("summary"),
                                                               Resolved = x.SafeGetDateFromMilleseconds("resolved"),
                                                               CreatorLogin = x.SafeGet<string>("reporterName"),
                                                               CreatorFullName = x.SafeGet<string>("reporterFullName")
                                                           };
                                           }).ToArray();
        }

        public int GetFilteredCount(string filter)
        {
            var parameters = new Dictionary<string, string>
                                 {
                                     {"filter", filter}
                                 };
            var timer = Stopwatch.StartNew();
            while (true)
            {
                var countResult = httpClient.SendGetAsync<EntityCount>(BuildUrl(bugsIssueRestStartsString + "/count"), parameters, authCookies.Value).Result;
                if (countResult.Value >= 0)
                {
                    timer.Stop();
                    return countResult.Value;
                }

                if (timer.ElapsedMilliseconds > 2000)
                {
                    timer.Stop();
                    return -1;
                }

                Thread.Sleep(100);
            }
        }

        public BugTrackerIssueAttachment[] GetAttachments(string issueId)
        {
            var url = BuildUrl($"rest/issue/{issueId}/attachment");
            var result = httpClient.SendGetAsString(url, null, authCookies.Value);
            
            var res = XDocument.Parse(result);
            return res.Root.Elements().Select(x => x.Attribute("id").Value).Select(x => new BugTrackerIssueAttachment
            {
                Id = x
            }).ToArray();
        }

        public BugTrackerIssueComment[] GetComments(string issueId)
        {
            var url = BuildUrl($"rest/issue/{issueId}/comment");
            return httpClient.SendGet<BugTrackerIssueComment[]>(url, null, authCookies.Value);
        }

        public void DeleteAttachment(string issueId, string attachmentId)
        {
            var url = BuildUrl($"rest/issue/{issueId}/attachment/{attachmentId}");
            httpClient.SendDelete(url, null, authCookies.Value);
        }

        public void DeleteComment(string issueId, string commentId, bool permanently)
        {
            var url = BuildUrl($"rest/issue/{issueId}/comment/{commentId}");
            httpClient.SendDelete(url, new Dictionary<string, string> { { "permanently", permanently.ToString().ToLower() } }, authCookies.Value);
        }

        private Cookie[] GetAuthCookies(YouTrackCredential credential, IHttpClient httpClient)
        {
            var credentials = new Dictionary<string, string>
                                  {
                                      {"login", credential.UserName},
                                      {"password", credential.Password},
                                  };

            return httpClient.SendEncodedFormPostAsync(BuildUrl(userLoginString), credentials)
                             .Result
                             .OfType<Cookie>()
                             .Where(x => x != null)
                             .ToArray();
        }

        private string BuildUrl(string queryPart)
        {
            return $"{youTrackDefaultUrl}/{queryPart}";
        }
    }
}