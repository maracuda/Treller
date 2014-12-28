using SKBKontur.Treller.WebApplication.Blocks.Builders;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels
{
    public class CardStageInfoViewModel
    {
        public CardStageInfoViewModel()
        {
            StageParrots = new ParrotsInfoViewModel();
        }

        public CardState State { get; set; }
        public ParrotsInfoViewModel StageParrots { get; set; }
    }
}