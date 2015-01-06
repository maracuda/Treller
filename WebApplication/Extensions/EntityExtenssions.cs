using System;
using SKBKontur.TaskManagerClient.BusinessObjects;
using System.Linq;

namespace SKBKontur.Treller.WebApplication.Extensions
{
    public static class EntityExtenssions
    {
        public static string GetCardBranchName(this BoardCard card)
        {
            var branchIndex = card.Description.IndexOf("ветка:", StringComparison.OrdinalIgnoreCase);
            if (branchIndex == -1)
            {
                return string.Empty;
            }

            return card.Description.Substring(branchIndex, card.Description.Length)
                                   .Split(new[] {' ', '\r', '\n', ':'}, StringSplitOptions.RemoveEmptyEntries)
                                   .Skip(1)
                                   .FirstOrDefault(x => x.Length > 1);
        } 
    }
}