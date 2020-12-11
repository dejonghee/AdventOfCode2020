using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day11
    {
        public class Part1 : IProblem<int>
        {
            public virtual async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                return await SolveProblemAsync(input, threshold: 4);
            }

            protected async Task<int> SolveProblemAsync(IAsyncEnumerable<string> input, int threshold)
            {
                var seatLayout = await ParseInputAsync(input);

                // Create copy that we can change when updating the layout.
                var seatLayoutCopy = new char[seatLayout.GetLength(0), seatLayout.GetLength(1)];
                Array.Copy(seatLayout, seatLayoutCopy, seatLayout.Length);

                var changed = true;

                while (changed)
                {
                    changed = false;

                    for (var row = 0; row < seatLayout.GetLength(0); row++)
                    {
                        for (var column = 0; column < seatLayout.GetLength(1); column++)
                        {
                            // Skip floor.
                            if (IsFloor(seatLayout, row, column))
                                continue;

                            var nrOfOccupiedAdjacentSeats = NrOfOccupiedSeats(seatLayout, row, column);

                            if (IsTaken(seatLayout, row, column) && nrOfOccupiedAdjacentSeats >= threshold)
                            {
                                // Seat becomes empty.
                                seatLayoutCopy[row, column] = 'L';
                                changed = true;
                            }
                            else if (IsEmpty(seatLayout, row, column) && nrOfOccupiedAdjacentSeats == 0)
                            {
                                // Seat becomes taken.
                                seatLayoutCopy[row, column] = '#';
                                changed = true;
                            }
                        }
                    }

                    if (changed)
                    {
                        //PrintLayout(seatLayoutCopy);
                        Array.Copy(seatLayoutCopy, seatLayout, seatLayout.Length);
                    }
                }

                return NrOfOccupiedSeats(seatLayout);
            }

            protected virtual int NrOfOccupiedSeats(char[,] layout, int row, int col)
            {
                var nrOfTakenSeats = 0;

                for (var rowOffSet = -1; rowOffSet < 2; rowOffSet++)
                {
                    for (var colOffset = -1; colOffset < 2; colOffset++)
                    {
                        // Skip current seat.
                        if (rowOffSet == 0 && colOffset == 0)
                            continue;

                        if (IsValidPosition(layout, row + rowOffSet, col + colOffset) &&
                            IsTaken(layout, row + rowOffSet, col + colOffset))
                            nrOfTakenSeats++;
                    }
                }

                return nrOfTakenSeats;
            }

            protected virtual bool IsValidPosition(char[,] seatLayout, int row, int col)
            {
                return row >= 0 &&
                       row < seatLayout.GetLength(0) &&
                       col >= 0 &&
                       col < seatLayout.GetLength(1);
            }

            protected static bool IsTaken(char[,] layout, int row, int col)
            {
                return layout[row, col] == '#';
            }

            protected static bool IsEmpty(char[,] layout, int row, int col)
            {
                return layout[row, col] == 'L';
            }

            protected static bool IsFloor(char[,] layout, int row, int col)
            {
                return layout[row, col] == '.';
            }

            private async Task<char[,]> ParseInputAsync(IAsyncEnumerable<string> input)
            {
                var data = await input.ToArrayAsync(entry =>
                {
                    return entry.Select(x =>
                    {
                        return x switch
                        {
                            '.' => '.',
                            'L' => '#',
                            _ => throw new Exception("Invalid input")
                        };
                    }).ToArray();
                });
                var result = new char[data.Length, data.Max(x => x.Length)];

                for (var i = 0; i < data.Length; ++i)
                    for (var j = 0; j < data[i].Length; ++j)
                        result[i, j] = data[i][j];

                return result;
            }

            private static int NrOfOccupiedSeats(char[,] seatLayout)
            {
                var result = 0;
                for (var i = 0; i < seatLayout.GetLength(0); i++)
                    for (var j = 0; j < seatLayout.GetLength(1); j++)
                        if (seatLayout[i, j] == '#')
                            result++;

                return result;
            }

            private static void PrintLayout(char[,] layout)
            {
                for (var i = 0; i < layout.GetLength(0); i++)
                {
                    for (var j = 0; j < layout.GetLength(1); j++)
                    {
                        Console.Write(layout[i, j]);
                    }

                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        public class Part2 : Part1
        {
            public override async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                return await SolveProblemAsync(input, threshold: 5);
            }

            protected override int NrOfOccupiedSeats(char[,] layout, int row, int col)
            {
                var nrOfTakenSeats = 0;

                for (var rowOffSet = -1; rowOffSet < 2; rowOffSet++)
                {
                    for (var colOffset = -1; colOffset < 2; colOffset++)
                    {
                        // Skip current seat.
                        if (rowOffSet == 0 && colOffset == 0)
                            continue;

                        var multiplier = 1;

                        while (IsValidPosition(layout, row + multiplier * rowOffSet, col + multiplier * colOffset) &&
                               IsFloor(layout, row + multiplier * rowOffSet, col + multiplier * colOffset) == true)
                        {
                            multiplier++;
                        }

                        if (IsValidPosition(layout, row + multiplier * rowOffSet, col + multiplier * colOffset) &&
                           IsFloor(layout, row + multiplier * rowOffSet, col + multiplier * colOffset) == false &&
                           IsTaken(layout, row + multiplier * rowOffSet, col + multiplier * colOffset))
                        {
                            nrOfTakenSeats++;
                        }
                    }
                }

                return nrOfTakenSeats;
            }
        }
    }
}
