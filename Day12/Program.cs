using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
{
    class Position
    {
        public static readonly (int dx, int dy) North = (0, -1);
        public static readonly (int dx, int dy) South = (0, 1);
        public static readonly (int dx, int dy) East = (1, 0);
        public static readonly (int dx, int dy) West = (-1, 0);

        public int X { get; set; }
        public int Y { get; set; }

        public int Distance => Math.Abs(X) + Math.Abs(Y);

        public void Move((int dx, int dy) direction, int steps)
        {
            X += direction.dx * steps;
            Y += direction.dy * steps;
        }
    }

    class Waypoint : Position
    {
        private void RotateLeft()
        {
            (X, Y) = (Y, -X);
        }

        public void RotateLeft(int degrees)
        {
            for (int i = 0; i < degrees / 90; i++)
            {
                RotateLeft();
            }
        }

        public void RotateRight(int degrees)
        {
            RotateLeft(360 - degrees);
        }
    }

    class Program
    {
        static (char command, int value) Parse(string x)
        {
            return (x[0], int.Parse(x.Substring(1)));
        }

        static void Move(Position ship, Waypoint waypoint, Position target, List<(char command, int value)> lines)
        {
            foreach (var line in lines)
            {
                switch (line.command)
                {
                    case 'N':
                        target.Move(Position.North, line.value);
                        break;
                    case 'S':
                        target.Move(Position.South, line.value);
                        break;
                    case 'E':
                        target.Move(Position.East, line.value);
                        break;
                    case 'W':
                        target.Move(Position.West, line.value);
                        break;
                    case 'L':
                        waypoint.RotateLeft(line.value);
                        break;
                    case 'R':
                        waypoint.RotateRight(line.value);
                        break;
                    case 'F':
                        ship.Move((waypoint.X, waypoint.Y), line.value);
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var parsed = input.Select(Parse).ToList();

            var ship1 = new Position();
            var waypoint1 = new Waypoint { X = Position.East.dx, Y = Position.East.dy };

            Move(ship1, waypoint1, ship1, parsed);

            var answer1 = ship1.Distance;

            Console.WriteLine($"Answer 1: {answer1}");


            var ship2 = new Position();
            var waypoint2 = new Waypoint { X = 10, Y = -1 };

            Move(ship2, waypoint2, waypoint2, parsed);

            var answer2 = ship2.Distance;

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
