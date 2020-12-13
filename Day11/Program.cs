using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    class Field
    {
        private Dictionary<(int x, int y), bool> field;

        public int MaxX { get; init; }
        public int MaxY { get; init; }

        public List<(int x, int y)> SeatPositions => field.Keys.ToList();

        public void Occupy((int x, int y) position)
        {
            field[position] = true;
        }

        public void Leave((int x, int y) position)
        {
            field[position] = false;
        }

        public bool HasSeat((int x, int y) position)
        {
            return field.ContainsKey(position);
        }

        public bool IsOccupied((int x, int y) position)
        {
            if (HasSeat(position))
            {
                return field[position];
            }

            return false;
        }

        public int CountOccupied()
        {
           return field.Count(kv => kv.Value);
        }

        public bool Equals(Field other)
        {
            foreach (var kv in field)
            {
                var position = kv.Key;

                if (field[position] != other.field[position])
                {
                    return false;
                }
            }

            return true;
        }

        public Field Clone()
        {
            var result = new Field
            {
                MaxX = MaxX,
                MaxY = MaxY
            };

            result.field = new Dictionary<(int x, int y), bool>(field);

            return result;
        }

        public void Print()
        {
            for (int y = 0; y <= MaxY; y++)
            {
                for (int x = 0; x <= MaxX; x++)
                {
                    var position = (x, y);

                    if (field.ContainsKey(position))
                    {
                        Console.Write(field[position] ? '#' : 'L');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public static Field Parse(string[] input)
        {
            var field = new Dictionary<(int x, int y), bool>();

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] == 'L')
                    {
                        field[(x, y)] = false;
                    }
                }
            }

            var result = new Field
            {
                MaxX = field.Max(kv => kv.Key.x),
                MaxY = field.Max(kv => kv.Key.y)
            };

            result.field = field;

            return result;
        }
    }

    class Program
    {
        public static List<(int dx, int dy)> Directions = new List<(int dx, int dy)>
        {
            (-1, -1),
            (0, -1),
            (1, -1),
            (-1, 0),
            (1, 0),
            (-1, 1),
            (0, 1),
            (1, 1)
        };

        static int OccupiedNeighbours(Field field, (int x, int y) position)
        {
            return Directions.Count(d => field.IsOccupied((position.x + d.dx, position.y + d.dy)));
        }

        static bool SeesOccupied(Field field, (int x, int y) position, (int dx, int dy) direction)
        {
            int x = position.x;
            int y = position.y;

            while (0 <= x && x <= field.MaxX && 0 <= y && y <= field.MaxY)
            {
                x += direction.dx;
                y += direction.dy;

                if (field.HasSeat((x, y)))
                {
                    return field.IsOccupied((x, y));
                }
            }

            return false;
        }

        static int VisibleOccupiedNeighbours(Field field, (int x, int y) position)
        {
            return Directions.Count(d => SeesOccupied(field, position, d));
        }

        static Field Next(Field field, int limit, Func<Field, (int x, int y), int> neighbours)
        {
            var result = field.Clone();

            foreach (var position in field.SeatPositions)
            {
                var occupied = field.IsOccupied(position);
                var occupiedNeighbours = neighbours(field, position);

                if (!occupied && occupiedNeighbours == 0)
                {
                    result.Occupy(position);
                }
                else if (occupied && occupiedNeighbours >= limit)
                {
                    result.Leave(position);
                }
            }

            return result;
        }

        static Field Simulate(Field field, int limit, Func<Field, (int x, int y), int> neighbours)
        {
            var next = field;

            do
            {
                field = next;

                // field.Print();

                next = Next(field, limit, neighbours);
            } while (!field.Equals(next));

            return field;
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var field = Field.Parse(input);

            var result1 = Simulate(field, 4, OccupiedNeighbours);
            var answer1 = result1.CountOccupied();

            Console.WriteLine($"Answer 1: {answer1}");

            var result2 = Simulate(field, 5, VisibleOccupiedNeighbours);
            var answer2 = result2.CountOccupied();

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
