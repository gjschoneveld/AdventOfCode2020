using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03
{
    class Program
    {
        static bool HasTree(string[] input, int x, int y)
        {
            var row = input[y];

            return row[x % row.Length] == '#';
        }

        static int CountTrees(string[] input, int deltaX, int deltaY)
        {
            int x = 0;
            int y = 0;

            int trees = 0;

            while (y < input.Length)
            {
                if (HasTree(input, x, y))
                {
                    trees++;
                }

                x += deltaX;
                y += deltaY;
            }

            return trees;
        }

        public static long Multiply(List<int> values)
        {
            return values.Select(x => (long)x).Aggregate((a, b) => a * b);
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var answer1 = CountTrees(input, 3, 1);

            Console.WriteLine($"Answer 1: {answer1}");

            var slopes = new List<(int dx, int dy)>
            {
                (1, 1),
                (3, 1),
                (5, 1),
                (7, 1),
                (1, 2)
            };

            var trees = slopes.Select(s => CountTrees(input, s.dx, s.dy)).ToList();

            var answer2 = Multiply(trees);

            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}
