namespace SKBKontur.Treller.WebApplication.Extensions
{
    public static class StringExtensions
    {
        public static string ToLowerString<T>(this T value)
        {
            return value.ToString().ToLower();
        }
    }
}