using System;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks
{
    public class CardStateBlock : BaseTaskDetalizationBlock
    {
        public int DueDays { get; set; }
        public CardState State { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}