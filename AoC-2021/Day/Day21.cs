namespace AoC.Day
{
    public class Day21
    {
        public static void Run(string file) {
            Console.WriteLine("Day 21: Dirac Dice" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            Console.WriteLine(lines[0]);
            Console.WriteLine(lines[1]);
            int player1 = int.Parse(lines[0].Substring(28));
            int player2 = int.Parse(lines[1].Substring(28));

            int partA = SolveA(player1, player2, true);

            if (false) {
                Console.WriteLine("Test winning score, Player 1 and Player 2 wins in how many universes:");
                for (int test = 1; test < 21; test++) {
                    (long win1, long win2) = SolveB(player1, player2, test);
                    Console.WriteLine("{0,2}, {1,14}, {2,14}", test, win1, win2);
                }
                Console.WriteLine();
            }

            (long partB1, long partB2) = SolveB(player1, player2);
            long partB = Math.Max(partB1, partB2);
            Console.WriteLine("Player 1 wins in {0} universes, Player 2 wins in {1} universes.", partB1, partB2);

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 926610
            Console.WriteLine("Part 2: " + partB);
            //Answer: 146854918035875
        }

        private class Player {
            public int ID;
            public int Position;
            public int Score;

            public Player(int id, int startingPos, int score = 0) {
                ID = id;
                Position = startingPos;
                Score = score;
            }

            public void Move(int roll, bool outputText) {
                Position += roll - 1;
                Position %= 10;
                Position++;
                Score += Position;

                if (outputText)
                    Console.WriteLine("Player {0} rolls {1} and moves to space {2} for a score of {3}.", ID, roll, Position, Score);
            }
        }

        private static int SolveA(int player1, int player2, bool outputText) {
            int result = 0;

            Player p1 = new Player(1, player1);
            Player p2 = new Player(2, player2);
            DeterministicDice dice = new DeterministicDice(1, 100);

            while (true) {
                int roll = dice.CombinedRoll(3);
                p1.Move(roll, outputText);
                if (p1.Score >= 1000) {
                    result = dice.RollCount * p2.Score;
                    break;
                }

                roll = dice.CombinedRoll(3);
                p2.Move(roll, outputText);
                if (p2.Score >= 1000) {
                    result = dice.RollCount * p1.Score;
                    break;
                }
            }

            if (outputText)
                Console.WriteLine();

            return result;
        }

        private static (long winP1, long winP2) SolveB(int p1startpos, int p2startpos, int winningScore = 21) {
            long winsP1 = 0;
            long winsP2 = 0;

            Dictionary<int, (int sum, int combos)> combinations = new Dictionary<int, (int sum, int combos)>() {
                { 0, (3, 1) },
                { 1, (4, 3) },
                { 2, (5, 6) },
                { 3, (6, 7) },
                { 4, (7, 6) },
                { 5, (8, 3) },
                { 6, (9, 1) },
            };

            List<(Player p1, Player p2, bool turn1, long uni)> progress = new List<(Player p1, Player p2, bool turn1, long uni)>() {
                (new Player(1, p1startpos, 0), new Player(2, p2startpos, 0), true, 1)
            };

            while (progress.Any()) {
                List<(Player p1, Player p2, bool turn1, long uni)> progressB = new List<(Player p1, Player p2, bool turn1, long uni)>();

                foreach ((Player p1, Player p2, bool turn1, long uni) scenario in progress) {
                    if (scenario.turn1) {
                        Player clone2 = new Player(2, scenario.p2.Position, scenario.p2.Score);

                        for (int i = 0; i < 7; i++) {
                            Player clone1 = new Player(1, scenario.p1.Position, scenario.p1.Score);
                            clone1.Move(combinations[i].sum, false);
                            long unis = combinations[i].combos * scenario.uni;
                            if (clone1.Score >= winningScore)
                                winsP1 += unis;
                            else
                                progressB.Add((clone1, clone2, false, unis));
                        }
                    } else {
                        Player clone1 = new Player(1, scenario.p1.Position, scenario.p1.Score);

                        for (int i = 0; i < 7; i++) {
                            Player clone2 = new Player(2, scenario.p2.Position, scenario.p2.Score);
                            clone2.Move(combinations[i].sum, false);
                            long unis = combinations[i].combos * scenario.uni;
                            if (clone2.Score >= winningScore)
                                winsP2 += unis;
                            else
                                progressB.Add((clone1, clone2, true, unis));
                        }
                    }
                }

                progress = progressB;
            }

            return (winsP1, winsP2);
        }

        private class DeterministicDice {
            //Used by Part 1

            private int Start;
            private int End;
            private int Next;
            public int RollCount { get; private set; }

            public DeterministicDice(int start, int end) {
                Start = start;
                End = end;
                Next = Start;
                RollCount = 0;
            }

            public int CombinedRoll(int times) {
                int result = 0;
                for(int i = 0; i < times; i++) {
                    result += Next++;
                    RollCount++;
                    if (Next > End)
                        Next = Start;
                }
                return result;
            }
        }
    }
}
