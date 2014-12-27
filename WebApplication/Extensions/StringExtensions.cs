using System.Globalization;
using System.Linq;

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

        public static string ToLoanDecimalFormat(this decimal value, int decimalPlaces = 2)
        {
            var format = string.Format("###0.{0}", new string(Enumerable.Range(0, decimalPlaces).Select(x => '#').ToArray()));;
            return value.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}