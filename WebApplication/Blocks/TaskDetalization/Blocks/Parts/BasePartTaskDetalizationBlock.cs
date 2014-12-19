using System;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks.Parts
{
    public abstract class BasePartTaskDetalizationBlock
    {
        public bool IsExists { get; set; }
        public bool IsCurrent { get; set; }
        public string BlockDisclamer { get; set; }
        public string[] PartUsers { get; set; }
        public DateTime? PartBeginDate { get; set; }
        public DateTime? PartEndDate { get; set; }
        public int PartDueDays { get; set; }
    }
}