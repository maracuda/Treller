using SKBKontur.Treller.WebApplication.Implementation.Services.BugTracker;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels
{
    public class BugsInfoViewModel
    {
        public TaskItemBug[] ItemItemBugs { get; set; }
        public int NotFixedBugsCount { get; set; }
        public int OverallBugsCount { get; set; }
        public int FixedBugsCount { get { return OverallBugsCount - NotFixedBugsCount; } }
    }
}