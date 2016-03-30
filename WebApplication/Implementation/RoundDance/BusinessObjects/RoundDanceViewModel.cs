using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class RoundDanceViewModel
    {
        public Dictionary<string, PeopleRoundDanceViewModel[]> PeoplesByDirections { get; set; }
        public PeopleRoundDanceResultViewModel[] OldRoundDances { get; set; }
        public PeopleRoundDanceResultViewModel[] FutureRoundDances { get; set; }
        public string[] AllActualDirections { get; set; }
    }
}