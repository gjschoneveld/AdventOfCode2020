using System;
using System.Collections.Generic;
using System.Linq;

namespace Day23
{
    class CircularList
    {
        public const int PickupSize = 3;

        private LinkedList<int> items;
        private LinkedListNode<int>[] index;

        public CircularList(int maxValue)
        {
            items = new LinkedList<int>();
            index = new LinkedListNode<int>[maxValue + 1];
        }

        public int CurrentItem => items.First.Value;

        public int Min => items.Min();
        public int Max => items.Max(); 

        public LinkedListNode<int> AddAfter(LinkedListNode<int> node, int value)
        {
            var newNode = items.AddAfter(node, value);

            index[value] = newNode;

            return newNode;
        }

        public LinkedListNode<int> AddLast(int value)
        {
            var newNode = items.AddLast(value);

            index[value] = newNode;

            return newNode;
        }

        public void Rotate()
        {
            var currentItem = CurrentItem;

            items.RemoveFirst();
            AddLast(currentItem);
        }

        public List<int> Remove()
        {
            var result = new List<int>();

            var node = items.First.Next;

            for (int i = 0; i < PickupSize; i++)
            {
                result.Add(node.Value);
                node = node.Next;
                items.Remove(node.Previous);
            }

            return result;
        }

        public void Insert(int destination, List<int> values)
        {
            var node = index[destination];

            foreach (var value in values)
            {
                node = AddAfter(node, value);
            }
        }

        public int State1 => items
            .Concat(items)
            .SkipWhile(i => i != 1)
            .Skip(1)
            .TakeWhile(i => i != 1)
            .Aggregate((a, b) => a * 10 + b);

        public long State2 => items
            .Concat(items)
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
            var min = circularList.Min;
            var max = circularList.Max;

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
            var input = "739862541";
            var inputValues = input.Select(i => i - '0').ToList();

            var circularList1 = new CircularList(9);

            foreach (var value in inputValues)
            {
                circularList1.AddLast(value);
            }

            Simulate(circularList1, 100);
                
            var answer1 = circularList1.State1;

            Console.WriteLine($"Answer 1: {answer1}");


            var circularList2 = new CircularList(1_000_000);

            foreach (var value in inputValues)
            {
                circularList2.AddLast(value);
            }

            var next = circularList2.Max + 1;

            while (next <= 1_000_000)
            {
                circularList2.AddLast(next);
                next++;
            }

            Simulate(circularList2, 10_000_000);

            var answer2 = circularList2.State2;

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
