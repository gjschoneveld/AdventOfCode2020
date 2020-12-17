using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day16
{
    class Rule
    {
        public string Name { get; set; }

        public List<(int from, int to)> Ranges { get; set; }

        public int Position { get; set; }

        public bool IsValid(int number) => Ranges.Any(r => r.from <= number && number <= r.to); 

        public static Rule Parse(string x)
        {
            var parts = x.Split(':');

            var regex = new Regex(@"\d+");
            var matches = regex.Matches(parts[1]);

            var numbers = matches.Select(m => int.Parse(m.Value)).ToList();

            var from = numbers.Where((v, i) => i % 2 == 0).ToList();
            var to = numbers.Where((v, i) => i % 2 == 1).ToList();

            return new Rule
            {
                Name = parts[0],
                Ranges = from.Zip(to).ToList()
            };
        }
    }

    class Program
    {
        static List<int> ParseTicket(string x)
        {
            return x.Split(',').Select(int.Parse).ToList();
        }

        static long Multiply(List<long> values)
        {
            return values.Aggregate((a, b) => a * b);
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var rules = input.TakeWhile(x => x != "").Select(Rule.Parse).ToList();
            var myTicket = ParseTicket(input[rules.Count + 2]);
            var otherTickets = input.Skip(rules.Count + 5).Select(ParseTicket).ToList();

            var invalidNumbers = otherTickets.SelectMany(t => t).Where(n => !rules.Any(r => r.IsValid(n))).ToList();

            var answer1 = invalidNumbers.Sum();

            Console.WriteLine($"Answer 1: {answer1}");


            var invalidNumbersLookup = invalidNumbers.Distinct().ToHashSet();

            var validTickets = otherTickets.Where(t => !t.Any(n => invalidNumbersLookup.Contains(n))).ToList();

            validTickets.Add(myTicket);

            var validRules = new List<List<Rule>>();

            for (int i = 0; i < rules.Count; i++)
            {
                var numbers = validTickets.Select(t => t[i]).ToList();
                validRules.Add(rules.Where(r => numbers.All(n => r.IsValid(n))).ToList());
            }

            while (true)
            {
                var ruleList = validRules.FirstOrDefault(r => r.Count == 1);

                if (ruleList == null)
                {
                    break;
                }

                var rule = ruleList.First();

                rule.Position = validRules.IndexOf(ruleList);

                foreach (var list in validRules)
                {
                    list.Remove(rule);
                }
            }

            var departureRules = rules.Where(r => r.Name.StartsWith("departure")).ToList();

            var departureNumbers = departureRules.Select(r => myTicket[r.Position]).ToList();

            var answer2 = Multiply(departureNumbers.Select(n => (long)n).ToList());

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
