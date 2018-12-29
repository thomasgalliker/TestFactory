using System;
using System.Collections.Generic;
using System.Linq;

namespace TestFactory.Internal
{
    internal static class StatisticExtensions
    {
        internal static TimeSpan Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
        {
            return source.Select(selector).Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
        }
    }
}