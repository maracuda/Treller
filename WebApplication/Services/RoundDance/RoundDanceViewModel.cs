using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Services.RoundDance
{
    public class RoundDanceViewModel
    {
        public Dictionary<Direction, RoundDancePeopleViewModel[]> DirectionPeoples { get; set; }
        public Dictionary<Direction, List<RoundDancePeopleViewModel[]>> AnotherDirectionPeoples { get; set; }
        public RoundDancePeopleDirectionChange[] LastChanges { get; set; }
        public RoundDancePeopleDirectionChange[] NearestChanges { get; set; }

        public RoundDancePeopleViewModel[] NearestRoundDances { get; set; }
    }
}