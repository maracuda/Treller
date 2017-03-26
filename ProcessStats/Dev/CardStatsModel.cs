using System;

namespace SkbKontur.Treller.ProcessStats.Dev
{
    public class CardStatsModel
    {
        public string CardId { get; set; }
        public TimeSpan AnaliticsDuration { get; set; }
        public TimeSpan DevDuration { get; set; }
        public TimeSpan ReviewDuration { get; set; }
        public TimeSpan TestingDuration { get; set; }
        public TimeSpan CicleDuration => AnaliticsDuration + DevDuration + ReviewDuration + TestingDuration;
    }
}