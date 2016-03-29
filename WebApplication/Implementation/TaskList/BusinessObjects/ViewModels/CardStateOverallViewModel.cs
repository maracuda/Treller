using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels
{
    public class CardStateOverallViewModel
    {
        public CardState State { get; set; }
        public string Title { get; set; }

        public CardListItemViewModel[] Cards { get; set; }

        public int TotalCardsCount { get { return Cards.Length; } }
        public int NewCardsCount { get { return Cards.Count(x => x.IsNewCard); } }
        public int FinishingCardsCount { get { return Cards.Count(x => x.StageInfo.StageParrots.AverageDaysRemind <= 1 && x.StageInfo.StageParrots.AverageSpeedInDay > 0); } }

    }
}