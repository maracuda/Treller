using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SKBKontur.TaskManagerClient.Abstractions;

namespace SKBKontur.TaskManagerClient.Youtrack
{
    public class YouTrackClient : IBugTrackerClient
    {
        private readonly IHttpRequester httpRequester;
        private const string BugsIssueStartsString = "https://yt.skbkontur.ru/issue";
        private const string BugsSprintStartsString = "https://yt.skbkontur.ru/rest/agile/Billing-169/sprint/";

        public YouTrackClient(IHttpRequester httpRequester)
        {
            this.httpRequester = httpRequester;
        }

        public Issue[] GetFiltered(string filter)
        {
            var parameters = new Dictionary<string, string>
                                 {
                                     {"filter", string.Format("#{{{0}}}", filter)}
                                 };
            var result = httpRequester.SendGetAsync<YouTrackIssues>(BugsIssueStartsString, parameters);
            

            throw new Exception("message");
        }

        public string GetIssueUrl()
        {
            return BugsIssueStartsString + "/";
        }

        public string GetSprintUrl()
        {
            return BugsSprintStartsString;
        }
    }

    [XmlRoot("issueCompacts")]
    public class YouTrackIssues
    {
        [XmlElement("issue")]
        public YouTrackIssue Issues { get; set; }
    }

    public class YouTrackIssue
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("field")]
        public YouTrackIssueField[] Fields { get; set; }
    }

    public class YouTrackIssueField
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("value")]
        public object Value { get; set; }
    }

    public class Issue
    {
        public string Id { get; set; }
        public string ProjectShortName { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdaterName { get; set; }
        public DateTime? Resolved { get; set; }
        public string ReporterName { get; set; }
        public string VoterName { get; set; }
        public int CommentsCount { get; set; }
        public int VotesCount { get; set; }
        public string PermittedGroup { get; set; }
        public string Comment { get; set; }
        public string Tag { get; set; }
    }
}