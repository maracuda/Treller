using System;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks
{
    public class CardWorkBlock : BaseTaskDetalizationBlock
    {
        public DateTime BeginDate { get; set; }
        public int DueDays { get; set; }
    }
}