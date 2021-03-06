﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day07 : Day<int, int>
    {
        protected override IProblem<int> GetPart1() => new Part1();
        protected override IProblem<int> GetPart2() => new Part2();

        private class Part1 : IProblem<int>
        {
            private static readonly Regex Pattern = new Regex("^(.*) bags contain( ([0-9]+) ([a-z ]+) bag[s]?[,.])+", RegexOptions.Compiled);

            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var lookup = new Dictionary<string, List<string>>();

                await foreach (var entry in input)
                {
                    var match = Pattern.Match(entry);
                    var outerBag = match.Groups[1].Value;

                    for (var i = 0; i < match.Groups[3].Captures.Count(); i++)
                    {
                        var innerBag = match.Groups[4].Captures[i].Value;
                        if (!lookup.TryAdd(innerBag, new List<string> { outerBag }))
                        {
                            lookup[innerBag].Add(outerBag);
                        }
                    }
                }

                var queue = new Queue<string>(lookup["shiny gold"]);
                var roots = new HashSet<string>();

                while (queue.Count > 0)
                {
                    var bag = queue.Dequeue();
                    if (lookup.TryGetValue(bag, out var values))
                    {
                        foreach (var val in values)
                        {
                            roots.Add(bag);
                            queue.Enqueue(val);
                        }
                    }
                    else
                    {
                        roots.Add(bag);
                    }
                }

                return roots.Count;
            }
        }

        private class Part2 : IProblem<int>
        {
            private static readonly Regex Pattern = new Regex("^(.*) bags contain( ([0-9]+) ([a-z ]+) bag[s]?[,.])+", RegexOptions.Compiled);

            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var lookup = new Dictionary<string, List<(string bagName, int count)>>();

                await foreach (var entry in input)
                {
                    var match = Pattern.Match(entry);
                    var outerBag = match.Groups[1].Value;

                    for (var i = 0; i < match.Groups[3].Captures.Count(); i++)
                    {
                        var innerBag = match.Groups[4].Captures[i].Value;
                        var count = int.Parse(match.Groups[3].Captures[i].Value);

                        if (!lookup.TryAdd(outerBag, new List<(string, int)> { (innerBag, count) }))
                        {
                            lookup[outerBag].Add((innerBag, count));
                        }
                    }
                }

                GetStuff(lookup, "shiny gold", 1);

                return count;
            }

            private int count = 0;

            public void GetStuff(Dictionary<string, List<(string bagName, int count)>> data, string bag, int multiplier)
            {
                if (data.TryGetValue(bag, out var info))
                {
                    foreach (var x in info)
                    {
                        var additional = (x.count * multiplier);
                        count += additional;
                        GetStuff(data, x.bagName, additional);
                    }
                }
            }
        }
    }
}