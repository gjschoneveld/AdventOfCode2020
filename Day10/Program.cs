using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {
        static long Multiply(List<long> values)
        {
            return values.Aggregate((a, b) => a * b);
        }

        static List<int> RunsLengths(List<int> values, int target)
        {
            var lengths = new List<int>();

            var currentLength = 0;

            foreach (var value in values)
            {
                if (value == target)
                {
                    currentLength++;
                }
                else if (currentLength > 0)
                {
                    lengths.Add(currentLength);
                    currentLength = 0;
                }
            }

            if (currentLength > 0)
            {
                // for completeness; will not happen on this day
                lengths.Add(currentLength);
            }

            return lengths;
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var parsed = input.Select(int.Parse).ToList();

            var from = parsed.Prepend(0).OrderBy(x => x).ToList();
            var to = parsed.Append(parsed.Max() + 3).OrderBy(x => x).ToList();

            var differences = from.Zip(to, (f, t) => t - f).ToList();

            var ones = differences.Count(d => d == 1);
            var threes = differences.Count(d => d == 3);

            var answer1 = ones * threes;

            Console.WriteLine($"Answer 1:{answer1}");

            var runLengths = RunsLengths(differences, 1);

            var lengthToPossibilities = new List<long> { 1, 1, 2, 4, 7 };
            var possibilities = runLengths.Select(len => lengthToPossibilities[len]).ToList();

            var answer2 = Multiply(possibilities);

            Console.WriteLine($"Answer 2:{answer2}");
        }
    }
}
