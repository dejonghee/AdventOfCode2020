using System.Collections.Generic;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day03 : Day<long, long>
    {
        protected override IProblem<long> GetPart1() => new Part1();
        protected override IProblem<long> GetPart2() => new Part2();

        private class Part1 : IProblem<long>
        {
            public async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var col = 0;
                var treeCount = 0;

                var enumerator = input.GetAsyncEnumerator();
                while (await enumerator.MoveNextAsync())
                {
                    var line = enumerator.Current;
                    if (line[col] == '#')
                    {
                        treeCount++;
                    }

                    col = (col + 3) % line.Length;
                }

                return treeCount;
            }
        }

        private class Part2 : IProblem<long>
        {
            public async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var inputArray = await input.ToArrayAsync();
                var a = Solve(inputArray, 1, 1);
                var b = Solve(inputArray, 3, 1);
                var c = Solve(inputArray, 5, 1);
                var d = Solve(inputArray, 7, 1);
                var e = Solve(inputArray, 1, 2);

                return a * b * c * d * e;
            }

            private int Solve(string[] input, int right, int down)
            {
                var cols = input[0].Length;
                var rows = input.Length;

                // Start in top left.
                var row = 0;
                var col = 0;
                var treeCount = 0;

                while (row < rows)
                {
                    if (input[row][col] == '#')
                    {
                        treeCount++;
                    }

                    row += down;
                    col = (col + right) % cols;
                }

                return treeCount;
            }
        }
    }
}
