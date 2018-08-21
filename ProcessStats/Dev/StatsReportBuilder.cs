using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerClient;

namespace ProcessStats.Dev
{
    public class StatsReportBuilder : IStatsReportBuilder
    {
        private const string organizationName = "konturbilling";
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ICardsAggregator cardsAggregator;

        public StatsReportBuilder(
            ITaskManagerClient taskManagerClient,
            ICardsAggregator cardsAggregator)
        {
            this.taskManagerClient = taskManagerClient;
            this.cardsAggregator = cardsAggregator;
        }

        public ReportModel[] BuildForBillingDelivery()
        {
            var lists = taskManagerClient.GetBoardLists("58d22275df59d0815216e1f0");
            var listNames = lists.Select(l => l.Name).ToArray();
            var doneList = lists.First(l => l.Name.Contains("Готово"));
            var listNameToIdIndex = lists.ToDictionary(l => l.Name, l => l.Id);
            var aggregation = cardsAggregator.Aggregate(doneList);

            var reportsList = new List<ReportModel>
            {
                new ReportModel("БД", Build(aggregation, listNames, listNameToIdIndex)),
                new ReportModel("БД.Продукты", Build(aggregation.FilterBy(KnownTaskQueuesLabels.ProductQueueLabel), listNames, listNameToIdIndex)),
                new ReportModel("БД.CRM", Build(aggregation.FilterBy(KnownTaskQueuesLabels.CrmQueueLabel), listNames, listNameToIdIndex)),
                new ReportModel("БД.Инфраструктура", Build(aggregation.FilterBy(KnownTaskQueuesLabels.InfrastructureQueueLabel), listNames, listNameToIdIndex)),
                new ReportModel("БД.Эксплуатация", Build(aggregation.FilterBy(KnownTaskQueuesLabels.SupportQueueLabel), listNames, listNameToIdIndex))
            };
            return reportsList.ToArray();
        }

        public IEnumerable<ReportModel> BuildForDirections()
        {
            var boards = taskManagerClient.GetAllBoards(organizationName).ToArray();
            var directionBoards = boards.Where(b => b.Name.StartsWith("[К]") && b.IsClosed == false);
            foreach (var directionBoard in directionBoards)
            {
                var lists = taskManagerClient.GetBoardLists(directionBoard.Id);
                var doneList = lists.Count(l => l.Name.Contains("Готово")) == 1 ? lists.First(l => l.Name.Contains("Готово")) : lists.Last();
                var listNames = lists.Select(l => l.Name).ToArray();
                var listNameToIdIndex = lists.ToDictionary(l => l.Name, l => l.Id);
                var aggregation = cardsAggregator.Aggregate(doneList);
                yield return new ReportModel(directionBoard.Name, Build(aggregation, listNames, listNameToIdIndex));
            }
        }

        private static IList<ReportRow> Build(CardsAggregationModel cardsAggregation, string[] listNames, Dictionary<string, string> listNameToIdIndex)
        {
            var rowsList = new List<ReportRow> {BuildHeader(listNames)};
            foreach (var cardStats in cardsAggregation.CardsStats)
            {
                rowsList.Add(BuildCardStats(cardStats, listNames, listNameToIdIndex));
            }
            rowsList.Add(ReportRow.Empty);
            rowsList.AddRange(BuildAgregationStats("Full aggregation stats", cardsAggregation.FullAggregationStats));
            rowsList.AddRange(BuildAgregationStats("S tasks aggregation stats", cardsAggregation.SAggregationStats));
            rowsList.AddRange(BuildAgregationStats("M tasks aggregation stats", cardsAggregation.MAggregationStats));
            rowsList.AddRange(BuildAgregationStats("L tasks aggregation stats", cardsAggregation.LAggregationStats));
            rowsList.AddRange(BuildAgregationStats("XL tasks aggregation stats", cardsAggregation.XLAggregationStats));

            return rowsList;
        }

        private static ReportRow[] BuildAgregationStats(string header, AggregationTimeStats cardsAggregationStats)
        {
            if (cardsAggregationStats.AreEmpty())
                return new ReportRow[0];

            return new[]
            {
                ReportRow.Create(header),
                ReportRow.Create("Average cycle time", FormatTimeSpan(cardsAggregationStats.AverageTime)),
                ReportRow.Create("Longest cycle time", FormatTimeSpan(cardsAggregationStats.LongestTimeCard.CycleTime), $"cardName {cardsAggregationStats.LongestTimeCard.Name}"), 
                ReportRow.Create("Shortest cycle time", FormatTimeSpan(cardsAggregationStats.ShortestTimeCard.CycleTime), $"cardName {cardsAggregationStats.ShortestTimeCard.Name}"), 
            };
        }

        private static ReportRow BuildCardStats(CardStatsModel cardStats, string[] listNames, Dictionary<string, string> listNameToIdIndex)
        {
            var reportRow = ReportRow.Create(cardStats.Name, cardStats.Size, FormatTimeSpan(cardStats.CycleTime));
            foreach (var listName in listNames)
            {
                var listId = listNameToIdIndex[listName];
                if (cardStats.ListStats.ContainsKey(listId))
                {
                    reportRow.Append(FormatTimeSpan(cardStats.ListStats[listId]));
                }
            }
            return reportRow;
        }

        private static ReportRow BuildHeader(string[] listNames)
        {
            var reportRow = ReportRow.Create("Name", "Size", "CycleTime");
            foreach (var listName in listNames)
            {
                reportRow.Append(listName);
            }
            return reportRow;
        }

        private static string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{timeSpan.Days},{Math.Round((double)timeSpan.Hours / 24 * 10).ToString().First()}";
        }
    }
}