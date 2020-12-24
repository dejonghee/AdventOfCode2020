using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day24 : Day<int, int>
    {
        protected override IProblem<int> GetPart1() => new Part1();

        protected override IProblem<int> GetPart2() => new Part2();

        private class Part1 : IProblem<int>
        {
            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var blackTiles = new HashSet<(int x, int y)>();

                await foreach (var tile in Parser.GetTilesAsync(input))
                {
                    if (blackTiles.Contains(tile))
                        blackTiles.Remove(tile);
                    else
                        blackTiles.Add(tile);
                }

                return blackTiles.Count;
            }
        }

        private class Part2 : IProblem<int>
        {
            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                // Boolean true means tile is black.
                var floor = new Dictionary<(int x, int y), bool>();

                await foreach (var tile in Parser.GetTilesAsync(input))
                {
                    floor[tile] = floor.TryGetValue(tile, out var isBlack)
                        ? !isBlack
                        : true;
                }

                for (var day = 1; day <= 100; day++)
                {
                    floor = FlipTiles(floor);
                }

                return floor.Values.Count(isBlack => isBlack == true);
            }

            private Dictionary<(int x, int y), bool> FlipTiles(Dictionary<(int x, int y), bool> state)
            {
                var nextState = new Dictionary<(int x, int y), bool>(state.Count);
                var tilesToExplore = new HashSet<(int x, int y)>();

                // Loop over all known tiles and flip when needed.
                foreach (((int x, int y) tile, bool isBlack) in state)
                {
                    nextState[tile] = ShouldFlip(state, tile, isBlack, out var newTiles)
                        ? !isBlack
                        : isBlack;

                    tilesToExplore.TryAddRange(newTiles);
                }

                // Loop over all new tiles we touched (they are all white) check if we need to flip them.
                foreach (var tile in tilesToExplore)
                {
                    // If shouldflip is true, flip to black. Else stay white.
                    nextState[tile] = ShouldFlip(state, tile, isBlack: false, out _) == true;
                }

                return nextState;
            }

            private static (int x, int y)[] GetAdjacentTiles((int x, int y) position)
            {
                (int x, int y) = position;

                return new[]
                {
                    (x, y + 1),     // NE
                    (x + 1, y),     // E
                    (x + 1, y - 1), // SE
                    (x, y - 1),     // SW
                    (x - 1, y),     // W
                    (x - 1, y + 1)  // NW
                };
            }

            private static bool ShouldFlip(IDictionary<(int x, int y), bool> state, (int x, int y) position, bool isBlack, out (int x, int y)[] newTiles)
            {
                var tilesToExplore = new List<(int x, int y)>();

                if (isBlack)
                {
                    // Any black tile with zero or more than 2 black tiles immediately adjacent to it is flipped to white.
                    var numberOfBlackAdjacentTiles = GetAdjacentTiles(position)
                        .Select(tile =>
                        {
                            if (state.TryGetValue(tile, out var isBlack))
                                return isBlack;

                            tilesToExplore.Add(tile);
                            return false;
                        })
                        .Count(isBlack => isBlack == true);

                    newTiles = tilesToExplore.ToArray();
                    return numberOfBlackAdjacentTiles == 0 || numberOfBlackAdjacentTiles > 2
                        ? true     // Flip to white.
                        : false;   // Stay black.
                }
                else
                {
                    // Any white tile with exactly 2 black tiles immediately adjacent to it is flipped to black.
                    var numberOfBlackAdjacentTiles = GetAdjacentTiles(position)
                        .Select(pos =>
                        {
                            if (state.TryGetValue(pos, out var isBlack))
                                return isBlack;

                            tilesToExplore.Add(pos);
                            return false;
                        })
                        .Count(isBlack => isBlack == true);

                    newTiles = tilesToExplore.ToArray();
                    return numberOfBlackAdjacentTiles == 2
                        ? true      // Flip to black
                        : false;    // Stay white.
                }
            }
        }

        private static class Parser
        {
            public static async IAsyncEnumerable<(int x, int y)> GetTilesAsync(IAsyncEnumerable<string> input)
            {
                await foreach (var entry in ParseAsync(input))
                {
                    var position = (0, 0);

                    foreach (var direction in entry)
                    {
                        position = Move(position, direction);
                    }

                    yield return position;
                }
            }

            private static async IAsyncEnumerable<string[]> ParseAsync(IAsyncEnumerable<string> input)
            {
                await foreach (var line in input)
                {
                    var result = new List<string>();

                    var index = 0;
                    while (index < line.Length)
                    {
                        if (index + 1 < line.Length && IsKnownInstruction(line[index], line[index + 1]))
                        {
                            result.Add($"{line[index]}{line[index + 1]}");
                            index += 2;
                        }
                        else if (IsKnownInstruction(line[index]))
                        {
                            result.Add($"{line[index]}");
                            index += 1;
                        }
                    }

                    yield return result.ToArray();
                }
            }

            private static bool IsKnownInstruction(char char1, char char2)
            {
                return (char1 == 'n' && char2 == 'e') ||
                       (char1 == 's' && char2 == 'e') ||
                       (char1 == 's' && char2 == 'w') ||
                       (char1 == 'n' && char2 == 'w');
            }

            private static bool IsKnownInstruction(char char1)
            {
                return char1 == 'e' || char1 == 'w';
            }

            private static (int x, int y) Move((int x, int y) position, string direction)
            {
                (int x, int y) = position;

                return direction switch
                {
                    "ne" => (x, y + 1),
                    "e" => (x + 1, y),
                    "se" => (x + 1, y - 1),
                    "sw" => (x, y - 1),
                    "w" => (x - 1, y),
                    "nw" => (x - 1, y + 1),
                    _ => throw new Exception("Unknown direction")
                };
            }
        }
    }
}
