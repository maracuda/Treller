using System;

namespace SKBKontur.Treller.WebApplication.Services.Statistics
{
    public interface IStatisticsService
    {
        StatisticsViewModel GetStatistics(DateTime statisticsStartTime, DateTime statisticsFinishTime, bool reCalculate = false);
    }
}