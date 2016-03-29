namespace SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.ViewModels
{
    public class ToDoItemsListViewModel
    {
        public string ListName { get; set; }
        public string LastIncompletedDescription { get; set; }
        public int CompleteCount { get; set; }
        public int Count { get; set; }
    }
}