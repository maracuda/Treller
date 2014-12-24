using SKBKontur.Treller.WebApplication.Blocks.Builders;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels
{
    public class CardStageInfoViewModel
    {
        public CardStageInfoViewModel()
        {
            Parrots = new ParrotsInfo();
        }

        public CardState State { get; set; }
        public int DaysInState { get; set; }
        public ParrotsInfo Parrots { get; set; }
        public int CurrentCardParrots { get; set; }
        public int TotalCardParrots { get; set; }
    }
}