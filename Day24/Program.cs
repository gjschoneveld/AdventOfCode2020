using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day24
{
    class Program
    {
        static List<string> Tokenize(string x)
        {
            var result = new List<string>();

            var index = 0;

            while (index < x.Length)
            {
                if (x[index] == 'n' || x[index] == 's')
                {
                    result.Add(x.Substring(index, 2));
                    index += 2;

                    continue;
                }

                result.Add(x.Substring(index, 1));
                index++;
            }

            return result;
        }

        static (int dx, int dy) DirectionToChange(string direction)
        {
            return direction switch
            {
                "e" => (1, 0),
                "se" => (0, 1),
                "sw" => (-1, 1),
                "w" => (-1, 0),
                "nw" => (0, -1),
                "ne" => (1, -1),
                _ => throw new Exception("Unknown direction")
            };
        }

        static (int x, int y) Move((int x, int y) position, string direction)
        {
            var change = DirectionToChange(direction);

            return (position.x + change.dx, position.y + change.dy);
        }

        static int BlackNeighbours(HashSet<(int x, int y)> black, (int x, int y) position)
        {
            var directions = new List<string> { "e", "se", "sw", "w", "nw", "ne" };

            return directions
                .Select(d => Move(position, d))
                .Count(nb => black.Contains(nb));
        }

        static IEnumerable<(int x, int y)> GeneratePositions(int max)
        {
            for (int x = -max; x <= max; x++)
            {
                for (int y = -max; y <= max; y++)
                {
                    yield return (x, y);
                }
            }
        }

        static void Flip(HashSet<(int x, int y)> black, (int x, int y) position)
        {
            if (black.Contains(position))
            {
                black.Remove(position);
                return;
            }

            black.Add(position);
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var parsed = input.Select(Tokenize).ToList();

            var black = new HashSet<(int x, int y)>();

            foreach (var line in parsed)
            {
                var position = line.Aggregate((x: 0, y: 0), Move);

                Flip(black, position);
            }

            var answer1 = black.Count;

            Console.WriteLine($"Answer 1: {answer1}");


            var days = 100;

            var extremes = new List<int> { 
                black.Max(t => t.x), 
                black.Max(t => t.y), 
                -black.Min(t => t.x),
                -black.Min(t => t.y)};

            var max = extremes.Max() + 1;

            for (int day = 1; day <= days; day++)
            {
                var candidates = GeneratePositions(max);
                max++;

                var flipped = candidates.Where(p => {
                    var isBlack = black.Contains(p);
                    var blackNeighbours = BlackNeighbours(black, p);

                    return (isBlack && (blackNeighbours == 0 || blackNeighbours > 2)) ||
                        (!isBlack && blackNeighbours == 2);
                }).ToList();

                foreach (var position in flipped)
                {
                    Flip(black, position);
                }
            }

            var answer2 = black.Count;

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
