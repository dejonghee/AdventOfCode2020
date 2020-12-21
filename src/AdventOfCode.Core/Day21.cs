using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day21 : Day<int, int>
    {
        protected override IProblem<int> GetPart1() => new Part1();

        protected override IProblem<int> GetPart2()
        {
            throw new NotImplementedException();
        }

        private class Part1 : IProblem<int>
        {
            private readonly static Regex InputParser = new Regex(@"(([a-z]+) )+\(contains (([a-z]+)(, )?)+\)", RegexOptions.Compiled);

            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var allergensMapping = new Dictionary<string, List<string>>();
                var allIngredients = new List<string>();

                await foreach(var line in input)
                {
                    var match = InputParser.Match(line);
                    if(match.Success)
                    {
                        var ingredients = match.Groups[2].Captures.Select(x => x.Value).ToList();
                        var allergens = match.Groups[4].Captures.Select(x => x.Value);

                        // Update allergens mapping.
                        foreach(var allergen in allergens)
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

                // Set of ingredients which contain allergens.
                var ingredientsWithAllergens = new HashSet<string>();

                while(allergensMapping.Any(x => x.Value.Count > 0))
                {
                    var mapped = allergensMapping.First(x => x.Value.Count == 1);
                    ingredientsWithAllergens.Add(mapped.Value.Single());

                    foreach(var entry in allergensMapping)
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
    }
}
