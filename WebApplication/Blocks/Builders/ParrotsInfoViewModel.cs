using System;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Extensions;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class ParrotsInfoViewModel
    {
        public ParrotsInfoViewModel()
        {
            ProgressInfo = new CardProgressInfoViewModel();
        }

        public decimal AverageSpeedInDay { get; set; }
        public int AverageDaysRemind { get; set; }
        public int PastDays { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string BeginDateFormat {get { return BeginDate.SafeDateFormat(); }}
        public string EndDateFormat { get { return BeginDate.SafeDateFormat(); } }

        public CardProgressInfoViewModel ProgressInfo { get; private set; }
    }
}