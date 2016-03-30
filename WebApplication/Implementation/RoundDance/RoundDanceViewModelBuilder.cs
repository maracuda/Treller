using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using SKBKontur.BlocksMapping.BlockExtenssions;
using SKBKontur.Infrastructure.CommonExtenssions;
using SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects;
using SKBKontur.Treller.WebApplication.Implementation.Services.Settings;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance
{
    public class RoundDanceViewModelBuilder : IRoundDanceViewModelBuilder
    {
        private readonly IRoundDancePeopleStorage roundDancePeopleStorage;
        private readonly ISettingService settingService;
        private readonly static HashSet<string> NotFeatureTeamDirections = new HashSet<string>(new[] { "Инфраструктура", "Дежурство", "Отпуск", "Болезнь", "Шустрые задачи" });

        public RoundDanceViewModelBuilder(IRoundDancePeopleStorage roundDancePeopleStorage, ISettingService settingService)
        {
            this.roundDancePeopleStorage = roundDancePeopleStorage;
            this.settingService = settingService;
        }

        public RoundDanceViewModel Build()
        {
            var peoples = roundDancePeopleStorage.GetAll().Select(InnerBuild).ToArray();
            var result = peoples
                            .GroupBy(x => x.CurrentDirection)
                            .ToDictionary(x => x.Key, 
                                          x => x.OrderByDescending(o => o.CurrentWeight.Weight)
                                                .ThenByDescending(a => a.DirectionWeights
                                                                        .SafeGet(Direction.Duty.GetEnumDescription())
                                                                        .IfNotNull(t => t.RotationWeight))
                                                .ToArray());

            return new RoundDanceViewModel { DirectionPeoples = result };
        }

        public RoundDanceViewModel BuildWithLinks()
        {
            var innerResult = new Dictionary<string, List<RoundDancePeopleViewModel[]>>();
            var usedPeopleNames = new HashSet<string>();

            var result = Build();
            foreach (var peoples in result.DirectionPeoples)
            {
                innerResult.Add(peoples.Key, new List<RoundDancePeopleViewModel[]>());

                foreach (var people in peoples.Value)
                {
                    if (!string.IsNullOrWhiteSpace(people.CurrentPairName))
                    {
                        var pair = peoples.Value.FirstOrDefault(x => x.People.Name.Contains(people.CurrentPairName, StringComparison.OrdinalIgnoreCase));
                        if (pair != null)
                        {
                            innerResult[peoples.Key].Add(new []{people, pair});
                            usedPeopleNames.Add(people.People.Name);
                            usedPeopleNames.Add(pair.People.Name);
                        }
                    }
                }

                foreach (var singlePeople in peoples.Value.Where(x => !usedPeopleNames.Contains(x.People.Name)))
                {
                    innerResult[peoples.Key].Add(new []{ singlePeople });
                }
            }

            result.AnotherDirectionPeoples = innerResult;
            result.AllActualDirections = settingService.GetDevelopingBoards().Select(x => x.Name).Union(new[] { Direction.Duty.GetEnumDescription() }).ToArray();

            return result;
        }

        private static RoundDancePeopleViewModel InnerBuild(RoundDancePeople people)
        {
            var nowTime = DateTime.Now;
            var now = nowTime.Date;

            var currentWorkPeriod = people.WorkPeriods.FirstOrDefault(x => nowTime > x.BeginDate && nowTime < x.EndDate) 
                                 ?? people.WorkPeriods.LastOrDefault(x => x.BeginDate <= now || x.EndDate >= now);
            var currentDirection = currentWorkPeriod.IfNotNull(x => x.Direction) ?? Direction.Leave.GetEnumDescription();
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
                    OldDirection = nextDirection != null ? nextDirection.Direction : null,
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
                        OldDirection = lastDirectionPeriod.IfNotNull(x => x.Direction) ?? Direction.Leave.GetEnumDescription()
                    });
                }
                lastDirectionPeriod = workPeriod;
            }

            return new RoundDancePeopleViewModel
            {
                People = people,
                CurrentDirection = currentDirection,
                DirectionWeights = weights,
                FeatureTeamRotationWeight = GetFeatureTeamRotationWeight(people, weights),
                LastDirection = nextDirection != null ? nextDirection.Direction : null,
                Suggest = suggest,
                CurrentPairName = currentWorkPeriod.IfNotNull(x => x.PairName),
                NextTransfers = nextTransfers.ToArray()
            };
        }

        private static decimal GetFeatureTeamRotationWeight(RoundDancePeople people, Dictionary<string, RoundDanceDirectionWeight> weights)
        {
            var directions = people.WorkPeriods.Select(x => x.Direction).Where(x => !NotFeatureTeamDirections.Contains(x));

            var result = 100M;
            foreach (var direction in directions)
            {
                var ft = weights.SafeGet(direction);
                if (ft != null && ft.RotationWeight < result)
                {
                    result = ft.RotationWeight;
                }
            }

            return result;
        }

        private static RoundDanceDirectionWeight[] BuildWeight(RoundDancePeople people)
        {
            var result = people
                .WorkPeriods
                .GroupBy(x => x.Direction)
                .Select(x => BuildDirection(x.Where(d => d.Direction == x.Key).ToArray()))
                .ToArray();

            // Добиваем остальных неправильно!
            return Enum.GetValues(typeof (Direction))
                .Cast<Direction>()
                .Where(x => result.All(r => r.Direction != x.GetEnumDescription()))
                .Select(x => new RoundDanceDirectionWeight
                {
                    Direction = x.GetEnumDescription(),
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
            var isServiceDirection = direction == Direction.Infrastructure.GetEnumDescription() || direction == Direction.Duty.GetEnumDescription();

            foreach (var period in actualPeriods)
            {
                var endDate = period.EndDate > now ? now : period.EndDate;
                var daysCount = ((endDate ?? now) - period.BeginDate).Days + 1;
                var daysDiff = (isServiceDirection ? 40 : 90) - (now - (endDate ?? now)).Days;

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