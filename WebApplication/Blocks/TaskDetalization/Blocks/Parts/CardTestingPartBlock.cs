using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.ViewModels;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks.Parts
{
    public class CardTestingPartBlock : BasePartTaskDetalizationBlock
    {
        public ToDoItemsListViewModel[] TestingToDoListsViewModel { get; set; }
        public int OverallBugsCount { get; set; }
        public int FixedBugsCount { get; set; }
    }
}