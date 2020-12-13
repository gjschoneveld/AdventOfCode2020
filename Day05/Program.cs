using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day05
{
    class Program
    {
        static int CodeToValue(string code, char character)
        {
            var result = 0;

            for (int i = 0; i < code.Length; i++)
            {
                if (code[code.Length - i - 1] == character)
                {
                    result |= 1 << i;
                }
            }

            return result;
        }

        static int ConvertToSeatId(string code)
        {
            var row = CodeToValue(code.Substring(0, 7), 'B');
            var column = CodeToValue(code.Substring(7, 3), 'R');

            return row * 8 + column;
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var seats = new HashSet<int>(input.Select(ConvertToSeatId));

            var answer1 = seats.Max();

            Console.WriteLine($"Answer 1: {answer1}");

            int answer2 = seats.First(s => !seats.Contains(s + 1) && seats.Contains(s + 2)) + 1;

            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}
