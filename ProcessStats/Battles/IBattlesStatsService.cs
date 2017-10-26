using System;

namespace ProcessStats.Battles
{
    public interface IBattlesStatsService
    {
        BattlesStats GetStats(DateTime date);
    }
}