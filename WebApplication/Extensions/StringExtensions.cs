using System.Globalization;

namespace SKBKontur.Treller.WebApplication.Extensions
{
    public static class StringExtensions
    {
        public static string ToLowerString<T>(this T value)
        {
            return value.ToString().ToLower();
        }

        public static string HtmlPercentFormat(this decimal value, string decimalSeparator = ".")
        {
            var numberFormatInfo = new NumberFormatInfo { PercentDecimalSeparator = decimalSeparator };
            return string.Format(numberFormatInfo, "{0:0.#####}%", value);
        }
    }
}