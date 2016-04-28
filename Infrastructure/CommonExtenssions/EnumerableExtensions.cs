using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SKBKontur.Infrastructure.CommonExtenssions
{
    public static class EnumerableExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            var attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static IEnumerable<TObject> DistinctBy<TKey, TObject>(this IEnumerable<TObject> collection, Func<TObject, TKey> keySelector, IEqualityComparer<TKey> keyComparer = null)
        {
            return collection.GroupBy(keySelector, keyComparer).Select(x => x.First());
        }

        public static bool Contains(this string input, string value, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (input == null || value == null)
                return false;

            if (value.Length == 0)
                return true;

            if (input.Length == 0)
                return false;

            return input.IndexOf(value, comparison) >= 0;
        }

        public static TVal IfNotNull<T, TVal>(this T value, Func<T, TVal> valueFunc, TVal defaultValue = default(TVal)) where T : class
        {
            return value != null ? valueFunc(value) : defaultValue;
        }
    }
}