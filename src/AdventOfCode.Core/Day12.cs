using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode.Core
{
    public class Day12
    {
        public class Part1 : IProblem<long>
        {
            public virtual async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                return await SolveProblemAsync<Ship>(input);
            }

            protected async Task<long> SolveProblemAsync<TShip>(IAsyncEnumerable<string> input)
                where TShip : Ship, new()
            {
                var ship = new TShip();
                await foreach (var line in input)
                {
                    var action = line[0];
                    var value = int.Parse(line.Substring(1));

                    _ = action switch
                    {
                        'N' => ship.GoNorth(value),
                        'E' => ship.GoEast(value),
                        'S' => ship.GoSouth(value),
                        'W' => ship.GoWest(value),
                        'F' => ship.MoveForward(value),
                        'L' => ship.GoLeft(value),
                        'R' => ship.GoRight(value),
                        _ => InvalidOperation(),
                    };
                }

                return ship.GetManhattanDistance();
            }

            private bool InvalidOperation()
            {
                throw new NotImplementedException();
            }

            protected class Ship
            {
                protected (int north, int east) Position;
                private Direction _direction;

                public Ship()
                {
                    Position = new(0, 0);
                    _direction = Direction.East;
                }

                public virtual bool GoNorth(int unit)
                {
                    Position.north += unit;
                    return true;
                }

                public virtual bool GoEast(int unit)
                {
                    Position.east += unit;
                    return true;
                }

                public virtual bool GoSouth(int unit)
                {
                    Position.north -= unit;
                    return true;
                }

                public virtual bool GoWest(int unit)
                {
                    Position.east -= unit;
                    return true;
                }

                public virtual bool MoveForward(int unit)
                {
                    if (_direction == Direction.North)
                        Position.north += unit;
                    if (_direction == Direction.South)
                        Position.north -= unit;

                    if (_direction == Direction.East)
                        Position.east += unit;
                    if (_direction == Direction.West)
                        Position.east -= unit;

                    return true;
                }

                public virtual bool GoLeft(int unit)
                {
                    if (unit == 90)
                    {
                        // TODO: use enum flags?
                        _direction = _direction switch
                        {
                            Direction.North => Direction.West,
                            Direction.West => Direction.South,
                            Direction.South => Direction.East,
                            Direction.East => Direction.North,
                            _ => throw new NotImplementedException(),
                        };

                        return true;
                    }
                    else if (unit == 180)
                    {
                        GoLeft(90);
                        GoLeft(90);
                        return true;
                    }
                    else if (unit == 270)
                    {
                        GoLeft(90);
                        GoLeft(90);
                        GoLeft(90);
                        return true;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }

                public virtual bool GoRight(int unit)
                {
                    if (unit == 90)
                    {
                        // TODO: use enum flags?
                        _direction = _direction switch
                        {
                            Direction.North => Direction.East,
                            Direction.East => Direction.South,
                            Direction.South => Direction.West,
                            Direction.West => Direction.North,
                            _ => throw new NotImplementedException(),
                        };

                        return true;
                    }
                    else if (unit == 180)
                    {
                        GoRight(90);
                        GoRight(90);
                        return true;
                    }
                    else if (unit == 270)
                    {
                        GoRight(90);
                        GoRight(90);
                        GoRight(90);
                        return true;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }

                public long GetManhattanDistance()
                {
                    return Math.Abs(Position.north) + Math.Abs(Position.east);
                }

                private enum Direction
                {
                    North, East, South, West
                }
            }
        }

        public class Part2 : Part1
        {
            public override async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                return await SolveProblemAsync<ShipWithWaypoint>(input);
            }

            protected class ShipWithWaypoint : Ship
            {
                private (int north, int east) _waypoint;

                public ShipWithWaypoint()
                    : base()
                {
                    _waypoint = new(1, 10);
                }

                public override bool GoNorth(int unit)
                {
                    _waypoint.north += unit;
                    return true;
                }

                public override bool GoEast(int unit)
                {
                    _waypoint.east += unit;
                    return true;
                }

                public override bool GoSouth(int unit)
                {
                    _waypoint.north -= unit;
                    return true;
                }

                public override bool GoWest(int unit)
                {
                    _waypoint.east -= unit;
                    return true;
                }

                public override bool MoveForward(int unit)
                {
                    var north = _waypoint.north * unit;
                    var east = _waypoint.east * unit;

                    Position.north += north;
                    Position.east += east;

                    return true;
                }

                public override bool GoLeft(int unit)
                {
                    if (unit == 90)
                    {
                        // West (-) to South (-) or
                        // East (+) to North (+)
                        var newNorth = _waypoint.east;

                        // North (+) to West (-) or
                        // South (-) to East (+)
                        var newEast = _waypoint.north * -1;

                        _waypoint.north = newNorth;
                        _waypoint.east = newEast;

                        return true;
                    }
                    else
                    {
                        base.GoLeft(unit);
                        return true;
                    }
                }

                public override bool GoRight(int unit)
                {
                    if (unit == 90)
                    {
                        // West (-) to North (+) or
                        // East (+) to South (-)
                        var newNorth = _waypoint.east * -1;

                        // North (+) to East (+) or
                        // South (-) to West (-)
                        var newEast = _waypoint.north;

                        _waypoint.north = newNorth;
                        _waypoint.east = newEast;

                        return true;
                    }
                    else
                    {
                        base.GoRight(unit);
                        return true;
                    }
                }
            }
        }
    }
}
