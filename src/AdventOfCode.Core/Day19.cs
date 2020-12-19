using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day19 : Day<int, int>
    {
        protected override IProblem<int> GetPart1() => new Part1();
        protected override IProblem<int> GetPart2() => new Part2();

        private class Part1 : IProblem<int>
        {
            public virtual async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var parsedRules = new Dictionary<int, Lazy<List<string>>>();
                var firstRule = new Lazy<HashSet<string>>(() => new HashSet<string>(parsedRules[0].Value));

                var result = 0;

                var mode = Mode.ParseRules;
                await foreach (var line in input)
                {
                    // Switch mode when we hit an empty line.
                    if (mode == Mode.ParseRules && string.IsNullOrEmpty(line))
                    {
                        mode = Mode.ValidateMessages;
                        continue;
                    }

                    if (mode == Mode.ParseRules)
                    {
                        Parse(line, parsedRules);
                    }
                    else
                    {
                        result += firstRule.Value.Contains(line)
                            ? 1
                            : 0;
                    }
                }

                return result;
            }

            protected static readonly Regex HasNoDependencies = new Regex("([0-9]+): \"([a-z])\"", RegexOptions.Compiled);

            protected void Parse(string input, IDictionary<int, Lazy<List<string>>> rules)
            {
                var result = HasNoDependencies.Match(input);
                if (result.Success)
                {
                    var id = int.Parse(result.Groups[1].Value);
                    var str = result.Groups[2].Value;
                    rules[id] = new Lazy<List<string>>(new List<string>() { str });
                }
                else
                {
                    var parts = input.Split(":", StringSplitOptions.RemoveEmptyEntries);
                    var id = int.Parse(parts[0]);
                    var rulesToParse = parts[1];

                    rules[id] = new Lazy<List<string>>(() =>
                    {
                        return ParseRule(id, rulesToParse, rules);
                    });
                }
            }

            protected virtual List<string> ParseRule(int id, string rulesToParse, IDictionary<int, Lazy<List<string>>> rules)
            {
                return rulesToParse
                    .Split("|", StringSplitOptions.RemoveEmptyEntries)
                    .SelectMany(pattern => pattern
                        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => int.Parse(x))
                        .Select(x => rules[x].Value)
                        .Aggregate((current, next) =>
                        {
                            var result = new List<string>();
                            foreach (var first in current)
                            {
                                foreach (var second in next)
                                {
                                    result.Add($"{first}{second}");
                                }
                            }
                            return result;
                        })
                    )
                    .ToList();
            }

            protected enum Mode
            {
                ParseRules,
                ValidateMessages
            }
        }

        private class Part2 : Part1
        {
            public override async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var parsedRules = new Dictionary<int, Lazy<List<string>>>();
                var rule0 = new Lazy<HashSet<string>>(() => new HashSet<string>(parsedRules[0].Value));
                var rule31 = new Lazy<HashSet<string>>(() => new HashSet<string>(parsedRules[31].Value));
                var rule42 = new Lazy<HashSet<string>>(() => new HashSet<string>(parsedRules[42].Value));

                var result = 0;

                var mode = Mode.ParseRules;
                await foreach (var line in input)
                {
                    // Switch mode when we hit an empty line.
                    if (mode == Mode.ParseRules && string.IsNullOrEmpty(line))
                    {
                        mode = Mode.ValidateMessages;
                        continue;
                    }

                    if (mode == Mode.ParseRules)
                    {
                        Parse(line, parsedRules);
                    }
                    else
                    {
                        if (rule0.Value.Contains(line) ||
                            DoesMatchRecursivePattern(line, rule0.Value, rule31.Value, rule42.Value))
                        {
                            result++;
                        }
                    }
                }

                return result;
            }

            private bool DoesMatchRecursivePattern(string input, HashSet<string> rule0, HashSet<string> rule31, HashSet<string> rule42)
            {
                // Support recursive rule 8.
                // If a prefix of the input matches rule 8, remove and continue with remaining input.
                // Since rule 8 equals rule 42, use 42 for simplicity.

                // Keep track of number of times we have a prefix match. We need it later on.
                var matches42 = 0;

                while (input.Length > 0 && rule0.Contains(input) == false)
                {
                    var prefix = rule42.SingleOrDefault(prefix => input.StartsWith(prefix));
                    if(string.IsNullOrEmpty(prefix) == false)
                    {
                        input = input.Substring(prefix.Length);
                        matches42++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (rule0.Contains(input))
                    return true;

                if (matches42 == 0 || input.Length == 0)
                    return false;

                // Support recursive rule 11.
                // If a prefix of the input matches rule 31, remove and continue with remaining input.

                // Keep track of number of times we have a prefix match. We need it to validate the result.
                var matches31 = 0;

                while (input.Length > 0 && rule31.Any(x => input.StartsWith(x)))
                {
                    var prefix = rule31.Single(prefix => input.StartsWith(prefix));
                    input = input.Substring(prefix.Length);
                    matches31++;
                }

                // Validate if we have a match.
                return input.Length == 0 &&         // Entire string needs to match
                       matches31 > 0 &&             // Rule 11 contains at least one occurence of rule 31
                       matches31 <= matches42 - 1;  // Rule 0 = rule 8 followed by rule 11.
            }
        }
    }
}
