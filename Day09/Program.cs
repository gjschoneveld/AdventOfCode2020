using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day09
{
    class Program
    {
        public static List<long> FindCombination(long requiredSum, List<long> choices, int picks)
        {
            if (picks == 1)
            {
                if (choices.Contains(requiredSum))
                {
                    return new List<long> { requiredSum };
                }

                return null;
            }

            foreach (var choice in choices)
            {
                var innerSum = requiredSum - choice;
                var innerPicks = picks - 1;

                var innerChoices = new List<long>(choices);
                innerChoices.Remove(choice);

                var innerCombination = FindCombination(innerSum, innerChoices, innerPicks);

                if (innerCombination != null)
                {
                    innerCombination.Add(choice);
                    return innerCombination;
                }
            }

            return null;
        }

        static bool IsValid(List<long> numbers, int length, int index)
        {
            var choices = numbers.GetRange(index - length, length);

            return FindCombination(numbers[index], choices, 2) != null;
        }

        static int? FindEndIndex(List<long> numbers, long requiredSum, int startIndex)
        {
            var endIndex = startIndex + 1;

            var sum = numbers[startIndex] + numbers[endIndex];

            while (sum < requiredSum)
            {
                endIndex++;
                sum += numbers[endIndex];
            }

            return sum == requiredSum ? endIndex : null;
        }

        static (int from, int to) FindRange(List<long> numbers, long requiredSum)
        {
            int from = 0;
            int? to;

            while ((to = FindEndIndex(numbers, requiredSum, from)) == null)
            {
                from++;
            }

            return (from, to.Value);
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var parsed = input.Select(long.Parse).ToList();

            var length = 25;

            var index = parsed.Skip(length).Select((_, i) => i + length).First(index => !IsValid(parsed, length, index));
            var answer1 = parsed[index];

            Console.WriteLine($"Answer 1:{answer1}");

            (var from, var to) = FindRange(parsed, answer1);
            var range = parsed.GetRange(from, to - from + 1);
            var answer2 = range.Min() + range.Max();

            Console.WriteLine($"Answer 2:{answer2}");
        }
    }
}
