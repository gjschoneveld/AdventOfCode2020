using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        static int WaitTime(int bus, int timestamp)
        {
            var elapsed = timestamp % bus;

            if (elapsed == 0)
            {
                return 0;
            }

            return bus - elapsed;
        }

        static long Modulo(long value, long mod)
        {
            return (value % mod + mod) % mod;
        }

        static long Multiply(List<long> values)
        {
            return values.Aggregate((a, b) => a * b);
        }

        static long FindInverse(long value, long mod)
        {
            long result = 1;

            while (Modulo(value * result, mod) != 1)
            {
                result++;
            }

            return result;
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var timestamp = int.Parse(input[0]);
            var buses = input[1].Split(',')
                .Select((raw, index) => (raw, index))
                .Where(b => b.raw != "x")
                .Select(b => (id: int.Parse(b.raw), b.index))
                .ToList();

            var minWaitTime = buses.Min(b => WaitTime(b.id, timestamp));
            var minWaitTimeBus = buses.First(b => WaitTime(b.id, timestamp) == minWaitTime);

            var answer1 = minWaitTime * minWaitTimeBus.id;

            Console.WriteLine($"Answer 1: {answer1}");

            var remainders = buses.Select(b => (value: Modulo(b.id - b.index, b.id), mod: (long)b.id)).ToList();

            var mod = Multiply(remainders.Select(r => r.mod).ToList());
            var terms = remainders.Select(r => r.value * (mod / r.mod) * FindInverse(mod / r.mod, r.mod)).ToList();
            var answer2 = Modulo(terms.Sum(), mod);

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
