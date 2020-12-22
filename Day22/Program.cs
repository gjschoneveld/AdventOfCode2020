using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Day22
{
    using Hand = List<int>;

    class Program
    {
        class HistoryComparer : IEqualityComparer<List<Hand>>
        {
            public bool Equals([AllowNull] List<Hand> x, [AllowNull] List<Hand> y)
            {
                return x.Zip(y).All(z => z.First.SequenceEqual(z.Second));
            }

            public int Hash(Hand hand)
            {
                return hand.Aggregate(0, (a, b) => 50 * a + b);
            }

            public int GetHashCode([DisallowNull] List<Hand> obj)
            {
                return obj.Select(Hash).Aggregate((a, b) => a ^ b);
            }
        }

        static List<Hand> QueuesToHistoryItem(List<Queue<int>> queues)
        {
            return queues.Select(q => q.ToList()).ToList();
        }

        static (int winner, int value) Simulate(List<Hand> players, bool recursive)
        {
            var history = new HashSet<List<Hand>>(new HistoryComparer());

            var queues = players.Select(p => new Queue<int>(p)).ToList();

            while (queues.All(q => q.Count > 0))
            {
                if (history.Contains(QueuesToHistoryItem(queues)))
                {
                    // cycle detected
                    return (0, 0);
                }

                history.Add(QueuesToHistoryItem(queues));

                var top = queues.Select(q => q.Dequeue()).ToList();

                var max = top.Max();
                var winnerOfRound = top.IndexOf(max);

                if (recursive && queues.Zip(top, (q, t) => t <= q.Count).All(x => x))
                {
                    (winnerOfRound, _) = Simulate(queues.Zip(top, (q, t) => q.Take(t).ToList()).ToList(), recursive);
                }

                queues[winnerOfRound].Enqueue(top[winnerOfRound]);
                queues[winnerOfRound].Enqueue(top[1 - winnerOfRound]);
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

            (_, var answer1) = Simulate(new List<Hand> { player1, player2 }, false);

            Console.WriteLine($"Answer 1: {answer1}");

            (_, var answer2) = Simulate(new List<Hand> { player1, player2 }, true);

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
