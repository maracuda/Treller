using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class ChecklistParrotsBuilder : IChecklistParrotsBuilder
    {
        public ParrotsInfoViewModel Build(IEnumerable<CardChecklist> checklists, int daysCount)
        {
            if (checklists == null || daysCount == 0)
            {
                return new ParrotsInfoViewModel();
            }

            var result = new ParrotsInfoViewModel();

            foreach (var listItem in checklists.SelectMany(x => x.Items))
            {
                var completeCount = 1;
                var isMatch = Regex.Match(listItem.Description, @"\(\d+/\d+\)$", RegexOptions.IgnoreCase);
                if (isMatch.Success)
                {
                    var matchResult = isMatch.Value.Trim('(', ')').Split('/');
                    result.ProgressInfo.TotalCount += int.Parse(matchResult[1]);
                    completeCount = int.Parse(matchResult[0]);
                }
                else
                {
                    result.ProgressInfo.TotalCount += 1;
                }

                if (listItem.IsChecked)
                {
                    result.ProgressInfo.CurrentCount += completeCount;
                }
            }

            result.AverageSpeedInDay = (decimal)result.ProgressInfo.CurrentCount / daysCount;
            result.AverageDaysRemind = (int)(result.AverageSpeedInDay > 0 ? (result.ProgressInfo.TotalCount - result.ProgressInfo.CurrentCount) / result.AverageSpeedInDay : 0);
            result.PastDays = daysCount;

            return result;
        }
    }
}