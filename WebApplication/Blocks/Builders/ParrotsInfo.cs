using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class ParrotsInfo
    {
        public ParrotsInfo()
        {
            ProgressInfo = new CardProgressInfoViewModel();
        }

        public decimal AverageSpeedInDay { get; set; }
        public int AverageDaysRemind { get; set; }
        public int PastDays { get; set; }

        public CardProgressInfoViewModel ProgressInfo { get; private set; }
    }
}