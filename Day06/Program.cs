using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day06
{
    class Program
    {
        static List<List<string>> GroupInput(string[] input)
        {
            var groups = new List<List<string>> { new List<string>() };

            foreach (var line in input)
            {
                if (line == "")
                {
                    groups.Add(new List<string>());
                    continue;
                }

                groups.Last().Add(line);
            }

            return groups;
        }

        static List<char> AllAnswers(List<string> group)
        {
            return group.SelectMany(p => p).Distinct().ToList();
        }

        static string Intersect(string a, string b)
        {
            return new string(a.Intersect(b).ToArray());
        }

        static List<char> CommonAnswers(List<string> group)
        {
            return group.Aggregate(Intersect).ToList();
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var groups = GroupInput(input);

            var answer1 = groups.Sum(g => AllAnswers(g).Count);

            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = groups.Sum(g => CommonAnswers(g).Count);

            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}
