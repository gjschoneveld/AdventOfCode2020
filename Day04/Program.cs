using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day04
{
    class Program
    {
        static List<string> GroupInput(string[] input)
        {
            var groups = new List<StringBuilder> { new StringBuilder() };

            foreach (var line in input)
            {
                if (line == "")
                {
                    groups.Add(new StringBuilder());
                    continue;
                }

                groups.Last().Append(line + " ");
            }

            return groups.Select(sb => sb.ToString().Trim()).ToList();
        }

        static Dictionary<string, string> Parse(string x)
        {
            return x.Split(' ').Select(kv => kv.Split(':')).ToDictionary(kv => kv[0], kv => kv[1]);
        }

        static bool IsValid1(Dictionary<string, string> passport)
        {
            var requiredFields = new List<string>
            {
                "byr",
                "iyr",
                "eyr",
                "hgt",
                "hcl",
                "ecl",
                "pid"
            };

            return requiredFields.All(r => passport.ContainsKey(r));
        }

        static bool IsValidBirthYear(string value)
        {
            // four digits; at least 1920 and at most 2002

            if (value.Length != 4 || value.Any(c => !char.IsDigit(c)))
            {
                return false;
            }

            var parsed = int.Parse(value);

            return 1920 <= parsed && parsed <= 2002;
        }

        static bool IsValidIssueYear(string value)
        {
            // four digits; at least 2010 and at most 2020

            if (value.Length != 4 || value.Any(c => !char.IsDigit(c)))
            {
                return false;
            }

            var parsed = int.Parse(value);

            return 2010 <= parsed && parsed <= 2020;
        }

        static bool IsValidExpirationYear(string value)
        {
            // four digits; at least 2020 and at most 2030

            if (value.Length != 4 || value.Any(c => !char.IsDigit(c)))
            {
                return false;
            }

            var parsed = int.Parse(value);

            return 2020 <= parsed && parsed <= 2030;
        }

        static bool IsValidHeight(string value)
        {
            // a number followed by either cm or in:
            // If cm, the number must be at least 150 and at most 193.
            // If in, the number must be at least 59 and at most 76.

            if (!value.EndsWith("cm") && !value.EndsWith("in"))
            {
                return false;
            }

            var number = value.Substring(0, value.Length - 2);

            if (number.Length == 0 || number.Any(c => !char.IsDigit(c)))
            {
                return false;
            }

            var parsed = int.Parse(number);

            if (value.EndsWith("cm"))
            {
                return 150 <= parsed && parsed <= 193;
            }

            return 59 <= parsed && parsed <= 76;
        }

        static bool IsValidHairColor(string value)
        {
            // a # followed by exactly six characters 0-9 or a-f

            if (value.Length != 7 || value[0] != '#')
            {
                return false;
            }

            return value.Substring(1).All(c => ('0' <= c && c <= '9') || ('a' <= c && c <= 'f'));
        }

        static bool IsValidEyeColor(string value)
        {
            // exactly one of: amb blu brn gry grn hzl oth

            var allowed = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

            return allowed.Contains(value);
        }

        static bool IsValidPassportID(string value)
        {
            // a nine-digit number, including leading zeroes

            return value.Length == 9 && value.All(c => char.IsDigit(c));
        }

        static bool IsValid2(Dictionary<string, string> passport)
        {
            return IsValidBirthYear(passport["byr"]) &&
                IsValidIssueYear(passport["iyr"]) &&
                IsValidExpirationYear(passport["eyr"]) &&
                IsValidHeight(passport["hgt"]) &&
                IsValidHairColor(passport["hcl"]) &&
                IsValidEyeColor(passport["ecl"]) &&
                IsValidPassportID(passport["pid"]);
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var rawPassports = GroupInput(input);
            var passports = rawPassports.Select(Parse).ToList();

            var valid1 = passports.Where(IsValid1).ToList();
            var answer1 = valid1.Count();

            Console.WriteLine($"Answer 1: {answer1}");

            var valid2 = valid1.Where(IsValid2).ToList();
            var answer2 = valid2.Count();

            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}
