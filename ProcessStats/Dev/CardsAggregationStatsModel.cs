using System;
using System.Linq;

namespace ProcessStats.Dev
{
    public class CardsAggregationStatsModel
    {
        private CardsAggregationStatsModel() { }

        public CardStatsModel[] CardsStats { get; private set; }

        public TimeSpan AverageCycleTime
        {
            get
            {
                var averageTicks = CardsStats.Average(c => c.CycleTime.Ticks);
                return new TimeSpan(Convert.ToInt64(averageTicks));
            }
        }

        public CardStatsModel LongestCycleCardStats
        {
            get
            {
                return CardsStats.OrderByDescending(c => c.CycleTime.Ticks).First();
            }
        }

        public CardStatsModel ShortestCycleCardStats
        {
            get
            {
                return CardsStats.OrderBy(c => c.CycleTime.Ticks).First(f => f.CycleTime.Ticks > 0);
            }
        }

        public static CardsAggregationStatsModel Create(CardStatsModel[] cardStats)
        {
            return new CardsAggregationStatsModel
            {
                CardsStats = cardStats
            };
        }
    }
}