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
                int partA = Solve(numPlayers, highMarble);
                int partB = Solve(numPlayers, highMarble * 100);

                Console.WriteLine("Part 1: " + partA);
                //Answer: 398242
                Console.WriteLine("Part 2: " + partB);
                //Answer: 
                Console.WriteLine();
            }
        }

        private static int Solve(int numPlayers, int highMarble) {
            bool verbose = (highMarble < 30);

            List<int> circle = new List<int>();
            Queue<int> marbles = new Queue<int>(Enumerable.Range(1, highMarble));
            Dictionary<int, List<int>> players = new Dictionary<int, List<int>>();
            for (int p = 1; p <= numPlayers; p++) {
                players[p] = new List<int>();
            }

            circle.Add(0);
            int pos = 0;

            while (marbles.Any()) {
                for (int p = 1; p <= numPlayers; p++) {
                    if (!marbles.Any())
                        break;

                    int marble = marbles.Dequeue();
                    if (marble % 23 == 0) {
                        players[p].Add(marble);
                        int takeFrom = pos - 7 % circle.Count;

                        if (takeFrom < 0)
                            takeFrom += circle.Count;

                        int marble2 = circle[takeFrom];
                        circle.RemoveAt(takeFrom);
                        players[p].Add(marble2);
                        players[p].Sort();
                        pos = takeFrom;
                        if (verbose) {
                            Console.WriteLine("[{0}] = SOMETHING INTERESTING HAPPENS (keep {1}, take {2})", p, marble, marble2);
                        }
                    } else {
                        int insertAt = ((pos + 1) % circle.Count) + 1;
                        //Console.WriteLine("Insert at: " + insertAt);
                        circle.Insert(insertAt, marble);
                        pos = insertAt;
                    }

                    if (verbose) {
                        Console.WriteLine("[{0}] = {1}", p, string.Join(' ', circle));
                        //Console.WriteLine("Current is now: " + circle[pos]);
                    }
                }

                //--

                /*
                int lookingFor = numPlayers * 23;

                bool test = true;
                for (int p = 1; p <= numPlayers; p++) {
                    if (players[p].Count < 2) {
                        test = false;
                        break;
                    }

                    int[] nums = players[p].TakeLast(2).ToArray();
                    int diff = nums[1] - nums[0];
                    if(diff != lookingFor) {
                        test = false;
                        break;
                    }
                }

                if(test) {
                    break;
                }
                */
            }

            Console.WriteLine();
            Console.WriteLine("{0} players, with {1} marbles.", numPlayers, highMarble);
            foreach (var player in players) {
                if (player.Value.Sum() > 0) {
                    //player.Value.Sort();
                    Console.WriteLine("[{0}] has {1}", player.Key, string.Join(' ', player.Value));
                }
            }
            Console.WriteLine();
            return players.Max(p => p.Value.Sum());
        }
    }
}
