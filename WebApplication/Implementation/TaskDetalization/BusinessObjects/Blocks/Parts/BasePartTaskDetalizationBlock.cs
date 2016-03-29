using System;
using SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Blocks.Parts
{
    public abstract class BasePartTaskDetalizationBlock
    {
        protected BasePartTaskDetalizationBlock()
        {
            CartParrots = new ParrotsInfoViewModel();
        }

        public bool IsExists { get; set; }
        public bool IsCurrent { get; set; }
        public string BlockDisclamer { get; set; }
        
        public DateTime? PartBeginDate { get; set; }
        public DateTime? PartEndDate { get; set; }

        public ParrotsInfoViewModel CartParrots { get; set; }
    }
}