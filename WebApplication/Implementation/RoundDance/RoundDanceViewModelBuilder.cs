using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.BlocksMapping.BlockExtenssions;
using SKBKontur.Infrastructure.CommonExtenssions;
using SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects;
using SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance
{
    public class RoundDanceViewModelBuilder : IRoundDanceViewModelBuilder
    {
        private readonly IRoundDancePeopleStorage roundDancePeopleStorage;
        private readonly IBoardsService boardsService;
        private static readonly HashSet<string> NotFeatureTeamDirections = new HashSet<string>(new[] { "Инфраструктура", "Дежурство", "Отпуск", "Болезнь", "Шустрые задачи" });

        public RoundDanceViewModelBuilder(
            IRoundDancePeopleStorage roundDancePeopleStorage,
            IBoardsService boardsService)
        {
            this.roundDancePeopleStorage = roundDancePeopleStorage;
            this.boardsService = boardsService;
        }

        public RoundDanceViewModel Build()
        {
            var peoples = roundDancePeopleStorage.GetAll();

            var byDirections = peoples
                                .GroupBy(x => x.GetCurrentDirection)
                                .ToDictionary(x => x.Key, x => x.Select(BuildPeopleWeights).ToArray());

            var actualDirections = boardsService.SelectKanbanBoards(false)
                                                .Select(x => x.Name)
                                                .Union(NotFeatureTeamDirections)
                                                .Distinct()
                                                .ToArray();


            var oldRounds = peoples.Select(TryBuildOldRoundDance).Where(x => x != null).ToArray();
            var futureRounds = peoples.Select(TryBuildFutureRoundDance).Where(x => x != null).ToArray();

            return new RoundDanceViewModel
            {
                PeoplesByDirections = byDirections,
                AllActualDirections = actualDirections,
                OldRoundDances = oldRounds,
                FutureRoundDances = futureRounds
            };
        }

        public DutyViewModel BuildDuty()
        {
            var peoples = roundDancePeopleStorage.GetAll();
            var dutyPeoples = peoples.Where(x => x.GetCurrentDirection == Direction.Duty.GetEnumDescription())
                                     .Select(x => new PeopleDutyViewModel { Name = x.Name, Email = x.Email })
                                     .ToArray();

            return new DutyViewModel
            {
                Peoples = dutyPeoples
            };
        }

        private static PeopleRoundDanceResultViewModel TryBuildOldRoundDance(RoundDancePeople people)
        {
            var lastChangeIndex = people.WorkPeriods.FindLastIndex(x => x.BeginDate <= DateTime.Now.Date && x.BeginDate.AddDays(4) > DateTime.Now.Date && x.Direction != people.GetCurrentDirection);

            return lastChangeIndex >= 0
                ? CreatePeople(people.Name,
                    (lastChangeIndex > 1 ? people.WorkPeriods[lastChangeIndex - 1] : null)?.Direction ?? string.Empty,
                    people.WorkPeriods[lastChangeIndex])
                : null;
        }

        private static PeopleRoundDanceResultViewModel TryBuildFutureRoundDance(RoundDancePeople people)
        {
            var futureIndex = people.WorkPeriods.FindIndex(x => x.BeginDate > DateTime.Now.Date && x.Direction != people.GetCurrentDirection);

            return futureIndex >= 0
                ? CreatePeople(people.Name, people.GetCurrentDirection, people.WorkPeriods[futureIndex])
                : null;
        }

        private static PeopleRoundDanceResultViewModel CreatePeople(string peopleName, string oldDirection, DirectionPeriod futureDirection)
        {
            return new PeopleRoundDanceResultViewModel
            {
                Name = peopleName,
                OldDirection = oldDirection,

                FutureDirection = futureDirection.Direction,
                RoundDanceDate = futureDirection.BeginDate,
                When = futureDirection.GetPeriodString()
            };
        }

        private static PeopleRoundDanceViewModel BuildPeopleWeights(RoundDancePeople people)
        {
            var weights = people
                .WorkPeriods
                .GroupBy(x => x.Direction)
                .ToDictionary(x => x.Key, x => CalculateWeight(x.ToArray()));

            var futureWeights = weights.Where(x => !NotFeatureTeamDirections.Contains(x.Key)).Select(x => x.Value.Weight).ToArray();

            return new PeopleRoundDanceViewModel
            {
                Name = people.Name,
                CurrentWeight = weights.SafeGet(people.GetCurrentDirection)?.Weight ?? 0,
                DutyWeight = weights.SafeGet(Direction.Duty.GetEnumDescription())?.RotationWeight ?? 100M,
                InfrastructureWeight = weights.SafeGet(Direction.Infrastructure.GetEnumDescription())?.RotationWeight ?? 100M,
                SpeedyWeight = weights.SafeGet(Direction.SpeedyFeatures.GetEnumDescription())?.RotationWeight ?? 100M,
                FeatureWeight = 100 - (futureWeights.Length > 0 ? futureWeights.Max() : 0M)
            };
        }

        private static RoundDanceDirectionWeight CalculateWeight(DirectionPeriod[] periods)
        {
            var weightResult = 0;
            var now = DateTime.Now.Date;
            var direction = periods[0].Direction;
            var isServiceDirection = NotFeatureTeamDirections.Contains(direction);
            var maxDaysInCalculation = isServiceDirection ? 40 : 90;
            var maxDaysInCalculationValue = isServiceDirection ? 14 : 45;
            var lastBeginDate = now.AddDays(-maxDaysInCalculation);
            var maxWeight = maxDaysInCalculation * maxDaysInCalculationValue;

            foreach (var period in periods)
            {
                if (period.BeginDate > now || (period.EndDate.HasValue && period.EndDate.Value.AddDays(maxDaysInCalculation) <= now))
                {
                    continue;
                }

                var endDate = ((period.EndDate.HasValue && period.EndDate.Value > now) || !period.EndDate.HasValue) ? now : period.EndDate.Value;
                var beginDate = period.BeginDate > lastBeginDate ? period.BeginDate : lastBeginDate;

                var daysCount = (endDate - beginDate).TotalDays;
                var koeff = maxDaysInCalculation - (int) (now - endDate).TotalDays;

                weightResult += (int)daysCount*koeff;
                
                if (weightResult > maxWeight)
                {
                    weightResult = maxWeight;
                    break;
                }
            }

            return new RoundDanceDirectionWeight
            {
                Direction = direction,
                Weight = ToPercent(weightResult, maxWeight),
                RotationWeight = ToPercent(maxWeight - weightResult, maxWeight)
            };
        }

        private static decimal ToPercent(int value, int maxValue)
        {
            return decimal.Round((decimal) value * 100 / maxValue, 2);
        }
    }
}