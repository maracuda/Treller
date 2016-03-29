using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Statistics
{
    public class StatisticsViewModel
    {
        public DateTime StatisticsStartTime { get; set; }
        public DateTime StatisticsFinishTime { get; set; }

        public TeamCardStatisticsModel FeatureTeamCardStatistics { get; set; }
        public TeamCardStatisticsModel ServiceTeamCardStatistics { get; set; }
        public TeamCardStatisticsModel Overall { get; set; }
    }
}