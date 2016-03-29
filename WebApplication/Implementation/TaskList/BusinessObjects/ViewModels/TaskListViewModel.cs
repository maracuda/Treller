using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.Blocks;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels
{
    public class TaskListViewModel
    {
        public BaseCardListBlock BoardsBlock { get; set; }
        public BaseCardListBlock BugsBlock { get; set; }
        public BaseCardListBlock TaskList { get; set; }
    }
}