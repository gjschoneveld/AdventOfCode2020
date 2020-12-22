using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day22
{
    class Program
    {
        static (int winner, int value) Simulate(List<List<int>> players, bool recursive)
        {
            var history = new List<List<List<int>>>();

            var queues = players.Select(p => new Queue<int>(p)).ToList();

            while (queues.All(q => q.Count > 0))
            {
                history.Add(queues.Select(q => q.ToList()).ToList());

                var top = queues.Select(q => q.Dequeue()).ToList();

                var max = top.Max();
                var winnerOfRound = top.IndexOf(max);

                if (recursive && queues.Select((q, i) => top[i] <= q.Count).All(x => x))
                {
                    (winnerOfRound, _) = Simulate(queues.Select((q, i) => q.Take(top[i]).ToList()).ToList(), recursive);
                }

                queues[winnerOfRound].Enqueue(top[winnerOfRound]);
                queues[winnerOfRound].Enqueue(top[1 - winnerOfRound]);

                if (history.Any(h => h.Zip(queues).All(z => z.First.SequenceEqual(z.Second))))
                {
                    // cycle detected
                    return (0, 0);
                }
            }

            var winner = queues.FindIndex(q => q.Count > 0);
            var value = queues[winner].Select((value, index) => value * (queues[winner].Count - index)).Sum();

            return (winner, value);
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var player1 = input.Skip(1).TakeWhile(x => x != "").Select(int.Parse).ToList();
            var player2 = input.Skip(player1.Count + 3).Select(int.Parse).ToList();

            (_, var answer1) = Simulate(new List<List<int>> { player1, player2 }, false);

            Console.WriteLine($"Answer 1: {answer1}");

            (_, var answer2) = Simulate(new List<List<int>> { player1, player2 }, true);

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
