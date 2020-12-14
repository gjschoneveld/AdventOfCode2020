using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day14
{
    class Program
    {
        static string ParseMask(string x)
        {
            var parts = x.Split(new char[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);

            return parts[1];
        }

        static (long address, long value) ParseMem(string x)
        {
            var parts = x.Split(new char[] { '[', ']', ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);

            return (long.Parse(parts[1]), long.Parse(parts[2]));
        }

        static string ToBitString(long value)
        {
            return Convert.ToString(value, 2).PadLeft(36, '0');
        }

        static long ToLong(string value)
        {
            return Convert.ToInt64(value, 2);
        }

        static string ApplyValueMask(string value, string mask)
        {
            return new string(value.Zip(mask, (v, m) => m == 'X' ? v : m).ToArray());
        }

        static string ApplyAddressMask(string address, string mask)
        {
            return new string(address.Zip(mask, (a, m) => m == '0' ? a : m).ToArray());
        }

        static List<string> GenerateAddresses(string generator)
        {
            if (!generator.Contains('X'))
            {
                return new List<string> { generator };
            }

            var indices = generator.Select((v, i) => (v, i)).Where(x => x.v == 'X').Select(x => x.i).ToList();

            var result = new List<string>();

            var builder = new StringBuilder(generator);

            for (var value = 0; value < (1 << indices.Count); value++)
            {
                for (var i = 0; i < indices.Count; i++)
                {
                    var isSet = (value & (1 << i)) > 0;

                    builder[indices[i]] = isSet ? '1' : '0';
                }

                result.Add(builder.ToString());
            }

            return result;
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var mask = "";
            var memory1 = new Dictionary<long, long>();

            foreach (var line in input)
            {
                if (line.StartsWith("mask"))
                {
                    mask = ParseMask(line);
                    continue;
                }

                (var address, var raw) = ParseMem(line);

                var value = ApplyValueMask(ToBitString(raw), mask);

                memory1[address] = ToLong(value);
            }

            var answer1 = memory1.Values.Sum();

            Console.WriteLine($"Answer 1: {answer1}");


            var memory2 = new Dictionary<long, long>();

            foreach (var line in input)
            {
                if (line.StartsWith("mask"))
                {
                    mask = ParseMask(line);
                    continue;
                }

                (var raw, var value) = ParseMem(line);

                var addressGenerator = ApplyAddressMask(ToBitString(raw), mask);

                var addresses = GenerateAddresses(addressGenerator);

                foreach (var address in addresses)
                {
                    memory2[ToLong(address)] = value;
                }
            }

            var answer2 = memory2.Values.Sum();

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
