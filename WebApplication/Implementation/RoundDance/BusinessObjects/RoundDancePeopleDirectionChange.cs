using System;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class RoundDancePeopleDirectionChange
    {
        public string Name { get; set; }
        public Direction? OldDirection { get; set; }
        public Direction NewDirection { get; set; }
        public DateTime RoundDanceDate { get; set; }
    }
}