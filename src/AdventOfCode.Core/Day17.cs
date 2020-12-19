using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day17 : Day<int, int>
    {
        protected override IProblem<int> GetPart1() => new Part1();
        protected override IProblem<int> GetPart2() => new Part2();

        private class Part1 : IProblem<int>
        {
            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                const int cycles = 6;
                var (currentState, size) = await ParseInput(input);

                var cycle = 0;
                while (cycle < cycles)
                {
                    var nextState = new HashSet<(int x, int y, int z)>();

                    for (int x = -cycle; x < size + cycle; x++)
                    {
                        for (var y = -cycle; y < size + cycle; y++)
                        {
                            for (var z = -cycle; z < size + cycle; z++)
                            {
                                var active = currentState.Contains((x, y, z));
                                var neighbors = GetNeighbors(x, y, z);
                                var activeNeighbors = currentState.Intersect(neighbors).Count();

                                if (active == true && (activeNeighbors == 2 || activeNeighbors == 3))
                                    nextState.Add((x, y, z));

                                if (active == false && activeNeighbors == 3)
                                    nextState.Add((x, y, z));
                            }
                        }
                    }

                    Print(nextState, size, cycle);

                    currentState = nextState;
                    cycle++;
                }

                return currentState.Count;
            }

            private static async Task<(HashSet<(int x, int y, int z)> cubes, int size)> ParseInput(IAsyncEnumerable<string> input)
            {
                var result = new HashSet<(int x, int y, int z)>();
                var x = 0;

                await foreach (var line in input)
                {
                    for (var y = 0; y < line.Length; y++)
                    {
                        if (line[y] == '#')
                            result.Add((x, y, 0));
                    }

                    x++;
                }

                return (result, x);
            }

            private static HashSet<(int x, int y, int z)> GetNeighbors(int x, int y, int z)
            {
                var result = new HashSet<(int x, int y, int z)>(26);

                for (var i = -1; i < 2; i++)
                {
                    for (var j = -1; j < 2; j++)
                    {
                        for (var k = -1; k < 2; k++)
                        {
                            if ((i == 0 && j == 0 && k == 0) == false)
                                result.Add((x + i, y + j, z + k));
                        }
                    }
                }

                return result;
            }

            private static void Print(HashSet<(int x, int y, int z)> state, int size, int cycle)
            {
                for (var x = -cycle; x < size + cycle; x++)
                {
                    for (var y = -cycle; y < size + cycle; y++)
                    {
                        for (var z = -cycle; z < size + cycle; z++)
                        {
                            if (state.Contains((x, y, z)))
                                Console.Write('#');
                            else
                                Console.Write('.');
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
        }

        private class Part2 : IProblem<int>
        {
            public Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                throw new NotImplementedException();
            }
        }
    }
}
