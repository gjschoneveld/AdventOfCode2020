using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day02
{
    class Line
    {
        public List<int> Numbers { get; set; }
        public char Character { get; set; }
        public string Value { get; set; }

        public bool IsValid1()
        {
            var count = Value.Count(c => c == Character);

            return Numbers[0] <= count && count <= Numbers[1];
        }

        public bool IsValid2()
        {
            var characters = Numbers.Select(n => Value[n - 1]).ToList();

            return characters.Count(c => c == Character) == 1;
        }
    }

    class Program
    {
        static Line Parse(string x)
        {
            var regex = new Regex(@"((?<num>\d+)-?)+ (?<char>\w): (?<value>\w+)");
            var match = regex.Match(x);

            var numbers = match.Groups["num"].Captures.Cast<Capture>().Select(c => int.Parse(c.Value)).ToList();

            return new Line {
                Numbers = numbers,
                Character = match.Groups["char"].Value[0],
                Value = match.Groups["value"].Value
            };
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var parsed = input.Select(Parse).ToList();

            var answer1 = parsed.Count(line => line.IsValid1());

            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = parsed.Count(line => line.IsValid2());

            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}
