using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Blocks.Parts
{
    public class CardReviewPartBlock : BasePartTaskDetalizationBlock
    {
        public ToDoItemsListViewModel[] ReviewToDoListsViewModel { get; set; }
    }
}