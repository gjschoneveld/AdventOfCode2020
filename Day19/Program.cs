using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day19
{
    class Rule
    {
        public int Id { get; set; }

        public char? Value { get; set; }

        public List<List<int>> InnerRules { get; set; }

        public List<int> MatchLengths(Dictionary<int, Rule> rules, string value)
        {
            if (Value != null)
            {
                if (value.Length > 0 && Value == value[0])
                {
                    return new List<int> { 1 };
                }

                return new List<int>();
            }

            var result = new List<int>();

            foreach (var combination in InnerRules)
            {
                var offsets = new List<int> { 0 };

                foreach (var rule in combination)
                {
                    var newOffsets = new List<int>();

                    foreach (var offset in offsets)
                    {
                        var innerValue = value.Substring(offset);

                        var innerLengths = rules[rule].MatchLengths(rules, innerValue);

                        newOffsets.AddRange(innerLengths.Select(l => l + offset));
                    }

                    offsets = newOffsets;
                }

                result.AddRange(offsets);
            }

            return result.Distinct().ToList();
        }

        public static Rule Parse(string x)
        {
            var colon = x.IndexOf(':');

            var id = int.Parse(x.Substring(0, colon));

            var rule = x.Substring(colon + 1).Trim(' ', '\"');

            if (!char.IsDigit(rule[0]))
            {
                return new Rule
                {
                    Id = id,
                    Value = rule[0]
                };
            }

            var innerRules = rule.Split('|').Select(c => c.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()).ToList();

            return new Rule
            {
                Id = id,
                InnerRules = innerRules
            };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var rules = input.TakeWhile(i => i != "").Select(Rule.Parse).ToDictionary(r => r.Id);

            var messages = input.Skip(rules.Count + 1).ToList();

            var answer1 = messages.Count(m => rules[0].MatchLengths(rules, m).Contains(m.Length));

            Console.WriteLine($"Answer 1: {answer1}");

            rules[8].InnerRules.Add(new List<int> { 42, 8 });
            rules[11].InnerRules.Add(new List<int> { 42, 11, 31 });

            var answer2 = messages.Count(m => rules[0].MatchLengths(rules, m).Contains(m.Length));

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
