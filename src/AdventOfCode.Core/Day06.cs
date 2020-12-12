using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day06 : Day<int, int>
    {
        protected override IProblem<int> GetPart1() => new Part1();
        protected override IProblem<int> GetPart2() => new Part2();

        private class Part1 : IProblem<int>
        {
            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var sum = 0;

                var uniqueGroupAnswers = new HashSet<char>(26);

                await foreach (var answerString in input)
                {
                    if (string.IsNullOrEmpty(answerString))
                    {
                        // Reached new group, process previous group.
                        sum += uniqueGroupAnswers.Count;

                        // Reset state.
                        uniqueGroupAnswers.Clear();
                    }
                    else
                    {
                        foreach (var answer in answerString)
                        {
                            uniqueGroupAnswers.Add(answer);
                        }
                    }
                }

                // Add remaining.
                sum += uniqueGroupAnswers.Count;

                return sum;
            }
        }

        private class Part2 : IProblem<int>
        {
            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var sum = 0;

                var answerCount = new int[26];
                var nrOfGroupMembers = 0;

                await foreach (var entry in input)
                {
                    if (string.IsNullOrEmpty(entry))
                    {
                        // Reached new group, process previous group.
                        sum += answerCount.Count(x => x == nrOfGroupMembers);

                        // Reset state.
                        Array.Clear(answerCount, 0, answerCount.Length);
                        nrOfGroupMembers = 0;
                    }
                    else
                    {
                        nrOfGroupMembers++;
                        foreach (var answer in entry)
                        {
                            answerCount[answer % 97]++;
                        }
                    }
                }

                // Add remaining.
                sum += answerCount.Count(x => x == nrOfGroupMembers);

                return sum;
            }
        }
    }
}
