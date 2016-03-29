using System;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Blocks
{
    public class CardStateBlock : BaseTaskDetalizationBlock
    {
        public int DueDays { get; set; }
        public CardState State { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}