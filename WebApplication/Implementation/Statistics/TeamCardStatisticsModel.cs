using System;
using System.Collections.Generic;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.Statistics
{
    public class TeamCardStatisticsModel
    {
        public int Count { get; set; }
        public TimeSpan AnalyticTimeSummary { get; set; }
        public TimeSpan DevelopTimeSummary { get; set; }
        public TimeSpan TestingTimeSummary { get; set; }
        public TimeSpan ReleaseTimeSummary { get; set; }
        public TimeSpan MedianReleaseTime { get; set; }

        public TimeSpan MaxReleaseTime { get; set; }
        public TimeSpan MinReleaseTime { get; set; }

        public double AverageReleaseDays { get; set; }
        public double AverageAnalyticDays { get; set; }
        public double AverageDevelopDays { get; set; }
        public double AverageTestingDays { get; set; }

        public Dictionary<CardState, int> PeriodStates { get; set; }
    }
}