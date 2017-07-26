using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessStats.Dev
{
    public class CardStatsModel
    {
        public CardStatsModel()
        {
            ListStats = new Dictionary<string, TimeSpan>();
        }

        public string CardId { get; set; }
        public string CardName { get; set; }

        public Dictionary<string, TimeSpan> ListStats { get; }

        public TimeSpan CycleTime => ListStats.Values.Aggregate(TimeSpan.Zero, (current, val) => current.Add(val));
    }
}