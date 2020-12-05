using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode.Core
{
    public class Day2
    {
        public class Part1 : IProblem<int>
        {
            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var valid = 0;

                await foreach (var rawData in input)
                {
                    var parts = rawData.Split(' ');
                    var numbers = parts[0].Split('-');

                    var min = int.Parse(numbers[0]);
                    var max = int.Parse(numbers[1]);
                    var expectedCharacter = parts[1][0];
                    var password = parts[2];

                    var count = 0;

                    foreach (var character in password)
                    {
                        if (character == expectedCharacter)
                        {
                            count++;

                            if (count > max)
                            {
                                break;
                            }
                        }
                    }

                    if (count >= min && count <= max)
                    {
                        valid++;
                    }
                }

                return valid;
            }
        }

        public class Part2 : IProblem<int>
        {
            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var valid = 0;

                await foreach (var rawData in input)
                {
                    var parts = rawData.Split(' ');
                    var numbers = parts[0].Split('-');

                    var expectedCharacter = parts[1][0];
                    var password = parts[2];

                    var index1 = int.Parse(numbers[0]) - 1;
                    var index2 = int.Parse(numbers[1]) - 1;

                    char? char1 = index1 >= 0 && index1 < password.Length ? password[index1] : null;
                    char? char2 = index2 >= 0 && index2 < password.Length ? password[index2] : null;

                    if ((char1 != null && char2 != null) &&
                        (char1 == expectedCharacter && char2 != expectedCharacter ||
                        char1 != expectedCharacter && char2 == expectedCharacter))
                    {
                        valid++;
                    }
                }

                return valid;
            }
        }
    }
}
