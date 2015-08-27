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
            var now = DateTime.Now;

            var currentWorkPeriod = people.WorkPeriods.LastOrDefault(x => x.BeginDate < now && (!x.EndDate.HasValue || x.EndDate > now));
            var currentDirection = currentWorkPeriod.IfNotNull(x => (Direction?)x.Direction) ?? Direction.Leave;
            var nextDirection = people.WorkPeriods.LastOrDefault(x => x.Direction != currentDirection);
            var weights = BuildWeight(people).OrderBy(t => t.Direction).ToDictionary(x => x.Direction);
            var currentWeight = weights[currentDirection];

            SuggestDirectionViewModel suggest = null;
            if (currentWeight.Weight < 5)
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
            if (currentWeight.Weight > 20 && weights.Any())
            {
                var newDirections = weights.Select(x => x.Value).OrderByDescending(x => x.Direction >= currentDirection ? x.Direction - 5 : x.Direction).ToArray();
                var newDirection = newDirections.First(x => x.RotationWeight == weights.Max(w => w.Value.RotationWeight));

                suggest = new SuggestDirectionViewModel
                {
                    IsSuggestDirection = true,
                    OldDirection = currentDirection,
                    SuggestWeight = currentWeight.Weight,
                    NewDirection = newDirection.Direction,
                    Name = people.Name,
                    NewDirections = newDirections
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
                        TransferEndDate = workPeriod.EndDate,
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

            foreach (var period in periods.Where(x => x.BeginDate < DateTime.Now))
            {
                var endDate = period.EndDate ?? now;
                endDate = endDate > now ? now : endDate;
                var daysCount = (endDate - period.BeginDate).Days + 1;
                var daysDiff = 90 - (now - endDate.Date).Days;

                directionWeight += daysCount*daysDiff;
            }

            var maxWeight = direction == Direction.Infrastructure || direction == Direction.Duty ? 12*91 : 90*91;
            
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