using System.Linq;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks
{
    public class CardStateOverallViewModel
    {
        public CardState State { get; set; }
        public CardListItemViewModel[] Cards { get; set; }
        public int TotalCardsCount { get { return Cards.Length; } }
        public int NewCardsCount { get { return Cards.Count(x => x.IsNewCard); } }
        public int FinishingCardsCount { get { return Cards.Count(x => x.StageInfo.StageParrots.AverageDaysRemind <= 1 && x.StageInfo.StageParrots.AverageSpeedInDay > 0); } }
    }
}