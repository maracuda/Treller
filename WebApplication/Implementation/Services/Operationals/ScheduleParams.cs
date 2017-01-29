using System;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Scheduler;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals
{
    public class ScheduleParams
    {
        private ScheduleParams(TimeSpan pollingPeriod, TimeSpan? at, ScheduleMode scheduleMode)
        {
            PollingPeriod = pollingPeriod;
            At = at;
            Mode = scheduleMode;
        }

        public TimeSpan PollingPeriod { get; private set; }
        public ScheduleMode Mode { get; private set; }
        public TimeSpan? At { get; private set; }

        public static ScheduleParams CreateAnytime(TimeSpan pollingPeriod)
        {
            return new ScheduleParams(pollingPeriod, null, ScheduleMode.Anytime);
        }

        public static ScheduleParams CreateEveryday(TimeSpan pollingPeriod, TimeSpan at)
        {
            return new ScheduleParams(pollingPeriod, at, ScheduleMode.EveryDay);
        }
    }
}