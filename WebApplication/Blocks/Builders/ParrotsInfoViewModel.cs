using System;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;

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

        public CardProgressInfoViewModel ProgressInfo { get; private set; }
    }
}