using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Day17
{
    using Position = List<int>;

    class Program
    {
        class PositionComparer : IEqualityComparer<Position>
        {
            public bool Equals([AllowNull] Position x, [AllowNull] Position y)
            {
                return x.SequenceEqual(y);
            }

            public int GetHashCode([DisallowNull] Position obj)
            {
                var hash = new HashCode();

                foreach (var item in obj)
                {
                    hash.Add(item);
                }

                return hash.ToHashCode();
            }
        }
        
        static Position CreateInitialPosition(int x, int y, int dimension)
        {
            var result = new Position { x, y };

            while (result.Count < dimension)
            {
                result.Add(0);
            }

            return result;
        }

        static List<(int min, int max)> FindLimits(HashSet<Position> active, int dimension)
        {
            var result = new List<(int min, int max)>();

            for (int d = 0; d < dimension; d++)
            {
                var min = active.Min(p => p[d]);
                var max = active.Max(p => p[d]);

                result.Add((min, max));
            }

            return result;
        }

        static List<(int min, int max)> Grow(List<(int min, int max)> dimensions)
        {
            return dimensions.Select(d => (d.min - 1, d.max + 1)).ToList();
        }

        static IEnumerable<Position> AllPositions(List<(int min, int max)> limits)
        {
            if (limits.Count == 0)
            {
                yield return new Position();
                yield break;
            }

            var innerLimits = limits.Skip(1).ToList();
            var innerPositions = AllPositions(innerLimits).ToList();

            for (int v = limits[0].min; v <= limits[0].max; v++)
            {
                foreach (var inner in innerPositions)
                {
                    var position = new Position { v };
                    position.AddRange(inner);

                    yield return position;
                }
            }
        }

        static int ActiveNeighbours(HashSet<Position> active, Position position)
        {
            var result = 0;

            var limits = position.Select(v => (min: v - 1, max: v + 1)).ToList();

            foreach (var neighbour in AllPositions(limits))
            {
                if (neighbour.SequenceEqual(position))
                {
                    // ignore self
                    continue;
                }

                if (active.Contains(neighbour))
                {
                    result++;
                }
            }

            return result;
        }

        static int Simulate(string[] input, int dimension)
        {
            var active = new HashSet<Position>(new PositionComparer());

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] == '#')
                    {
                        active.Add(CreateInitialPosition(x, y, dimension));
                    }
                }
            }

            var limits = FindLimits(active, dimension);

            for (int cycle = 1; cycle <= 6; cycle++)
            {
                var next = new HashSet<Position>(new PositionComparer());

                limits = Grow(limits);

                foreach (var position in AllPositions(limits))
                {
                    var isActive = active.Contains(position);
                    var activeNeightbours = ActiveNeighbours(active, position);

                    if (isActive)
                    {
                        if (activeNeightbours == 2 || activeNeightbours == 3)
                        {
                            next.Add(position);
                        }
                    }
                    else
                    {
                        if (activeNeightbours == 3)
                        {
                            next.Add(position);
                        }
                    }
                }

                active = next;
            }

            return active.Count;
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var answer1 = Simulate(input, 3);
            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = Simulate(input, 4);
            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
