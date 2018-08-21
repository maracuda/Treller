using System.Collections.Generic;
using System.Linq;

namespace ProcessStats.Battles
{
    public class InMemorySubProductRef : ISubProductsRef
    {
        private static readonly Dictionary<string, string> subProductsCache = new Dictionary<string, string>
        {
            {"ProspectiveSales", "Потенциальные продажи"},
            {"Orders", "Заказы"},
            {"Documents", "Документы"},
            {"TarrificationAndDelivery", "Тарификация и доставка"},
            {"Infrastructure", "Инфраструктура"},
            {"Support", "Команда АРМов"},
            {"Payments", "Платежи"},
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