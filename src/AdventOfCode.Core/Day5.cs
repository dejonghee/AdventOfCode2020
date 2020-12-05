using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Core
{
    public class Day5
    {
        private const int planeRows = 128;
        private const int planeColumns = 8;

        public class Part1 : IProblem<int>
        {
            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var highestSeatId = -1;

                await foreach (var boardingPass in input)
                {
                    var seatId = GetSeat(boardingPass).Id;
                    if (seatId > highestSeatId)
                    {
                        highestSeatId = seatId;
                    }
                }

                return highestSeatId;
            }

            public Seat GetSeat(string boardingPass)
            {
                int? rowMin = null;
                int? rowMax = null;

                int? colMin = null;
                int? colMax = null;

                for (var i = 0; i < boardingPass.Length; i++)
                {
                    if (i < 7)
                    {
                        var val = (int)Math.Floor((decimal)(((rowMax ?? planeRows) + (rowMin ?? 1)) / 2));
                        if (boardingPass[i] == 'F')
                        {
                            rowMax = val;
                        }
                        else
                        {
                            rowMin = val + 1;
                        }
                    }
                    else
                    {
                        var val = (int)Math.Floor((decimal)(((colMax ?? planeColumns) + (colMin ?? 1)) / 2));
                        if (boardingPass[i] == 'L')
                        {
                            colMax = val;
                        }
                        else
                        {
                            colMin = val + 1;
                        }
                    }
                }

                if ((rowMin == null && rowMax == null) ||
                    (colMin == null && colMax == null) ||
                    (rowMin != null && rowMax != null && rowMin != rowMax) ||
                    (colMin != null && colMax != null && colMin != colMax))
                {
                    throw new Exception($"Invalid result: input = {boardingPass}, rowMin = {rowMin}, rowMax = {rowMax}, colMin = {colMin}, colMax = {colMax}.");
                }

                var row = (rowMax ?? rowMin.Value) - 1;
                var column = (colMax ?? colMin.Value) - 1;

                return new Seat(row, column);
            }
        }

        public class Part2 : IProblem<int>
        {
            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var part1 = new Part1();
                var plane = new bool[planeRows, planeColumns];
                var seats = new List<Seat>();

                await foreach (var boardingPass in input)
                {
                    var seat = part1.GetSeat(boardingPass);
                    plane[seat.Row, seat.Column] = true;
                    seats.Add(seat);
                }

                for (int row = 1; row < planeRows - 1; row++)
                {
                    for (int column = 0; column < planeColumns; column++)
                    {
                        if (plane[row, column] == false)
                        {
                            var seat = new Seat(row, column);
                            var seatX = seats.FirstOrDefault(x => x.Id == seat.Id - 1);
                            var seatY = seats.FirstOrDefault(x => x.Id == seat.Id + 1);

                            if (seatX != null && seatY != null)
                            {
                                return new Seat(row, column).Id;
                            }
                        }
                    }
                }

                return -1;
            }
        }

        public class Seat
        {
            public int Row { get; }
            public int Column { get; }
            public int Id { get; }

            /// <summary>
            /// Row & column are 0 based.
            /// </summary>
            public Seat(int row, int column)
            {
                Row = row;
                Column = column;
                Id = (row * 8) + column;
            }
        }
    }
}
