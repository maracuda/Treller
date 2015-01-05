using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class BugsBuilder : IBugsBuilder
    {
        // TODO: to early, need client integration :(
//        private readonly IBugTrackerClient bugTrackerClient;
        private readonly string issueStartUrl;
        private readonly string sprintStartUrl;

        public BugsBuilder(IBugTrackerClient bugTrackerClient)
        {
            issueStartUrl = bugTrackerClient.GetIssueUrl();
            sprintStartUrl = bugTrackerClient.GetSprintUrl();
        }

        public BugsInfoViewModel Build(IEnumerable<CardChecklist> checklists)
        {
            var bugs = new Dictionary<string, TaskItemBug>();
            foreach (var item in checklists.SelectMany(x => x.Items))
            {
                if (item.Description.StartsWith(issueStartUrl, StringComparison.OrdinalIgnoreCase))
                {
                    var issue = item.Description.Substring(issueStartUrl.Length);
                    bugs[issue] = new TaskItemBug
                                      {
                                          Issue = issue,
                                          IsFixed = item.IsChecked,
                                          Url = item.Description
                                      };
                }
                if (item.Description.StartsWith(sprintStartUrl, StringComparison.OrdinalIgnoreCase))
                {
                    var filter = string.Format("#{{{0}}}", item.Description);
//                    var issues = bugTrackerClient.GetFiltered(filter);
//                    foreach (var issue in issues)
//                    {
//                        bugs[issue.Id] = new TaskItemBug
//                                             {
//                                                 Url = issueStartUrl + issue.Id,
//                                                 IsFixed = issue.Resolved.HasValue,
//                                                 Issue = issue.Id
//                                             };
//                    }
                }
            }

            var itemBugs = bugs.Select(x => x.Value).ToArray();

            return new BugsInfoViewModel
                       {
                           ItemItemBugs = itemBugs,
                           NotFixedBugsCount = itemBugs.Count(x => !x.IsFixed),
                           OverallBugsCount = itemBugs.Length,
                       };
        }       
    }
}