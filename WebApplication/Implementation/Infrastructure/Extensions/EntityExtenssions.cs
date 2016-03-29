using System;
using System.Linq;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Extensions
{
    public static class EntityExtenssions
    {
        public static string GetCardBranchName(this BoardCard card)
        {
            return SearchInfo(card.Description, "**Ветка:**", new[] { ':', '*' }, 1)
                   ?? SearchInfo(card.Description, "Ветка:", new[] {':', '*'}, 1)
                   ?? SearchInfo(card.Description, "**Ветка**:", new[] { ':', '*' }, 1)
                   ?? SearchInfo(card.Description, "Ветка", new[] { ':', '*' }, 1);
        }

        public static string GetAnalyticLink(this BoardCard card, string wikiUrl, string bugTrackerUrl)
        {
            return (SearchInfo(card.Description, wikiUrl)
                   ?? SearchInfo(card.Description, bugTrackerUrl)
                   ?? string.Empty).TrimEnd('.');
        }

        private static string SearchInfo(this string text, string searchText, char[] additionalSplitCharacters = null, int skip = 0)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(searchText))
            {
                return null;
            }

            var startIndex = text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
            if (startIndex == -1)
            {
                return null;
            }

            var splitCharacters = new[] {' ', '\r', '\n'}.Union(additionalSplitCharacters ?? new char[0]).ToArray();
            var restLength = text.Length - startIndex;
            var array = text.Substring(startIndex, restLength > 300 ? 300 : restLength)
                            .Split(splitCharacters, StringSplitOptions.RemoveEmptyEntries);

            if (array.Length == 0)
            {
                return null;
            }

            var result = array.Skip(skip).FirstOrDefault(x => x.Length > 1);
            return string.IsNullOrWhiteSpace(result) ? null : result;
        }
    }
}