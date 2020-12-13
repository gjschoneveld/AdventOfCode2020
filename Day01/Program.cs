using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01
{
    class Program
    {
        public static List<int> FindCombination(int requiredSum, List<int> choices, int picks)
        {
            if (picks == 1)
            {
                if (choices.Contains(requiredSum))
                {
                    return new List<int> { requiredSum };
                }

                return null;
            }

            foreach (var choice in choices)
            {
                var innerSum = requiredSum - choice;
                var innerPicks = picks - 1;

                var innerChoices = new List<int>(choices);
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

        public static int Multiply(List<int> values)
        {
            return values.Aggregate((a, b) => a * b);
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var choices = input.Select(int.Parse).ToList();

            var combination1 = FindCombination(2020, choices, 2);
            var answer1 = Multiply(combination1);

            Console.WriteLine($"Answer 1: {answer1}");

            var combination2 = FindCombination(2020, choices, 3);
            var answer2 = Multiply(combination2);

            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}
