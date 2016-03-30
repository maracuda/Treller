using System;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class SuggestDirectionViewModel
    {
        public string NewDirection { get; set; }
        public string OldDirection { get; set; }
        public DateTime? SuggestDate { get; set; }

        public bool IsSuggestDirection { get; set; }
        public string Name { get; set; }
    }
}