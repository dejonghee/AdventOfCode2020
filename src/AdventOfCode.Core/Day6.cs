using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Core
{
    public class Day6
    {
        public class Part1 : IProblem<long>
        {
            public async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var sum = 0L;

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

        public class Part2 : IProblem<long>
        {
            public async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var sum = 0L;

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
