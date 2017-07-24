using System;

namespace WebApplication.Implementation.Infrastructure.Extensions
{
    public static class FormattingExtensions
    {
        public static string Stringify(this DateTime value)
        {
            return value.ToString("dd.MM.yyyy HH:mm");
        }

        public static string Stringify(this DateTime? value, string defaultResult)
        {
            return value.HasValue ? value.Value.Stringify() : defaultResult;
        }

        public static string Stringify(this TimeSpan value)
        {
            return value.ToString(@"dd\.hh\:mm\:ss");
        }
    }
}