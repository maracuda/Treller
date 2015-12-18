using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;

namespace SKBKontur.Treller.WebApplication.Controllers.RoundDance
{
    public class RoundDanceViewModelBuilder : IRoundDanceViewModelBuilder
    {
        private readonly IRoundDancePeopleStorage roundDancePeopleStorage;

        public RoundDanceViewModelBuilder(IRoundDancePeopleStorage roundDancePeopleStorage)
        {
            this.roundDancePeopleStorage = roundDancePeopleStorage;
        }

        public RoundDanceViewModel Build()
        {
            var peoples = roundDancePeopleStorage.GetAll().Select(InnerBuild).ToArray();
            var result = peoples.GroupBy(x => x.CurrentDirection).ToDictionary(x => x.Key, x => x.OrderByDescending(o => o.CurrentWeight.Weight).ToArray());

            return new RoundDanceViewModel
            {
                DirectionPeoples = result,
                NearestRoundDances = new RoundDancePeopleViewModel[0],
                LastChanges = new RoundDancePeopleDirectionChange[0],
                NearestChanges = new RoundDancePeopleDirectionChange[0]
            };
        }

        private static RoundDancePeopleViewModel InnerBuild(RoundDancePeople people)
        {
            var nowTime = DateTime.Now;
            var now = nowTime.Date;

            var currentWorkPeriod = people.WorkPeriods.FirstOrDefault(x => nowTime > x.BeginDate && nowTime < x.EndDate) 
                                 ?? people.WorkPeriods.LastOrDefault(x => x.BeginDate <= now || x.EndDate >= now);
            var currentDirection = currentWorkPeriod.IfNotNull(x => (Direction?)x.Direction) ?? Direction.Leave;
            var nextDirection = people.WorkPeriods.LastOrDefault(x => x.Direction != currentDirection);
            var weights = BuildWeight(people).OrderBy(t => t.Direction).ToDictionary(x => x.Direction);
            var currentWeight = weights[currentDirection];

            SuggestDirectionViewModel suggest = null;
            if (currentWeight.Weight < 5 || (currentWorkPeriod != null && (now - currentWorkPeriod.BeginDate).Days <= 4))
            {
                suggest = new SuggestDirectionViewModel
                {
                    IsSuggestDirection = false,
                    NewDirection = currentDirection,
                    OldDirection = nextDirection != null ? nextDirection.Direction : (Direction?)null,
                    SuggestDate = currentWorkPeriod.IfNotNull(x => (DateTime?)x.BeginDate),
                    Name = people.Name
                };
            }

            DirectionPeriod lastDirectionPeriod = null;
            var nextTransfers = new List<DirectionTransferViewModel>();
            foreach (var workPeriod in people.WorkPeriods)
            {
                if (workPeriod.BeginDate > now)
                {
                    nextTransfers.Add(new DirectionTransferViewModel
                    {
                        Name = people.Name,
                        NewDirection = workPeriod.Direction,
                        TransferDate = workPeriod.BeginDate,
                        TransferEndDate = workPeriod.EndDate == now ? (DateTime?) null : workPeriod.EndDate,
                        OldDirection = lastDirectionPeriod.IfNotNull(x => (Direction?)x.Direction) ?? Direction.Leave
                    });
                }
                lastDirectionPeriod = workPeriod;
            }

            return new RoundDancePeopleViewModel
            {
                People = people,
                CurrentDirection = currentDirection,
                DirectionWeights = weights,
                LastDirection = nextDirection != null ? nextDirection.Direction : (Direction?)null,
                Suggest = suggest,
                NextTransfers = nextTransfers.ToArray()
            };
        }

        private static RoundDanceDirectionWeight[] BuildWeight(RoundDancePeople people)
        {
            var result = people
                .WorkPeriods
                .GroupBy(x => x.Direction)
                .Select(x => BuildDirection(x.Where(d => d.Direction == x.Key).ToArray()))
                .ToArray();

            return Enum.GetValues(typeof (Direction))
                .Cast<Direction>()
                .Where(x => result.All(r => r.Direction != x))
                .Select(x => new RoundDanceDirectionWeight
                {
                    Direction = x,
                    Weight = 0,
                    RotationWeight = 100
                }).Union(result).ToArray();

        }

        private static RoundDanceDirectionWeight BuildDirection(DirectionPeriod[] periods)
        {
            var directionWeight = 0;
            var now = DateTime.Now.Date;
            var direction = periods.First().Direction;

            var actualPeriods = periods.Where(x => x.BeginDate <= DateTime.Now.Date);
            var isServiceDirection = direction == Direction.Infrastructure || direction == Direction.Duty;

            foreach (var period in actualPeriods)
            {
                var endDate = period.EndDate > now ? now : period.EndDate;
                var daysCount = (endDate - period.BeginDate).Days + 1;
                var daysDiff = (isServiceDirection ? 40 : 90) - (now - endDate).Days;

                if (daysDiff > 0)
                {
                    directionWeight += daysCount*daysDiff;
                }
            }

            
            var maxWeight = isServiceDirection ? 14*41 : 90*91;
            
            return new RoundDanceDirectionWeight
            {
                Direction = direction,
                Weight = ToPercent(directionWeight, maxWeight),
                RotationWeight = maxWeight < directionWeight ? 0 : ToPercent(maxWeight - directionWeight, maxWeight)
            };
        }

        private static decimal ToPercent(int value, int maxValue)
        {
            return decimal.Round((decimal) value * 100 / maxValue, 2);
        }
    }
}