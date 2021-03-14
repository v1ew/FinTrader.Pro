using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinTrader.Pro.Bonds.Extensions
{
    public static class LinqExtensions
    {
        public static Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
        {
            return Task.WhenAll(enumerable.Select(item => action(item)));
        }
    }
}
