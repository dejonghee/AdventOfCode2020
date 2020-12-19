using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day14 : Day<long, long>
    {
        protected override IProblem<long> GetPart1() => new Part1();
        protected override IProblem<long> GetPart2() => new Part2();

        private static readonly Regex InputRegexParser = new Regex(@"^mem\[([0-9]+)\] = ([0-9]+)$", RegexOptions.Compiled);

        private class Part1 : IProblem<long>
        {
            public async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var parsedInput = ParseInputAsync(input);
                return await SolveProblemAsync(parsedInput);
            }

            protected virtual async Task<long> SolveProblemAsync(IAsyncEnumerable<(string mask, (int index, long value)[])> input)
            {
                var data = new Dictionary<int, long>();

                await foreach (var set in input)
                {
                    (string mask, var entries) = set;
                    foreach ((int index, long value) in entries)
                    {
                        data[index] = ApplyMaskToValue(value, mask);
                    }
                }

                return data.Values.Sum();
            }

            protected async IAsyncEnumerable<(string mask, (int index, long value)[])> ParseInputAsync(IAsyncEnumerable<string> input)
            {
                var enumerator = input.GetAsyncEnumerator();

                var tmpMask = string.Empty;
                var tmpEntries = new List<(int index, long value)>();

                while (await enumerator.MoveNextAsync())
                {
                    if (enumerator.Current.StartsWith("mask"))
                    {
                        // Return previous record if we have one.
                        if (!string.IsNullOrEmpty(tmpMask))
                            yield return (tmpMask, tmpEntries.ToArray());

                        // Reset state & start processing of next item.
                        tmpEntries.Clear();
                        tmpMask = GetMask(enumerator.Current);
                    }
                    else
                    {
                        var entry = enumerator.Current;
                        var regexMatch = InputRegexParser.Match(entry);
                        if (!regexMatch.Success)
                            throw new Exception("Invalid input: " + entry);

                        var index = regexMatch.Groups[1].Value;
                        var value = regexMatch.Groups[2].Value;

                        tmpEntries.Add((int.Parse(index), long.Parse(value)));
                    }
                }

                // return last item.
                yield return (tmpMask, tmpEntries.ToArray());
            }

            protected long ApplyMaskToValue(long value, string mask)
            {
                var bitValue = ApplyMask(value, mask);
                return Convert.ToInt64(new string(bitValue), 2);
            }

            protected virtual char[] ApplyMask(long value, string mask)
            {
                var bitValue = Convert.ToString(value, 2)
                    .PadLeft(mask.Length, '0')
                    .ToCharArray();

                for (var index = mask.Length - 1; index >= 0; index--)
                {
                    if (mask[index] != 'X')
                    {
                        bitValue[index] = mask[index];
                    }
                }

                return bitValue;
            }

            private static string GetMask(string line)
            {
                return line.Split("=", StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            }
        }

        private class Part2 : Part1
        {
            protected override async Task<long> SolveProblemAsync(IAsyncEnumerable<(string mask, (int index, long value)[])> input)
            {
                var data = new Dictionary<long, long>();

                await foreach (var i in input)
                {
                    (string mask, var entries) = i;
                    foreach ((int index, long value) in entries)
                    {
                        foreach (var patchedIndex in ApplyMaskToIndex(index, mask))
                        {
                            data[patchedIndex] = value;
                        }
                    }
                }

                return data.Values.Sum();
            }

            private IEnumerable<long> ApplyMaskToIndex(int index, string mask)
            {
                var floatingIndex = ApplyMask((long)index, mask);
                return CalculateIndex(floatingIndex);
            }

            private static IEnumerable<long> CalculateIndex(char[] floatingIndex)
            {
                var index = Array.IndexOf(floatingIndex, 'X');
                if (index == -1)
                {
                    yield return Convert.ToInt64(new string(floatingIndex), 2);
                }
                else
                {
                    var bit1 = new char[floatingIndex.Length];
                    Array.Copy(floatingIndex, bit1, floatingIndex.Length);
                    bit1[index] = '0';

                    foreach (var idx in CalculateIndex(bit1))
                        yield return idx;

                    var bit2 = new char[floatingIndex.Length];
                    Array.Copy(floatingIndex, bit2, floatingIndex.Length);
                    bit2[index] = '1';

                    foreach (var idx in CalculateIndex(bit2))
                        yield return idx;
                }
            }

            protected override char[] ApplyMask(long value, string mask)
            {
                var bitValue = Convert.ToString(value, 2)
                    .PadLeft(mask.Length, '0')
                    .ToCharArray();

                for (var index = mask.Length - 1; index >= 0; index--)
                {
                    bitValue[index] = mask[index] switch
                    {
                        '0' => bitValue[index],
                        '1' => '1',
                        'X' => 'X',
                        _ => throw new Exception("Invalid input")
                    };
                }

                return bitValue;
            }
        }
    }
}
