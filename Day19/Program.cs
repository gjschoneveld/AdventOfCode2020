using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day19
{
    abstract class Rule
    {
        public int Id { get; set; }

        public abstract List<int> MatchLengths(Dictionary<int, Rule> rules, string value);

        public static Rule Parse(string x)
        {
            var colon = x.IndexOf(':');

            var id = int.Parse(x.Substring(0, colon));

            var rule = x.Substring(colon + 1).Trim(' ', '\"');

            if (!char.IsDigit(rule[0]))
            {
                return new ValueRule
                {
                    Id = id,
                    Value = rule[0]
                };
            }

            var innerRules = rule.Split('|').Select(c => c.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()).ToList();

            return new RegularRule
            {
                Id = id,
                InnerRules = innerRules
            };
        }
    }

    class ValueRule : Rule
    {
        public char? Value { get; set; }

        public override List<int> MatchLengths(Dictionary<int, Rule> rules, string value)
        {
            if (value.Length > 0 && Value == value[0])
            {
                return new List<int> { 1 };
            }

            return new List<int>();
        }
    }

    class RegularRule : Rule
    {
        public List<List<int>> InnerRules { get; set; }

        private List<int> MatchLengthsForInnerRule(Dictionary<int, Rule> rules, List<int> rule, string value)
        {
            var offsets = new List<int> { 0 };

            foreach (var id in rule)
            {
                offsets = offsets.SelectMany(o => rules[id].MatchLengths(rules, value.Substring(o)).Select(l => o + l)).Distinct().ToList();
            }

            return offsets;
        }

        public override List<int> MatchLengths(Dictionary<int, Rule> rules, string value)
        {
            return InnerRules.SelectMany(r => MatchLengthsForInnerRule(rules, r, value)).Distinct().ToList();
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

            (rules[8] as RegularRule).InnerRules.Add(new List<int> { 42, 8 });
            (rules[11] as RegularRule).InnerRules.Add(new List<int> { 42, 11, 31 });

            var answer2 = messages.Count(m => rules[0].MatchLengths(rules, m).Contains(m.Length));

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
