using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Core
{
    public class Day3
    {
        public class Part1
        {
            public int Solve(string[] input, int right = 3, int down = 1)
            {
                var cols = input[0].Length;
                var rows = input.Length;

                // Start in top left.
                var row = 0;
                var col = 0;
                var treeCount = 0;

                while(row < rows)
                {
                    if(input[row][col] == '#')
                    {
                        treeCount++;
                    }

                    row += down;
                    col = (col + right) % cols;
                }

                return treeCount;
            }
        }

        public class Part2
        {
            public int Solve(string[] input)
            {
                var part1 = new Part1();
                var a = part1.Solve(input, 1, 1);
                var b = part1.Solve(input, 3, 1);
                var c = part1.Solve(input, 5, 1);
                var d = part1.Solve(input, 7, 1);
                var e = part1.Solve(input, 1, 2);

                return a * b * c * d * e;
            }
        }

        public static string[] GetData()
        {
            var data = new List<string>();
            using (var reader = File.OpenText("InputDay3.txt"))
            {
                var line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    data.Add(line);
                }
            }

            return data.ToArray();
        }
    }
}
