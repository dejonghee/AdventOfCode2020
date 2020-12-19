using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day15 : Day<int, int>
    {
        protected override IProblem<int> GetPart1() => new Part1();
        protected override IProblem<int> GetPart2() => new Part2();

        private class Part1 : IProblem<int>
        {
            public virtual async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                return await SolveProblemAsync(input, 2020);
            }

            protected async Task<int> SolveProblemAsync(IAsyncEnumerable<string> input, int lastTurn)
            {
                var numbers = await ParseInput(input);

                var seenNumbers = new Dictionary<int, int>();

                for (var i = 0; i < numbers.Length - 1; i++)
                {
                    seenNumbers.Add(numbers[i], i + 1);
                }

                var turn = numbers.Length + 1;
                var lastSpokenNumber = numbers[numbers.Length - 1];

                while (turn <= lastTurn)
                {
                    var spokenNumber = seenNumbers.TryGetValue(lastSpokenNumber, out var previousTurn)
                        ? (turn - 1) - previousTurn
                        : 0;

                    seenNumbers[lastSpokenNumber] = turn - 1;
                    lastSpokenNumber = spokenNumber;
                    turn++;
                }

                return lastSpokenNumber;
            }

            private static async Task<int[]> ParseInput(IAsyncEnumerable<string> input)
            {
                var enumerator = input.GetAsyncEnumerator();
                await enumerator.MoveNextAsync();
                return enumerator.Current.Split(',').Select(x => int.Parse(x)).ToArray();
            }
        }

        private class Part2 : Part1
        {
            public override async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                return await SolveProblemAsync(input, 30000000);
            }
        }
    }
}
