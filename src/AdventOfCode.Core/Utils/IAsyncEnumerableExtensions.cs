using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Core.Utils
{
    public static class IAsyncEnumerableExtensions
    {
        public async static Task<T[]> ToArrayAsync<T>(this IAsyncEnumerable<T> input)
        {
            var tmp = new LinkedList<T>();

            await foreach(var item in input)
            {
                tmp.AddLast(item);
            }

            return tmp.ToArray();
        }

        public async static Task<TOutput[]> ToArrayAsync<TInput, TOutput>(this IAsyncEnumerable<TInput> input, Func<TInput, TOutput> converter)
        {
            var tmp = new LinkedList<TOutput>();

            await foreach (var item in input)
            {
                tmp.AddLast(converter(item));
            }

            return tmp.ToArray();
        }
    }
}
