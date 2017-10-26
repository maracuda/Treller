using System;

namespace ProcessStats.Battles
{
    public class BattlesStats
    {
        public DateTime Date { get; set; }
        public int CreatedCount { get; set; }
        public int ReopenCount { get; set; }
        public int FixedCount { get; set; }
    }
}