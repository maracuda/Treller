using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerClient.BusinessObjects.TaskManager;

namespace ProcessStats.Dev
{
    public class CardsAggregationModel
    {
        private CardsAggregationModel() { }

        public CardStatsModel[] CardsStats { get; private set; }
        public AggregationTimeStats FullAggregationStats { get; private set; }
        public AggregationTimeStats SAggregationStats { get; private set; }
        public AggregationTimeStats MAggregationStats { get; private set; }
        public AggregationTimeStats LAggregationStats { get; private set; }
        public AggregationTimeStats XLAggregationStats { get; private set; }

        public CardsAggregationModel FilterBy(CardLabel label)
        {
            return Create(CardsStats.Where(c => c.HasLabel(label)).ToArray());
        }

        public static CardsAggregationModel Create(CardStatsModel[] cardStats)
        {
            var filteredCardStats = cardStats.Where(c => c.CycleTime.Ticks > 0).OrderByDescending(c => c.LastActivity).ToArray();
            return new CardsAggregationModel
            {
                CardsStats = filteredCardStats,
                FullAggregationStats = AggregationTimeStats.Create(filteredCardStats),
                SAggregationStats = AggregationTimeStats.Create(filteredCardStats.Where(c => c.Size == CardSize.S).OrderByDescending(c => c.LastActivity)),
                MAggregationStats = AggregationTimeStats.Create(filteredCardStats.Where(c => c.Size == CardSize.M).OrderByDescending(c => c.LastActivity)),
                LAggregationStats = AggregationTimeStats.Create(filteredCardStats.Where(c => c.Size == CardSize.L).OrderByDescending(c => c.LastActivity)),
                XLAggregationStats = AggregationTimeStats.Create(filteredCardStats.Where(c => c.Size == CardSize.XL).OrderByDescending(c => c.LastActivity))
            };
        }
    }

    public class AggregationTimeStats
    {
        public static readonly AggregationTimeStats Empty = new AggregationTimeStats {AverageTime = TimeSpan.Zero};

        public TimeSpan AverageTime { get; private set; }
        public CardStatsModel LongestTimeCard { get; private set; }
        public CardStatsModel ShortestTimeCard { get; private set; }

        public bool AreEmpty()
        {
            return ReferenceEquals(this, Empty);
        }

        public static AggregationTimeStats Create(IEnumerable<CardStatsModel> cardStats)
        {
            if (cardStats.Any())
            {
                var orderedByDescendingStats = cardStats.OrderByDescending(c => c.CycleTime.Ticks);
                return new AggregationTimeStats
                {
                    AverageTime = new TimeSpan(Convert.ToInt64(orderedByDescendingStats.Average(c => c.CycleTime.Ticks))),
                    LongestTimeCard = orderedByDescendingStats.First(),
                    ShortestTimeCard = orderedByDescendingStats.Last()
                };
            }
            return Empty;
        }

    }
}