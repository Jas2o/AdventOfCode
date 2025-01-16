using System.Text;
using static AoC.Day.Day09;

namespace AoC.Day
{
    public class Day09
    {
        public static void Run(string file) {
            Console.WriteLine("Day 9: Marble Mania" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                //There's only one line in the real input.

                string[] fields = line.Split(' ');
                int numPlayers = int.Parse(fields[0]);
                int highMarble = int.Parse(fields[6]);
                long partA = Solve(numPlayers, highMarble);
                long partB = Solve(numPlayers, highMarble * 100);

                Console.WriteLine("Part 1: " + partA);
                //Answer: 398242
                Console.WriteLine("Part 2: " + partB);
                //Answer: 3273842452
                Console.WriteLine();
            }
        }

        private static long Solve(int numPlayers, int highMarble) {
            bool verbose = (highMarble < 30);

            Queue<int> marbles = new Queue<int>(Enumerable.Range(1, highMarble));
            Dictionary<int, List<long>> players = new Dictionary<int, List<long>>();
            for (int p = 1; p <= numPlayers; p++) {
                players[p] = new List<long>();
            }

            DLNode<int> root = new DLNode<int>(0);
            root.Previous = root;
            root.Next = root;
            DLNode<int> current = root;

            while (marbles.Any()) {
                for (int p = 1; p <= numPlayers; p++) {
                    if (!marbles.Any())
                        break;

                    int marble = marbles.Dequeue();
                    if (marble % 23 == 0) {
                        players[p].Add(marble);
                        DLNode<int> takeMarble = current.Previous.Previous.Previous.Previous.Previous.Previous.Previous;
                        players[p].Add(takeMarble.Value);

                        DLNode<int> prev = takeMarble.Previous;
                        current = takeMarble.Next;
                        prev.Next = current;
                        current.Previous = prev;

                        if (verbose) {
                            Console.WriteLine("[{0}] = SOMETHING INTERESTING HAPPENS (keep {1}, take {2})", p, marble, takeMarble.Value);
                        }
                    } else {
                        DLNode<int> addedMarble = new DLNode<int>(marble);
                        current.Next.AddAfter(addedMarble);
                        current = addedMarble;
                    }

                    if (verbose) {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(root.Value);
                        DLNode<int> n = root.Next;
                        while(n != root) {
                            sb.Append(" " + n.Value);
                            n = n.Next;
                        }
                        Console.WriteLine("[{0}] = {1}", p, sb.ToString());
                    }
                }
            }

            long result = players.Max(p => p.Value.Sum());

            if (result < 50000) {
                Console.WriteLine();
                Console.WriteLine("{0} players, with {1} marbles.", numPlayers, highMarble);
                foreach (var player in players) {
                    if (player.Value.Sum() > 0) {
                        player.Value.Sort();
                        Console.WriteLine("[{0}] has {1}", player.Key, string.Join(' ', player.Value));
                    }
                }
                Console.WriteLine();
            }

            return result;
        }

        public class DLNode<T> {
            public DLNode<T> Previous;
            public DLNode<T> Next;
            public T Value;

            public DLNode(T data) {
                Value = data;
            }

            public void AddAfter(DLNode<T> after) {
                DLNode<T> orgNext = Next;
                Next = after;
                Next.Previous = this;
                Next.Next = orgNext;
                orgNext.Previous = Next;
            }

            public void AddBefore(DLNode<T> before) {
                DLNode<T> orgPrev = Previous;
                Previous = before;
                Previous.Next = this;
                Previous.Previous = orgPrev;
                orgPrev.Next = Previous;
            }
        }

    }
}
