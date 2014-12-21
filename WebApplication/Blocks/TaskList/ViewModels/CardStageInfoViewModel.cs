using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels
{
    public class CardStageInfoViewModel
    {
        public CardState State { get; set; }
        public int DaysInState { get; set; }
        public int CurrentStateParrots { get; set; }
        public int TotalStateParrots { get; set; }
        public int CurrentCardParrots { get; set; }
        public int TotalCardParrots { get; set; }
    }
}