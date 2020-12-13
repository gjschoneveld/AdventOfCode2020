using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day07
{
    class Node
    {
        public string Name { get; set; }

        public List<(string Name, int Quantity)> Children { get; set; }

        public List<string> Parents { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"{Name}:" + string.Join(',', Children.Select(c => $"{c.Name}:{c.Quantity}"));
        }
    }

    class Program
    {
        static Node Parse(string x)
        {
            var regex = new Regex(@"(?<name>\w+ \w+) bags contain( (?<quantity>\d+) (?<child>\w+ \w+) bags?[,.])*");
            var match = regex.Match(x);

            var quantities = match.Groups["quantity"].Captures.Cast<Capture>().Select(c => int.Parse(c.Value)).ToList();
            var childNames = match.Groups["child"].Captures.Cast<Capture>().Select(c => c.Value).ToList();

            return new Node
            {
                Name = match.Groups["name"].Value,
                Children = childNames.Zip(quantities, (child, quantity) => (child, quantity)).ToList()
            };
        }

        static void FillParents(Dictionary<string, Node> nodes)
        {
            foreach (var kv in nodes)
            {
                foreach (var child in kv.Value.Children)
                {
                    nodes[child.Name].Parents.Add(kv.Key);
                }
            }    
        }

        static List<string> CollectParents(Dictionary<string, Node> nodes, string name)
        {
            var node = nodes[name];

            if (node.Parents.Count == 0)
            {
                return new List<string>();
            }

            var parents = new List<string>(node.Parents);

            parents.AddRange(node.Parents.SelectMany(p => CollectParents(nodes, p)));

            return parents.Distinct().ToList();
        }

        static int CountBags(Dictionary<string, Node> nodes, string name)
        {
            var node = nodes[name];

            return node.Children.Sum(c => c.Quantity * (CountBags(nodes, c.Name) + 1));
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var nodes = input.Select(Parse).ToDictionary(n => n.Name);
            FillParents(nodes);

            var answer1 = CollectParents(nodes, "shiny gold").Count;

            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = CountBags(nodes, "shiny gold");

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
