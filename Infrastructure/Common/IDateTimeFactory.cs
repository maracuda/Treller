using System;

namespace SKBKontur.Infrastructure.Common
{
    public interface IDateTimeFactory
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
        long Ticks { get; }
        long UtcTicks { get; }

        DateTime Create(long ticks);
    }
}