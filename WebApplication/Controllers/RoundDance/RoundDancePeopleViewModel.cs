using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Controllers.RoundDance
{
    public class RoundDancePeopleViewModel
    {
        public RoundDancePeople People { get; set; }
        public Dictionary<Direction, RoundDanceDirectionWeight> DirectionWeights { get; set; }

        public Direction CurrentDirection { get; set; }
        public RoundDanceDirectionWeight CurrentWeight { get { return DirectionWeights[CurrentDirection]; } }
        public DirectionTransferViewModel[] NextTransfers { get; set; }

        public Direction? LastDirection { get; set; }
        public SuggestDirectionViewModel Suggest { get; set; }
    }
}