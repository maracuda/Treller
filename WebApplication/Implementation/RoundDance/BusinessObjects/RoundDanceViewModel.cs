using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class RoundDanceViewModel
    {
        public Dictionary<Direction, RoundDancePeopleViewModel[]> DirectionPeoples { get; set; }
        public Dictionary<Direction, List<RoundDancePeopleViewModel[]>> AnotherDirectionPeoples { get; set; }
    }
}