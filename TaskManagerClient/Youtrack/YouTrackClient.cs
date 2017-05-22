using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Web;
using System.Linq;
using System.Xml.Linq;
using SKBKontur.HttpInfrastructure.Clients;
using SKBKontur.TaskManagerClient.BusinessObjects.BugTracker;
using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Youtrack.BusinessObjects;

namespace SKBKontur.TaskManagerClient.Youtrack
{
    public class YouTrackClient : IBugTrackerClient
    {
        private readonly IHttpClient httpClient;
        private readonly string youTrackDefaultUrl;
        private const string BugsIssueStartsString = "issue";
        private const string BugsIssueRestStartsString = "rest/issue";
        private const string BugsSprintStartsString = "rest/agile/";
        private const string UserLoginString = "rest/user/login";
        private Lazy<IEnumerable<Cookie>> authCookies;

        public YouTrackClient(IHttpClient httpClient, IYouTrackCredentialService youTrackCredentialService)
        {
            this.httpClient = httpClient;
            var credential = youTrackCredentialService.GetYouTrackCredentials();
            youTrackDefaultUrl = credential.DefaultUrl;
            authCookies = new Lazy<IEnumerable<Cookie>>(() => GetAuthCookies(credential, httpClient));
        }

        public Issue[] GetFiltered(string filter)
        {
            var parameters = new Dictionary<string, string>
                                 {
                                     {"filter", filter},
                                     {"max", "1000"}
                                 };
            var result = httpClient.SendGetAsync<YouTrackIssues>(BuildUrl(BugsIssueRestStartsString), parameters, authCookies.Value).Result;
            return result.Issue.Select(x =>
                                           {
                                               var lastComment = x.Comment.LastOrDefault();
                                               var created = x.SafeGetDateFromMilleseconds("created");
                                               var updated = x.SafeGetDateFromMilleseconds("updated");
                                               return new Issue
                                                           {
                                                               Id = x.Id,
                                                               CommentsCount = x.Comment.Length,
                                                               LastComment = lastComment != null ? lastComment.Text : null,
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
                var countResult = httpClient.SendGetAsync<EntityCount>(BuildUrl(BugsIssueRestStartsString + "/count"), parameters, authCookies.Value).Result;
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

        public Issue[] GetSprintInfo(string sprintName)
        {
            var sprintFilter = string.Format("Fix versions:{{{0}}}", sprintName);
            return GetFiltered(sprintFilter);
        }

        public string GetIssueUrl()
        {
            return BuildUrl(BugsIssueStartsString) + "/";
        }

        public string GetSprintUrl()
        {
            return BuildUrl(BugsSprintStartsString);
        }

        public string GetStrintUrlEndWord()
        {
            return "/sprint/";
        }

        public string GetBaseUrl()
        {
            return youTrackDefaultUrl;
        }

        public string GetBrowseFilterUrl(string filter)
        {
            return BuildUrl($"issues?q={HttpUtility.UrlEncode(filter)}");
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
            var url = BuildUrl(string.Format("rest/issue/{0}/comment", issueId));
            return httpClient.SendGet<BugTrackerIssueComment[]>(url, null, authCookies.Value);
        }

        public void DeleteAttachment(string issueId, string attachmentId)
        {
            var url = BuildUrl(string.Format("rest/issue/{0}/attachment/{1}", issueId, attachmentId));
            httpClient.SendDelete(url, null, authCookies.Value);
        }

        public void DeleteComment(string issueId, string commentId, bool permanently)
        {
            var url = BuildUrl(string.Format("rest/issue/{0}/comment/{1}", issueId, commentId));
            httpClient.SendDelete(url, new Dictionary<string, string> { { "permanently", permanently.ToString().ToLower() } }, authCookies.Value);
        }

        private Cookie[] GetAuthCookies(YouTrackCredential credential, IHttpClient httpClient)
        {
            var credentials = new Dictionary<string, string>
                                  {
                                      {"login", credential.UserName},
                                      {"password", credential.Password},
                                  };

            return httpClient.SendEncodedFormPostAsync(BuildUrl(UserLoginString), credentials)
                             .Result
                             .OfType<Cookie>()
                             .Where(x => x != null)
                             .ToArray();
        }

        private string BuildUrl(string queryPart)
        {
            return string.Format("{0}/{1}", youTrackDefaultUrl, queryPart);
        }
    }
}