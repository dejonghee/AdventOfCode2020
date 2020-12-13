using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day13 : Day<decimal, decimal>
    {
        protected override IProblem<decimal> GetPart1() => new Part1();
        protected override IProblem<decimal> GetPart2() => new Part2();

        private class Part1 : IProblem<decimal>
        {
            public async Task<decimal> SolveAsync(IAsyncEnumerable<string> input)
            {
                (var timestamp, var buses) = await ParseInputAsync(input);

                var busToTake = buses.OrderBy(busId => TimeToNextArrival(busId, timestamp)).First();

                return busToTake * TimeToNextArrival(busToTake, timestamp);
            }

            protected async Task<(decimal timestamp, decimal[] busIds)> ParseInputAsync(IAsyncEnumerable<string> input)
            {
                var inputEnumerator = input.GetAsyncEnumerator();
                if (await inputEnumerator.MoveNextAsync() == false)
                    throw new Exception("Invalid input");

                var timestamp = int.Parse(inputEnumerator.Current);

                if (await inputEnumerator.MoveNextAsync() == false)
                    throw new Exception("invalid input");

                var busIds = inputEnumerator.Current
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Where(busId => !string.Equals("x", busId, StringComparison.OrdinalIgnoreCase))
                    .Select(busId => decimal.Parse(busId))
                    .ToArray();

                return (timestamp, busIds);
            }

            protected decimal TimeToNextArrival(decimal busId, decimal currentTime)
            {
                return (Math.Ceiling(currentTime / busId) * busId) - currentTime;
            }
        }

        private class Part2 : IProblem<decimal>
        {
            public async Task<decimal> SolveAsync(IAsyncEnumerable<string> input)
            {
                var busIds = await ParseInputAsync(input);

                // Keep track of last 2 timestamps that were valid.
                // The difference defines the step size for the next options we should explore.
                var nrOfTimestampsToLookup = 2;
                var validTimestamps = new Stack<decimal>();

                // Steps we should take to increase the timestamp.
                var stepSize = busIds[0].Value;

                var timetracker = 0M;
                for (var busIndex = 1; busIndex < busIds.Length; busIndex++)
                {
                    var bus = busIds[busIndex];
                    if (bus.HasValue)
                    {
                        // For the last bus, we only need to find 1 valid timestamp.
                        nrOfTimestampsToLookup = busIndex == busIds.Length - 1
                            ? 1
                            : nrOfTimestampsToLookup;

                        while (validTimestamps.Count < nrOfTimestampsToLookup)
                        {
                            timetracker += stepSize;
                            if (IsValidTimestamp(timetracker, bus.Value, busIndex))
                            {
                                validTimestamps.Push(timetracker);
                            }
                        }

                        if (nrOfTimestampsToLookup == 2)
                        {
                            // Determine new step size.
                            stepSize = validTimestamps.Pop() - validTimestamps.Pop();
                            timetracker += stepSize;
                        }
                    }
                }

                return timetracker;
            }

            /// <summary>
            /// A timestamp for a bus is valid if the expected wait time equals the actual wait time.
            /// </summary>
            /// <returns></returns>
            private bool IsValidTimestamp(decimal currentTimestamp, decimal busId, decimal expectedWaitTime)
            {
                return (Math.Ceiling(currentTimestamp / busId) * busId) - currentTimestamp == (expectedWaitTime % busId);
            }

            protected async Task<decimal?[]> ParseInputAsync(IAsyncEnumerable<string> input)
            {
                var inputEnumerator = input.GetAsyncEnumerator();
                if (await inputEnumerator.MoveNextAsync() == false)
                    throw new Exception("Invalid input");

                // First line is no longer relevant.

                if (await inputEnumerator.MoveNextAsync() == false)
                    throw new Exception("invalid input");

                return inputEnumerator.Current
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select<string, decimal?>(input => decimal.TryParse(input, out var busId) ? busId : null)
                    .ToArray();
            }
        }
    }
}
