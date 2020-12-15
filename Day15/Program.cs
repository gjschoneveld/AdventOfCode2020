using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15
{
    class Program
    {
        static int FindNumber(List<int> start, int turn)
        {
            var lastLocation = new Dictionary<int, int>();

            for (int i = 0; i < start.Count - 1; i++)
            {
                lastLocation[start[i]] = i + 1;
            }

            var lastNumber = start.Last();
            var lastTurn = start.Count;

            while (lastTurn < turn)
            {
                var number = 0;

                if (lastLocation.ContainsKey(lastNumber))
                {
                    number = lastTurn - lastLocation[lastNumber];
                }

                lastLocation[lastNumber] = lastTurn;

                lastNumber = number;
                lastTurn++;
            }

            return lastNumber;
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");
            var start = input.Split(',').Select(int.Parse).ToList();

            var answer1 = FindNumber(start, 2020);
            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = FindNumber(start, 30000000);
            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
