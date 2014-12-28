using System;
using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Extensions
{
    public static class EntityExtenssions
    {
        public static string GetCardBranchName(this BoardCard card)
        {
            int branchIndex;
            if (!string.IsNullOrEmpty(card.Description) && (branchIndex = card.Description.IndexOf("ветка", StringComparison.OrdinalIgnoreCase)) >= 0)
            {
                var startBranchNameIndex = card.Description.IndexOf(' ', branchIndex);
                var endBranchNameIndex = card.Description.IndexOf(' ', startBranchNameIndex + 1);
                endBranchNameIndex = endBranchNameIndex < startBranchNameIndex ? card.Description.Length : endBranchNameIndex;
                return card.Description.Substring(startBranchNameIndex, endBranchNameIndex - startBranchNameIndex).Trim();
            }

            return string.Empty;
        } 
    }
}