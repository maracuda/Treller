using System;

namespace SKBKontur.Infrastructure.Common
{
    public class DateTimeFactory : IDateTimeFactory
    {
        private static readonly object LastTimeLocker = new object();
        private static readonly object LastUtcTimeLocker = new object();

        private static DateTime lastTime;
        private static DateTime lastUtcTime;

        public DateTime Now
        {
            get
            {
                lock (LastTimeLocker)
                {
                    lastTime = lastTime >= DateTime.Now ? lastTime.AddTicks(1) : DateTime.Now;
                    return lastTime;
                }
            }
        }

        public DateTime UtcNow
        {
            get
            {
                lock (LastUtcTimeLocker)
                {
                    lastUtcTime = lastUtcTime >= DateTime.UtcNow ? lastUtcTime.AddTicks(1) : DateTime.UtcNow;
                    return lastUtcTime;
                }
            }
        }

        public long UtcTicks => UtcNow.Ticks;

        public long Ticks => Now.Ticks;

        public DateTime Create(long ticks)
        {
            return new DateTime(ticks);
        }
    }
}