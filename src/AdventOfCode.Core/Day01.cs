﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day01 : Day<long, long>
    {
        protected override IProblem<long> GetPart1() => new Part1();
        protected override IProblem<long> GetPart2() => new Part2();

        private class Part1 : IProblem<long>
        {
            public async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var inputArray = await input.ToArrayAsync(line => long.Parse(line));

                for (var i = 0; i < inputArray.Length; i++)
                {
                    for (var j = i + 1; j < inputArray.Length; j++)
                    {
                        if (inputArray[i] + inputArray[j] == 2020)
                        {
                            return inputArray[i] * inputArray[j];
                        }
                    }
                }

                return -1;
            }
        }

        private class Part2 : IProblem<long>
        {
            public async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var inputArray = await input.ToArrayAsync(line => long.Parse(line));

                for (var i = 0; i < inputArray.Length; i++)
                {
                    for (var j = i + 1; j < inputArray.Length; j++)
                    {
                        for (var k = j + 1; k < inputArray.Length; k++)
                        {
                            if (inputArray[i] + inputArray[j] + inputArray[k] == 2020)
                            {
                                return inputArray[i] * inputArray[j] * inputArray[k];
                            }
                        }
                    }
                }

                return -1;
            }
        }
    }
}
