using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day20 : Day<long, long>
    {
        protected override IProblem<long> GetPart1() => new Part1();

        protected override IProblem<long> GetPart2() => new Part2();

        private class Part1 : IProblem<long>
        {
            public virtual async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var tiles = await GetLinkedTilesAsync(input);

                return tiles
                    .Where(tile => tile.IsCorner())
                    .Select(tile => tile.Id)
                    .Aggregate((x, y) => x * y);
            }

            protected async Task<Tile[]> GetLinkedTilesAsync(IAsyncEnumerable<string> input)
            {
                var tiles = await Parse(input);

                foreach (var tile in tiles)
                {
                    tile.FindMissingNeighbours(tiles);
                }

                return tiles;
            }

            private async Task<Tile[]> Parse(IAsyncEnumerable<string> input)
            {
                var result = new List<Tile>();

                var currentId = -1L;
                var currentImage = default(char[,]);
                var lineId = 0;

                await foreach (var line in input)
                {
                    currentImage ??= new char[line.Length, line.Length];

                    if (line.StartsWith("Tile"))
                    {
                        currentId = long.Parse(line.Split(' ')[1].TrimEnd(':'));
                    }
                    else if (string.IsNullOrEmpty(line))
                    {
                        result.Add(new Tile(currentId, currentImage));

                        // Reset state.
                        currentId = -1;
                        currentImage = null;
                        lineId = 0;
                    }
                    else
                    {
                        for (var index = 0; index < line.Length; index++)
                        {
                            currentImage[lineId, index] = line[index];
                        }

                        lineId++;
                    }
                }

                result.Add(new Tile(currentId, currentImage));

                return result.ToArray();
            }

            protected class Tile
            {
                private readonly string _hashTop;
                private readonly string _hashTopReverse;
                private readonly string _hashLeft;
                private readonly string _hashLeftReverse;
                private readonly string _hashRight;
                private readonly string _hashRightReverse;
                private readonly string _hashBottom;
                private readonly string _hashBottomReverse;

                public long Id { get; }
                public char[,] Image { get; }

                public Tile NeighbourTop { get; set; }
                public Tile NeighbourLeft { get; set; }
                public Tile NeighbourRight { get; set; }
                public Tile NeighbourBottom { get; set; }

                public Tile(long id, char[,] image)
                {
                    Id = id;
                    Image = image;

                    _hashTop = Enumerable
                        .Range(0, Image.GetLength(0))
                        .Select(x => $"({x}-{Image[0, x]})")
                        .Aggregate((x, y) => x + y);
                    _hashTopReverse = Enumerable
                        .Range(0, Image.GetLength(0))
                        .Select(x => $"({x}-{Image[0, Image.GetLength(0) - 1 - x]})")
                        .Aggregate((x, y) => x + y);

                    _hashLeft = Enumerable
                        .Range(0, Image.GetLength(1))
                        .Select(y => $"({y}-{Image[y, 0]})")
                        .Aggregate((x, y) => x + y);
                    _hashLeftReverse = Enumerable
                        .Range(0, Image.GetLength(1))
                        .Select(y => $"({y}-{Image[Image.GetLength(1) - 1 - y, 0]})")
                        .Aggregate((x, y) => x + y);

                    _hashRight = Enumerable
                        .Range(0, Image.GetLength(1))
                        .Select(y => $"({y}-{Image[y, Image.GetLength(0) - 1]})")
                        .Aggregate((x, y) => x + y);
                    _hashRightReverse = Enumerable
                        .Range(0, Image.GetLength(1))
                        .Select(y => $"({y}-{Image[Image.GetLength(1) - 1 - y, Image.GetLength(0) - 1]})")
                        .Aggregate((x, y) => x + y);

                    _hashBottom = Enumerable
                        .Range(0, Image.GetLength(0))
                        .Select(x => $"({x}-{Image[Image.GetLength(1) - 1, x]})")
                        .Aggregate((x, y) => x + y);
                    _hashBottomReverse = Enumerable
                        .Range(0, Image.GetLength(0))
                        .Select(x => $"({x}-{Image[Image.GetLength(1) - 1, Image.GetLength(0) - 1 - x]})")
                        .Aggregate((x, y) => x + y);
                }

                public void FindMissingNeighbours(Tile[] tiles)
                {
                    if (NeighbourTop == null)
                        FindNeighbour(tiles, Direction.Top);

                    if (NeighbourLeft == null)
                        FindNeighbour(tiles, Direction.Left);

                    if (NeighbourRight == null)
                        FindNeighbour(tiles, Direction.Right);

                    if (NeighbourBottom == null)
                        FindNeighbour(tiles, Direction.Bottom);
                }

                public bool Match(string hash, out Direction direction)
                {
                    if (string.Equals(_hashTop, hash) || string.Equals(_hashTopReverse, hash))
                    {
                        direction = Direction.Top;
                        return true;
                    }

                    if (string.Equals(_hashLeft, hash) || string.Equals(_hashLeftReverse, hash))
                    {
                        direction = Direction.Left;
                        return true;
                    }

                    if (string.Equals(_hashRight, hash) || string.Equals(_hashRightReverse, hash))
                    {
                        direction = Direction.Right;
                        return true;
                    }

                    if (string.Equals(_hashBottom, hash) || string.Equals(_hashBottomReverse, hash))
                    {
                        direction = Direction.Bottom;
                        return true;
                    }

                    direction = Direction.None;
                    return false;
                }

                public bool IsCorner()
                {
                    var neighbours = CountNeighbours();

                    return (neighbours == 2 && NeighbourTop != null && NeighbourLeft != null) ||
                           (neighbours == 2 && NeighbourTop != null && NeighbourRight != null) ||
                           (neighbours == 2 && NeighbourBottom != null && NeighbourLeft != null) ||
                           (neighbours == 2 && NeighbourBottom != null && NeighbourRight != null);
                }

                private void FindNeighbour(Tile[] tiles, Direction directionToExplore)
                {
                    var hash = directionToExplore switch
                    {
                        Direction.Top => _hashTop,
                        Direction.Left => _hashLeft,
                        Direction.Right => _hashRight,
                        Direction.Bottom => _hashBottom,
                        _ => throw new Exception("Invalid direction")
                    };

                    var match = tiles
                        .Where(tile => Id != tile.Id)
                        .Select(tile =>
                        {
                            var hasMatch = tile.Match(hash, out var direction);
                            return (hasMatch, direction, tile);
                        })
                        .SingleOrDefault(x => x.hasMatch);

                    if (match == default)
                        return;

                    if (directionToExplore == Direction.Top)
                        NeighbourTop = match.tile;
                    else if (directionToExplore == Direction.Left)
                        NeighbourLeft = match.tile;
                    else if (directionToExplore == Direction.Right)
                        NeighbourRight = match.tile;
                    else if (directionToExplore == Direction.Bottom)
                        NeighbourBottom = match.tile;

                    if (match.direction == Direction.Top)
                        match.tile.NeighbourTop = this;
                    else if (match.direction == Direction.Left)
                        match.tile.NeighbourLeft = this;
                    else if (match.direction == Direction.Right)
                        match.tile.NeighbourRight = this;
                    else if (match.direction == Direction.Bottom)
                        match.tile.NeighbourBottom = this;
                }

                private int CountNeighbours()
                {
                    var count = 0;

                    if (NeighbourTop != null)
                        count++;
                    if (NeighbourLeft != null)
                        count++;
                    if (NeighbourRight != null)
                        count++;
                    if (NeighbourBottom != null)
                        count++;

                    return count;
                }
            }

            protected enum Direction
            {
                None,
                Top,
                Left,
                Right,
                Bottom
            }
        }

        private class Part2 : Part1
        {
            public override async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var tiles = await GetLinkedTilesAsync(input);

                // TODO: solve Part II.

                return -1;
            }
        }
    }
}
