using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode.Core
{
    public class Day3
    {
        public class Part1 : IProblem<long>
        {
            public async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                return await SolveAsync(input, 3, 1);
            }

            public async Task<long> SolveAsync(IAsyncEnumerable<string> input, int right, int down)
            {
                var col = 0;
                var treeCount = 0;

                var enumerator = input.GetAsyncEnumerator();
                while(await enumerator.MoveNextAsync())
                {
                    var line = enumerator.Current;
                    if(line[col] == '#')
                    {
                        treeCount++;
                    }

                    for(var i = 0; i < down - 1; i++)
                    {
                        await enumerator.MoveNextAsync();
                    }

                    col = (col + right) % line.Length;
                }

                return treeCount;
            }
        }

        public class Part2 : IProblem<long>
        {
            public async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var part1 = new Part1();
                var a = await part1.SolveAsync(input, 1, 1);
                var b = await part1.SolveAsync(input, 3, 1);
                var c = await part1.SolveAsync(input, 5, 1);
                var d = await part1.SolveAsync(input, 7, 1);
                var e = await part1.SolveAsync(input, 1, 2);

                return a * b * c * d * e;
            }
        }
    }
}
