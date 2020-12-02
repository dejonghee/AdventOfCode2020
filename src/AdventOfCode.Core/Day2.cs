using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Core
{
    public class Day2
    {
        public class Part1
        {
            public int Solve(Record[] input)
            {
                var valid = 0;

                foreach (var entry in input)
                {
                    var count = 0;

                    foreach (var character in entry.Pwd)
                    {
                        if (character == entry.C)
                        {
                            count++;

                            if (count > entry.Y)
                            {
                                break;
                            }
                        }
                    }

                    if (count >= entry.X && count <= entry.Y)
                    {
                        valid++;
                    }
                }

                return valid;
            }
        }

        public class Part2
        {
            public int Solve(Record[] input)
            {
                var valid = 0;

                foreach (var entry in input)
                {
                    var index1 = entry.X - 1;
                    var index2 = entry.Y - 1;

                    char? char1 = index1 >= 0 && index1 < entry.Pwd.Length ? entry.Pwd[index1] : null;
                    char? char2 = index2 >= 0 && index2 < entry.Pwd.Length ? entry.Pwd[index2] : null;

                    if ((char1 != null && char2 != null) &&
                        (char1 == entry.C && char2 != entry.C ||
                        char1 != entry.C && char2 == entry.C))
                    {
                        valid++;
                    }
                }

                return valid;
            }
        }

        public static Record[] GetData()
        {
            var data = new List<Record>();
            using (var reader = File.OpenText("InputDay2.txt"))
            {
                var line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(' ');
                    var numbers = parts[0].Split('-');
                    var character = parts[1][0];
                    var password = parts[2];

                    data.Add(new Record(int.Parse(numbers[0]), int.Parse(numbers[1]), character, password));
                }
            }

            return data.ToArray();
        }

        public record Record(int X, int Y, char C, string Pwd);
    }
}
