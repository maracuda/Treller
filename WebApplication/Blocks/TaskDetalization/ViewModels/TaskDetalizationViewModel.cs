using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks.Parts;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.ViewModels
{
    public class TaskDetalizationViewModel
    {
        public string CardId { get; set; }
        public BaseTaskDetalizationBlock[] CommonBlocks { get; set; }
        public CardDetalizationPartsBlock Detalization { get; set; }
    }
}