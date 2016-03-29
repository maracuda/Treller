using System;

namespace SKBKontur.Treller.WebApplication.Services.RoundDance
{
    public class SuggestDirectionViewModel
    {
        public Direction NewDirection { get; set; }
        public RoundDanceDirectionWeight[] NewDirections { get; set; }
        public Direction? OldDirection { get; set; }
        public DateTime? SuggestDate { get; set; }

        public bool IsSuggestDirection { get; set; }
        public decimal SuggestWeight { get; set; }
        public string Name { get; set; }
    }
}