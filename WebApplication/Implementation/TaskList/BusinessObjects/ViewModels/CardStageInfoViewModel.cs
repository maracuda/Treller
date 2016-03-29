using SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels
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