using SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks;

namespace SKBKontur.Treller.WebApplication.Models.TaskList
{
    public class TaskListViewModel
    {
        public BaseCardListBlock BoardsBlock { get; set; }
        public BaseCardListBlock BugsBlock { get; set; }
        public BaseCardListBlock TaskList { get; set; }
    }
}