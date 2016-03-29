using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Statistics
{
    public interface IStatisticsService
    {
        StatisticsViewModel GetStatistics(DateTime statisticsStartTime, DateTime statisticsFinishTime, bool reCalculate = false);
    }
}