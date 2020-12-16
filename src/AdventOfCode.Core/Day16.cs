using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day16 : IDay<long, long>
    {
        public Task<long> SolvePart1Async(IAsyncEnumerable<string> input) => new Part1().SolveAsync(input);

        public Task<long> SolvePart2Async(IAsyncEnumerable<string> input) => new Part2().SolveAsync(input);

        private class Part1 : IProblem<long>
        {
            protected static readonly Regex FieldParser = new Regex("^([^:]*): ([0-9]+)-([0-9]+) or ([0-9]+)-([0-9]+)$", RegexOptions.Compiled);

            public virtual async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var inputEnumerator = input.GetAsyncEnumerator();

                var contraints = await ParseConstraints(inputEnumerator);
                await SkipTicket(inputEnumerator);

                var errorRate = 0;
                await foreach (var nearbyTicket in ParseNearbyTickets(inputEnumerator))
                {
                    foreach (var value in nearbyTicket)
                    {
                        if (contraints.Any(c => c.IsValid(value)) == false)
                        {
                            errorRate += value;
                        }
                    }
                }

                return errorRate;
            }

            private static async Task<List<Constraint>> ParseConstraints(IAsyncEnumerator<string> inputEnumerator)
            {
                var constraints = new List<Constraint>();

                while (await inputEnumerator.MoveNextAsync() && string.IsNullOrEmpty(inputEnumerator.Current) == false)
                {
                    var match = FieldParser.Match(inputEnumerator.Current);
                    if (!match.Success)
                        throw new Exception("Invalid input");

                    constraints.Add(new Constraint(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value)));
                    constraints.Add(new Constraint(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value)));
                }

                return constraints;
            }

            protected async IAsyncEnumerable<int[]> ParseNearbyTickets(IAsyncEnumerator<string> inputEnumerator)
            {
                // Skip 'nearby tickets:' line.
                await inputEnumerator.MoveNextAsync();

                while (await inputEnumerator.MoveNextAsync())
                {
                    yield return inputEnumerator.Current
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(value => int.Parse(value))
                        .ToArray();
                }
            }

            private static async Task SkipTicket(IAsyncEnumerator<string> inputEnumerator)
            {
                // Ignore own ticket for now (3 lines).
                await inputEnumerator.MoveNextAsync();
                await inputEnumerator.MoveNextAsync();
                await inputEnumerator.MoveNextAsync();
            }

            private record Constraint(int Min, int Max)
            {
                public bool IsValid(int value) => Min <= value && value <= Max;
            }
        }

        private class Part2 : Part1
        {
            public override async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var inputEnumerator = input.GetAsyncEnumerator();

                var fields = await ParseFields(inputEnumerator);
                var ticket = await ParseTicket(inputEnumerator);

                var fieldMapping = new Dictionary<int, List<string>>(fields.Count);

                await foreach (var nearbyTicket in ParseNearbyTickets(inputEnumerator))
                {
                    for (var index = 0; index < nearbyTicket.Length; index++)
                    {
                        var matchingFields = fields
                            .Where(f => f.Match(nearbyTicket[index]))
                            .Select(x => x.Name)
                            .ToList();

                        if (matchingFields.Count > 0)
                        {
                            fieldMapping[index] = fieldMapping.TryGetValue(index, out var value)
                                ? value.Intersect(matchingFields).ToList()
                                : matchingFields;
                        }
                    }
                }

                var result = 1L;
                while (fieldMapping.Any(x => x.Value.Count > 0))
                {
                    var fieldByOptions = fieldMapping
                        .OrderBy(x => x.Value.Count)
                        .Where(x => x.Value.Count > 0)
                        .FirstOrDefault();

                    if (fieldByOptions.Value.Single().StartsWith("departure"))
                    {
                        result *= ticket[fieldByOptions.Key];
                    }

                    fieldMapping = fieldMapping
                        .ToDictionary(x => x.Key, x => x.Value.Except(fieldByOptions.Value).ToList());
                }

                return result;
            }

            private static async Task<List<Field>> ParseFields(IAsyncEnumerator<string> inputEnumerator)
            {
                var fields = new List<Field>();

                while (await inputEnumerator.MoveNextAsync() && string.IsNullOrEmpty(inputEnumerator.Current) == false)
                {
                    var match = FieldParser.Match(inputEnumerator.Current);
                    if (!match.Success)
                        throw new Exception("Invalid input");

                    fields.Add(
                        new Field(
                            match.Groups[1].Value,
                            int.Parse(match.Groups[2].Value),
                            int.Parse(match.Groups[3].Value),
                            int.Parse(match.Groups[4].Value),
                            int.Parse(match.Groups[5].Value)
                        )
                    );
                }

                return fields;
            }

            private static async Task<int[]> ParseTicket(IAsyncEnumerator<string> inputEnumerator)
            {
                // Skip 'your ticket:'
                await inputEnumerator.MoveNextAsync();

                await inputEnumerator.MoveNextAsync();
                var ticket = inputEnumerator.Current;

                // Skip empty line after ticket.
                await inputEnumerator.MoveNextAsync();

                return ticket.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
            }

            private record Field(string Name, int Min1, int Max1, int Min2, int Max2)
            {
                public bool Match(int value) => (Min1 <= value && value <= Max1) || (Min2 <= value && value <= Max2);
            }
        }
    }
}
