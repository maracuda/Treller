using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class ChecklistParrotsBuilder : IChecklistParrotsBuilder
    {
        public ParrotsInfo Build(IEnumerable<CardChecklist> checklists, int daysCount)
        {
            if (checklists == null || daysCount == 0)
            {
                return new ParrotsInfo();
            }

            var result = new ParrotsInfo();

            foreach (var listItem in checklists.SelectMany(x => x.Items))
            {
                var completeCount = 1;
                var isMatch = Regex.Match(listItem.Description, @"\(\d+/\d+\)$", RegexOptions.IgnoreCase);
                if (isMatch.Success)
                {
                    var matchResult = isMatch.Value.Trim('(', ')').Split('/');
                    result.TotalCount += int.Parse(matchResult[1]);
                    completeCount = int.Parse(matchResult[0]);
                }
                else
                {
                    result.TotalCount += 1;
                }

                if (listItem.IsChecked)
                {
                    result.CurrentCount += completeCount;
                }
            }

            result.AverageSpeedInDay = (decimal)result.CurrentCount/daysCount;
            result.AverageDaysRemind = (int) (result.AverageSpeedInDay > 0 ? (result.TotalCount - result.CurrentCount) / result.AverageSpeedInDay : 0);

            return result;
        }
    }
}