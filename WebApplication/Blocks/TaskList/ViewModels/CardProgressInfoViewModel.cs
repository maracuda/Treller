namespace SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels
{
    public class CardProgressInfoViewModel
    {
        public int CurrentCount { get; set; }
        public int TotalCount { get; set; }

        public decimal Progress { get { return TotalCount > 0 ? CurrentCount * 100M / TotalCount : 0; } }
    }
}