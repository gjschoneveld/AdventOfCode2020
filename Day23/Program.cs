using System;
using System.Collections.Generic;
using System.Linq;

namespace Day23
{
    class CircularList
    {
        public const int PickupSize = 3;

        public List<int> Items { get; set; }

        public int CurrentItem => Items[0];

        public void Rotate()
        {
            Items = Items.Skip(1).Take(Items.Count - 1).Append(Items[0]).ToList();
        }

        public List<int> Remove()
        {
            var result = Items.GetRange(1, PickupSize);

            Items.RemoveRange(1, PickupSize);

            return result;
        }

        public void Insert(int destination, List<int> values)
        {
            var index = Items.IndexOf(destination) + 1;
            Items.InsertRange(index, values);
        }

        public int State1 => Items
            .Concat(Items)
            .SkipWhile(i => i != 1)
            .Skip(1)
            .TakeWhile(i => i != 1)
            .Aggregate((a, b) => a * 10 + b);

        public long State2 => Items
            .Concat(Items)
            .SkipWhile(i => i != 1)
            .Skip(1)
            .Take(2)
            .Select(i => (long)i)
            .Aggregate((a, b) => a * b);
    }

    class Program
    {
        static int SubtractOne(int value, int min, int max)
        {
            var result = value - 1;

            if (result < min)
            {
                return max;
            }

            return result;
        }

        static void Simulate(CircularList circularList, int moves)
        {
            var min = circularList.Items.Min();
            var max = circularList.Items.Max();

            for (int move = 1; move <= moves; move++)
            {
                var current = circularList.CurrentItem;
                var group = circularList.Remove();

                var destination = SubtractOne(current, min, max);

                while (group.Contains(destination))
                {
                    destination = SubtractOne(destination, min, max);
                }

                circularList.Insert(destination, group);

                circularList.Rotate();
            }
        }

        static void Main(string[] args)
        {
            var input = "389125467";

            var circularList1 = new CircularList
            {
                Items = input.Select(i => i - '0').ToList()
            };

            Simulate(circularList1, 100);
                
            var answer1 = circularList1.State1;

            Console.WriteLine($"Answer 1: {answer1}");


            var circularList2 = new CircularList
            {
                Items = input.Select(i => i - '0').ToList()
            };

            var next = circularList2.Items.Max() + 1;

            while (next <= 1_000_000)
            {
                circularList2.Items.Add(next);
                next++;
            }

            Simulate(circularList2, 1_000);

            var answer2 = circularList2.State2;

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
