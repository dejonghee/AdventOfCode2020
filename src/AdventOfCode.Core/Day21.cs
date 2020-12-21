using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day21 : Day<int, string>
    {
        protected override IProblem<int> GetPart1() => new Part1();

        protected override IProblem<string> GetPart2() => new Part2();

        private class Part1 : IProblem<int>
        {
            public virtual async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                (var allergensMapping, var allIngredients) = await Parser.ParseDataAsync(input);

                // Set of ingredients which contain allergens.
                var ingredientsWithAllergens = new HashSet<string>();

                while (allergensMapping.Any(x => x.Value.Count > 0))
                {
                    var mapped = allergensMapping.First(x => x.Value.Count == 1);
                    ingredientsWithAllergens.Add(mapped.Value.Single());

                    foreach (var entry in allergensMapping)
                    {
                        allergensMapping[entry.Key] = entry.Value.Except(mapped.Value).ToList();
                    }
                }

                return allIngredients
                    .GroupBy(x => x)
                    .Where(x => !ingredientsWithAllergens.Contains(x.Key))
                    .Select(x => x.Count())
                    .Sum();
            }
        }

        private class Part2 : IProblem<string>
        {
            public async Task<string> SolveAsync(IAsyncEnumerable<string> input)
            {
                (var allergensMapping, var allIngredients) = await Parser.ParseDataAsync(input);

                // List of ingredient with the allergen it contains.
                var ingredientsWithAllergens = new Dictionary<string, string>();

                while (allergensMapping.Any(x => x.Value.Count > 0))
                {
                    var mapped = allergensMapping.First(x => x.Value.Count == 1);
                    ingredientsWithAllergens[mapped.Value.Single()] = mapped.Key;

                    foreach (var entry in allergensMapping)
                    {
                        allergensMapping[entry.Key] = entry.Value.Except(mapped.Value).ToList();
                    }
                }

                return ingredientsWithAllergens
                    .OrderBy(x => x.Value)
                    .Select(x => x.Key)
                    .Aggregate((x, y) => $"{x},{y}");
            }
        }

        private static class Parser
        {
            private readonly static Regex InputParser = new Regex(@"(([a-z]+) )+\(contains (([a-z]+)(, )?)+\)", RegexOptions.Compiled);

            public static async Task<(Dictionary<string, List<string>> allergensMapping, List<string> allIngredients)> ParseDataAsync(IAsyncEnumerable<string> input)
            {
                var allergensMapping = new Dictionary<string, List<string>>();
                var allIngredients = new List<string>();

                await foreach (var line in input)
                {
                    var match = InputParser.Match(line);
                    if (match.Success)
                    {
                        var ingredients = match.Groups[2].Captures.Select(x => x.Value).ToList();
                        var allergens = match.Groups[4].Captures.Select(x => x.Value);

                        // Update allergens mapping.
                        foreach (var allergen in allergens)
                        {
                            allergensMapping[allergen] = allergensMapping.TryGetValue(allergen, out var tmp)
                                ? tmp.Intersect(ingredients).ToList()
                                : ingredients;
                        }

                        // Track all ingredients.
                        allIngredients.AddRange(ingredients);
                    }
                    else
                    {
                        throw new Exception("Line does not match.");
                    }
                }

                return (allergensMapping, allIngredients);
            }
        }
    }
}
