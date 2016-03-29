using System;

namespace SKBKontur.Infrastructure.Common
{
    public class TimeValue<T>
    {
        private readonly int expireInSeconds;

        public TimeValue(T value, int expireInSeconds = 600)
        {
            this.expireInSeconds = expireInSeconds;
            Value = value;
            Time = DateTime.Now;
        }

        public DateTime Time { get; private set; }
        public T Value { get; private set; }

        public bool IsExpire()
        {
            return (DateTime.Now - Time).TotalSeconds > expireInSeconds;
        }
    }
}