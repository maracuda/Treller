using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Blocks;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Blocks.Parts;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.ViewModels
{
    public class TaskDetalizationViewModel
    {
        public string CardId { get; set; }
        public BaseTaskDetalizationBlock[] CommonBlocks { get; set; }
        public CardDetalizationPartsBlock Detalization { get; set; }
    }
}