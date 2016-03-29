using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.BlocksMapping.BlockExtenssions;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class RoundDancePeopleViewModel
    {
        public RoundDancePeople People { get; set; }
        public Dictionary<Direction, RoundDanceDirectionWeight> DirectionWeights { get; set; }

        public decimal GetFeatureTeamRotationWeight()
        {
            var featureTeamDirections = Enum.GetValues(typeof (Direction)).Cast<Direction>().Where(x => x > Direction.SpeedyFeatures).ToArray();

            var result = 100M;
            foreach (var direction in featureTeamDirections)
            {
                var ft = DirectionWeights.SafeGet(direction);
                if (ft != null && ft.RotationWeight < result)
                {
                    result = ft.RotationWeight;
                }
            }

            return result;
        }

        public Direction CurrentDirection { get; set; }
        public string CurrentPairName { get; set; }
        public RoundDanceDirectionWeight CurrentWeight { get { return DirectionWeights[CurrentDirection]; } }
        public DirectionTransferViewModel[] NextTransfers { get; set; }

        public Direction? LastDirection { get; set; }
        public SuggestDirectionViewModel Suggest { get; set; }
    }
}