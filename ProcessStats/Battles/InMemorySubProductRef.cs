using System.Collections.Generic;
using System.Linq;

namespace ProcessStats.Battles
{
    public class InMemorySubProductRef : ISubProductsRef
    {
        private static readonly Dictionary<string, string> subProductsCache = new Dictionary<string, string>
        {
            {"ProspectiveSales", "Потенциальные продаж"},
            {"Orders", "Потенциальные продаж"},
            {"Documents", "Документы"},
            {"TarrificationAndDelivery", "Тарификация и доставка"},
            {"Infrastructure", "Инфраструктура"},
            {"Support", "Команда АРМов"},
        };

        public string[] GetSubproductIds()
        {
            return subProductsCache.Keys.ToArray();
        }

        public string GetSubProductName(string subProductId)
        {
            return subProductsCache[subProductId];
        }
    }
}