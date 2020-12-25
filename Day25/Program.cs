using System;
using System.IO;
using System.Linq;

namespace Day25
{
    class Program
    {
        static int FindLoopSize(long publicKey)
        {
            var subject = 7L;

            var size = 0;
            var value = 1L;

            while (value != publicKey)
            {
                value = (value * subject) % 20201227L;
                size++;
            }

            return size;
        }

        static long Transform(long subject, int size)
        {
            var value = 1L;

            for (int i = 0; i < size; i++)
            {
                value = (value * subject) % 20201227L;
            }

            return value;
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var publicKeys = input.Select(long.Parse).ToList();

            var loopSizes = publicKeys.Select(FindLoopSize).ToList();

            var answer = Transform(publicKeys[0], loopSizes[1]);

            Console.WriteLine($"Answer: {answer}");
        }
    }
}
