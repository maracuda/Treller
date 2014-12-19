using System;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks.Parts
{
    public class CardDevelopPartBlock : BasePartTaskDetalizationBlock
    {
        public int CompleteParrotsCount { get; set; }
        public int ParrotsCount { get; set; }
        public decimal ParrotInDayAverageSpeed { get; set; }
        public DateTime SuggetedFinishDate { get; set; }
    }
}