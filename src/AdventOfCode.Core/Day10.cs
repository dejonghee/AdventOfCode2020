using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;
using C5;

namespace AdventOfCode.Core
{
    public class Day10 : Day<int, long>
    {
        protected override IProblem<int> GetPart1() => new Part1();
        protected override IProblem<long> GetPart2() => new Part2();

        private class Part1 : IProblem<int>
        {
            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var adapters = new SortedSet<long>();
                await foreach (var line in input)
                {
                    adapters.Add(long.Parse(line));
                }

                var current = 0L;
                var amountOneJolt = 0;
                var amountThreeJolts = 0;
                foreach (var adapter in adapters)
                {
                    var diff = adapter - current;
                    if (diff == 1)
                        amountOneJolt++;
                    else if (diff == 3)
                        amountThreeJolts++;
                    else
                        throw new Exception("Not sure what to do :/");

                    current = adapter;
                }

                // Add device built-in adapter.
                amountThreeJolts++;

                return amountOneJolt * amountThreeJolts;
            }
        }

        public class Part2 : IProblem<long>
        {
            public async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var adapters = new SortedArray<long>();
                await foreach (var line in input)
                {
                    adapters.Add(long.Parse(line));
                }

                // Add dummy adapter 0 to trigger branching on root level.
                adapters.Add(0);

                return Explore(adapters);
            }

            private long[] _cache;

            private long Explore(SortedArray<long> adapters, int index = 0)
            {
                if (index == 0)
                {
                    // Init cache when processing first item.
                    _cache = new long[adapters.Count];
                }

                var branches = Enumerable
                    .Range(1, 3)
                    .Where(offset =>
                    {
                        return index + offset < adapters.Count
                            && adapters[index + offset] - adapters[index] <= 3;
                    })
                    .Select(offset => index + offset)
                    .ToArray();

                if (branches.Length == 0)
                {
                    // Reached end of path.
                    return 1;
                }

                return branches.Sum(x =>
                {
                    if (_cache[x] == 0)
                    {
                        _cache[x] = Explore(adapters, x);
                    }
                    return _cache[x];
                });
            }
        }
    }
}
