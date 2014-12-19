namespace SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks
{
    public class CardOverallBlock : BaseCardListBlock
    {
        public int TotalCount { get; set; }
        public int DevelopCount { get; set; }
        public int TestingCount { get; set; }
        public int AnaliticsCount { get; set; }
    }
}