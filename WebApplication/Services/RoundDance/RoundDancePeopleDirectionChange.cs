using System;

namespace SKBKontur.Treller.WebApplication.Services.RoundDance
{
    public class RoundDancePeopleDirectionChange
    {
        public string Name { get; set; }
        public Direction? OldDirection { get; set; }
        public Direction NewDirection { get; set; }
        public DateTime RoundDanceDate { get; set; }
    }
}