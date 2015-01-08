using System;
using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Extensions
{
    public static class EnumerableExtensions
    {
        public static void Split<TSource>(this IEnumerable<TSource> sources, Func<TSource, bool> predicate, out TSource[] trueResult, out TSource[] falseResult)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            var result1 = new List<TSource>();
            var result2 = new List<TSource>();

            foreach (var source in sources)
            {
                if (predicate(source))
                {
                    result1.Add(source);
                }
                else
                {
                    result2.Add(source);
                }
            }

            trueResult = result1.ToArray();
            falseResult = result2.ToArray();
        }
    }
}