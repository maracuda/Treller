using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class RoundDancePeopleViewModel
    {
        public RoundDancePeople People { get; set; }
        public Dictionary<string, RoundDanceDirectionWeight> DirectionWeights { get; set; }

        public decimal FeatureTeamRotationWeight { get; set; }
        public string CurrentDirection { get; set; }
        public string CurrentPairName { get; set; }
        public RoundDanceDirectionWeight CurrentWeight { get { return DirectionWeights[CurrentDirection]; } }
        public DirectionTransferViewModel[] NextTransfers { get; set; }

        public string LastDirection { get; set; }
        public SuggestDirectionViewModel Suggest { get; set; }
    }
}