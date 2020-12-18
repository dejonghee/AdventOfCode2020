using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day17 : IDay<int, int>
    {
        public Task<int> SolvePart1Async(IAsyncEnumerable<string> input) => new Part1().SolveAsync(input);

        public Task<int> SolvePart2Async(IAsyncEnumerable<string> input) => new Part2().SolveAsync(input);

        private class Part1 : IProblem<int>
        {
            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                const int cycles = 6;
                var initialState = await ParseInput(input);

                // Make 2D input 3D.
                var nextState = new char[initialState.GetLength(0), initialState.GetLength(0), initialState.GetLength(0)];
                for (var x = 0; x < nextState.GetLength(0); x++)
                {
                    for (var y = 0; y < nextState.GetLength(1); y++)
                    {
                        for (var z = 0; z < nextState.GetLength(2); z++)
                        {
                            if (z == 1)
                            {
                                nextState[x, y, z] = initialState[x, y];
                            }
                            else
                            {
                                nextState[x, y, z] = '.';
                            }
                        }
                    }
                }

                // Additional cycles.
                var currentState = nextState;
                nextState = new char[currentState.GetLength(0) + 2, currentState.GetLength(0) + 2, currentState.GetLength(0) + 2];
                for (var iteration = 0; iteration < cycles; iteration++)
                {
                    // Where the magic happens.
                    for (var x = 0; x < nextState.GetLength(0); x++)
                    {
                        for (var y = 0; y < nextState.GetLength(1); y++)
                        {
                            for (var z = 0; z < nextState.GetLength(2); z++)
                            {
                                var isActive = IsActive(currentState, (x, y, z));
                                var nrOfActiveNeighbors = NumberOfActiveNeighbors(currentState, (x, y, z));

                                nextState[x, y, z] =
                                    // If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active.
                                    (isActive && (nrOfActiveNeighbors == 2 || nrOfActiveNeighbors == 3)) ||
                                    // If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active.
                                    (isActive == false && nrOfActiveNeighbors == 3)
                                    ? '#'
                                    : '.';
                            }
                        }
                    }

                    currentState = nextState;
                    nextState = new char[currentState.GetLength(0) + 2, currentState.GetLength(0) + 2, currentState.GetLength(0) + 2];
                }

                // Get number of active cubes.
                var numberOfActiveCubes = 0;
                for (var x = 0; x < nextState.GetLength(0); x++)
                    for (var y = 0; y < nextState.GetLength(1); y++)
                        for (var z = 0; z < nextState.GetLength(2); z++)
                            numberOfActiveCubes += currentState[x, y, z] == '#' ? 1 : 0;

                return numberOfActiveCubes;
            }

            private static async Task<char[,]> ParseInput(IAsyncEnumerable<string> input)
            {
                char[,] result = default;

                var lineId = 0;
                await foreach (var line in input)
                {
                    if (result == default)
                        result = new char[line.Length, line.Length];

                    for (var i = 0; i < line.Length; i++)
                    {
                        result[lineId, i] = line[i];
                    }

                    lineId++;
                }

                return result;
            }

            private static bool IsActive(char[,,] currentState, (int, int, int) positionNextState)
            {
                var (x, y, z) = positionNextState;

                // New cubes start inactive.
                if (x == 0 || y == 0 || z == 0 || x == currentState.GetLength(0) || y == currentState.GetLength(1) || z == currentState.GetLength(2))
                    return false;

                return currentState[x - 1, y - 1, z - 1] == '#';
            }

            private static int NumberOfActiveNeighbors(char[,,] currentState, (int, int, int) positionNextState)
            {
                var (x, y, z) = positionNextState;
                var numberOfActiveNeighbors = 0;

                for (var i = -1; i < 2; i++)
                {
                    for (var j = -1; j < 2; j++)
                    {
                        for (var k = -1; k < 2; k++)
                        {
                            // Skip current item.
                            if (i == 0 && j == 0 && k == 0)
                                continue;


                        }
                    }
                }

                return numberOfActiveNeighbors;
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
