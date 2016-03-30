using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class RoundDanceViewModel
    {
        public Dictionary<string, RoundDancePeopleViewModel[]> DirectionPeoples { get; set; }
        public Dictionary<string, List<RoundDancePeopleViewModel[]>> AnotherDirectionPeoples { get; set; }
        public string[] AllActualDirections { get; set; }
    }
}