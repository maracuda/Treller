namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class BugsInfoViewModel
    {
        public TaskItemBug[] ItemItemBugs { get; set; }
        public int NotFixedBugsCount { get; set; }
        public int OverallBugsCount { get; set; }
        public int FixedBugsCount { get { return OverallBugsCount - NotFixedBugsCount; } }
    }
}