using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ViskeyTube.Common
{
    public static class DateTimeHelpers
    {
        private static Dictionary<string, string> russianEnglishMonthMap = new Dictionary<string, string>
        {
            { "января" , "Jan"},
            { "февраля","Feb"},
            { "марта","Mar"},
            { "апреля", "Apr"},
            { "мая","May"},
            { "июня","Jun"},
            { "июля","Jul"},
            { "августа","Aug"},
            { "сентября","Sep"},
            { "октября","Oct"},
            { "ноября" ,"Nov"},
            { "декабря","Dec"},
        };

        private static readonly Regex regexForRUssianDateTime = new Regex(@"(([0-9])|([0-2][0-9])|([3][0-1]))\ ("
                                                                       + "января" + "|"
                                                                       + "февраля" + "|"
                                                                       + "марта" + "|"
                                                                       + "апреля" + "|"
                                                                       + "мая" + "|"
                                                                       + "июня" + "|"
                                                                       + "июля" + "|"
                                                                       + "августа" + "|"
                                                                       + "сентября" + "|"
                                                                       + "октября" + "|"
                                                                       + "ноября" + "|"
                                                                       + "декабря"
                                                                       + @")\ \d{4}");

        public static DateTime? ExtractRussianDateTime(string source)
        {
            var match = regexForRUssianDateTime.Match(source);
            if (!match.Success)
                return null;

            var extractedRussian = match.Value;
            var extractedEnglish = extractedRussian.Replace(' ', '-');
            foreach (var kvp in russianEnglishMonthMap)
            {
                extractedEnglish = extractedEnglish.Replace(kvp.Key, kvp.Value);
            }

            return DateTime.TryParse(extractedEnglish, out var result) ? result : (DateTime?)null;
        }
    }
}