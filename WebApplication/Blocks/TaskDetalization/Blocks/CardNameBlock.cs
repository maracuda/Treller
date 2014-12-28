namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks
{
    public class CardNameBlock : BaseTaskDetalizationBlock
    {
        public string OriginalName { get; set; }
        public string ApplicationName { get; set; }
        public string CardUrl { get; set; }

        public string ControlVersionSystemBranchName { get; set; }
    }
}