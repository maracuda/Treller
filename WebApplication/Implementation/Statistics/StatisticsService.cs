using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using SKBKontur.BlocksMapping.BlockExtenssions;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;
using SKBKontur.Treller.WebApplication.Implementation.Services.Settings;
using SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ICardStateInfoBuilder cardStateInfoBuilder;
        private readonly IKanbanBoardMetaInfoBuilder kanbanBoardMetaInfoBuilder;
        private readonly ICachedFileStorage cachedFileStorage;
        private const string StatisticsFileStoreName = "billingTeamStatisticsInfo";

        public StatisticsService(ITaskManagerClient taskManagerClient, ICardStateInfoBuilder cardStateInfoBuilder, IKanbanBoardMetaInfoBuilder kanbanBoardMetaInfoBuilder, ICachedFileStorage cachedFileStorage)
        {
            this.taskManagerClient = taskManagerClient;
            this.cardStateInfoBuilder = cardStateInfoBuilder;
            this.kanbanBoardMetaInfoBuilder = kanbanBoardMetaInfoBuilder;
            this.cachedFileStorage = cachedFileStorage;
        }

        public StatisticsViewModel GetStatistics(DateTime statisticsStartTime, DateTime statisticsFinishTime, bool reCalculate)
        {
            var result = cachedFileStorage.Find<StatisticsViewModel>(StatisticsFileStoreName);
            if (result != null && !reCalculate)
            {
                return result;
            }

            var boards = kanbanBoardMetaInfoBuilder.GetDevelopingBoardsWithClosed();
            var lists = taskManagerClient.GetBoardLists(boards.Select(x => x.Id).ToArray()).GroupBy(x => x.BoardId).ToDictionary(x => x.Key, x => x.ToArray());
            var boardSettings = boards.ToDictionary(x => x.Id);


            var featureActions = taskManagerClient.GetActionsForBoardCards(boards.Where(x => !x.IsServiceTeamBoard).Select(x => x.Id).ToArray(), statisticsStartTime, statisticsFinishTime);
            var feature = BuildBoardsStatistics(featureActions, boardSettings, lists);

            var serviceActions = taskManagerClient.GetActionsForBoardCards(boards.Where(x => x.IsServiceTeamBoard).Select(x => x.Id).ToArray(), statisticsStartTime, statisticsFinishTime);
            var service = BuildBoardsStatistics(serviceActions, boardSettings, lists);

            var actions = featureActions.Concat(serviceActions).OrderBy(x => x.Date).ToArray();
            var overall = BuildBoardsStatistics(actions, boardSettings, lists);

            result = new StatisticsViewModel
            {
                StatisticsStartTime = statisticsStartTime,
                StatisticsFinishTime = statisticsFinishTime,
                FeatureTeamCardStatistics = feature,
                ServiceTeamCardStatistics = service,
                Overall = overall
            };

            cachedFileStorage.Write(StatisticsFileStoreName, result);
            return result;
        }

        private TeamCardStatisticsModel BuildBoardsStatistics(CardAction[] allActions, Dictionary<string, KanbanBoardMetaInfo> boardSettings, Dictionary<string, BoardList[]> boardLists)
        {
            
            var result = new TeamCardStatisticsModel{MinReleaseTime = new TimeSpan(10, 0, 0, 0)};

            var allResults = new LinkedList<TimeSpan>();
            var states = new Dictionary<CardState, int>();
            foreach (var actionsByCardId in allActions.GroupBy(x => x.CardId))
            {
                var cardStateInfo = cardStateInfoBuilder.Build(actionsByCardId.ToArray(), boardSettings, boardLists);
                var cardStates = cardStateInfo.States;

                if (!states.ContainsKey(cardStateInfo.CurrentState))
                {
                    states.Add(cardStateInfo.CurrentState, 0);
                }
                states[cardStateInfo.CurrentState] += 1;

                if (cardStateInfo.CurrentState < CardState.ReleaseWaiting)
                {
                    continue;
                }
                var releaseTimeSummary = BuildReleaseTime(cardStates);
                if (releaseTimeSummary <= TimeSpan.Zero)
                {
                    continue;
                }

                allResults.AddLast(releaseTimeSummary);
                result.Count++;
                result.AnalyticTimeSummary += cardStates.SafeGet(CardState.Analityc).IfNotNull(x => x.StatePeriod);
                result.AnalyticTimeSummary += cardStates.SafeGet(CardState.AnalitycPresentation).IfNotNull(x => x.StatePeriod);
                result.DevelopTimeSummary += cardStates.SafeGet(CardState.Develop).IfNotNull(x => x.StatePeriod);
                result.DevelopTimeSummary += cardStates.SafeGet(CardState.Review).IfNotNull(x => x.StatePeriod);
                result.DevelopTimeSummary += cardStates.SafeGet(CardState.Presentation).IfNotNull(x => x.StatePeriod);
                result.TestingTimeSummary += cardStates.SafeGet(CardState.Testing).IfNotNull(x => x.StatePeriod);
                result.ReleaseTimeSummary += releaseTimeSummary;

                result.MaxReleaseTime = result.MaxReleaseTime > releaseTimeSummary ? result.MaxReleaseTime : releaseTimeSummary;
                result.MinReleaseTime = result.MinReleaseTime < releaseTimeSummary ? result.MinReleaseTime : releaseTimeSummary;
            }
            var spans = allResults.ToArray();
            Array.Sort(spans);
            result.MedianReleaseTime = spans.Length > 1 ? spans[spans.Length / 2] : TimeSpan.Zero;

            result.AverageReleaseDays = result.ReleaseTimeSummary.TotalDays/result.Count;
            result.AverageDevelopDays = result.DevelopTimeSummary.TotalDays/result.Count;
            result.AverageAnalyticDays =result.AnalyticTimeSummary.TotalDays/result.Count;
            result.AverageTestingDays = result.TestingTimeSummary.TotalDays/result.Count;
            result.PeriodStates = states;
            
            return result;
        }

        private static DateTime BuildCartMoveToStateTime(Dictionary<CardState, CardActionStateInfo> cardStates, CardState fromState)
        {
            foreach (var cardState in Enum.GetValues(typeof(CardState)).Cast<CardState>().Where(x => x >= fromState))
            {
                var start = cardStates.SafeGet(cardState);
                if (start != null)
                {
                    return start.BeginDate;
                }
            }

            return DateTime.Now;
        }

        private static TimeSpan BuildReleaseTime(Dictionary<CardState, CardActionStateInfo> cardStates)
        {
            var beginTime = BuildCartMoveToStateTime(cardStates, CardState.Analityc);
            var endTime = BuildCartMoveToStateTime(cardStates, CardState.ReleaseWaiting);
            return beginTime.CalculatePeriod(endTime);
        }
    }
}