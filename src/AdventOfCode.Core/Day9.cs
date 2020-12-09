using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day9
    {
        public class Part1 : IProblem<long>
        {
            public async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                const int preambleSize = 25;
                var preamble = new Queue<long>(preambleSize);
                var preambleArray = new long[preambleSize];

                var lineNumber = 0;
                await foreach (var line in input)
                {
                    if (!long.TryParse(line, out var number))
                    {
                        throw new Exception($"Invalid input: {line}");
                    }

                    if (preamble.Count == preambleSize)
                    {
                        preamble.CopyTo(preambleArray, 0);

                        if (IsSum(preambleArray, number) == false)
                        {
                            return number;
                        }

                        preamble.Dequeue();
                    }

                    preamble.Enqueue(number);
                    lineNumber++;
                }

                throw new Exception("All numbers are sum of items from the preamble :/");
            }

            public bool IsSum(long[] preambleArray, long number)
            {
                var isSum = false;
                for (var i = 0; isSum == false && i < preambleArray.Length; i++)
                {
                    for (var j = i + 1; isSum == false && j < preambleArray.Length; j++)
                    {
                        var x = preambleArray[i];
                        var y = preambleArray[j];

                        var sum = x + y;

                        if (sum == number)
                        {
                            isSum = true;
                        }
                    }
                }

                return isSum;
            }
        }

        public class Part2 : IProblem<long>
        {
            public async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var resultPart1 = 258585477;

                var inputArray = await input.ToArrayAsync(s => long.Parse(s));

                var windowLowerBound = 0;
                var windowUpperBound = 1;
                var windowSum = inputArray[0] + inputArray[1];
                var window = new SortedSet<long>()
                {
                    inputArray[0],
                    inputArray[1]
                };

                var result = 0L;
                var resultWindowLength = 0;

                while (windowUpperBound < inputArray.Length && windowLowerBound <= windowUpperBound && inputArray[windowUpperBound] != resultPart1)
                {
                    if (windowSum == resultPart1)
                    {
                        if(resultWindowLength < (windowUpperBound - windowLowerBound))
                        {
                            result = window.First() + window.Last();
                            resultWindowLength = windowUpperBound - windowLowerBound;
                        }

                        windowSum -= inputArray[windowLowerBound];
                        window.Remove(inputArray[windowLowerBound]);
                        windowLowerBound++;
                    }
                    else if (windowSum > resultPart1)
                    {
                        if(windowLowerBound < windowUpperBound)
                        {
                            windowSum -= inputArray[windowLowerBound];
                            window.Remove(inputArray[windowLowerBound]);
                            windowLowerBound++;
                        }
                        else
                        {
                            windowUpperBound++;
                            windowSum += inputArray[windowUpperBound];
                            window.Add(inputArray[windowUpperBound]);
                        }
                    }
                    else if (windowSum < resultPart1)
                    {
                        windowUpperBound++;
                        windowSum += inputArray[windowUpperBound];
                        window.Add(inputArray[windowUpperBound]);
                    }
                }

                return result;
            }
        }
    }
}
