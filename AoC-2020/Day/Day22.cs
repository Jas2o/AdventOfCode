namespace AoC.Day
{
    public class Day22
    {
        public static void Run(string file) {
            Console.WriteLine("Day 22: Crab Combat" + Environment.NewLine);

            bool change = false;
            List<int> deck1 = new List<int>();
            List<int> deck2 = new List<int>();
			string[] lines = File.ReadAllLines(file);
            foreach(string line in lines) {
                if (line.Contains(':'))
                    change = !change;
                else if(int.TryParse(line, out int num)) {
                    if (change)
                        deck1.Add(num);
                    else
                        deck2.Add(num);
                }
            }

            bool outputExtra = deck1.Count < 10;

            int partA = SolveA(deck1, deck2, outputExtra, out int winnerA);
            int partB = SolveB(deck1, deck2, outputExtra, 1, out int winnerB);

            Console.WriteLine("Part 1: {0}", partA);
            //Answer: 33680
            Console.WriteLine("Part 2: {0}", partB);
            //Answer: 33683
        }

        private static int SolveA(List<int> deck1, List<int> deck2, bool outputExtraText, out int winningPlayer) {
            if (outputExtraText) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("=== Game Start ===\r\n");
                Console.ResetColor();
            }

            Queue<int> player1 = new Queue<int>(deck1);
            Queue<int> player2 = new Queue<int>(deck2);

            HashSet<string> alreadySeen = new HashSet<string>();

            int round = 0;
            while (player1.Any() && player2.Any()) {
                round++;
                int play1 = player1.Dequeue();
                int play2 = player2.Dequeue();

                if (outputExtraText) {
                    Console.WriteLine("-- Round {0} --", round);
                    Console.WriteLine("Player 1's deck: " + string.Join(", ", player1));
                    Console.WriteLine("Player 2's deck: " + string.Join(", ", player2));
                }

                string seen = string.Format("{0}|{1}", string.Join(',', player1), string.Join(',', player2));
                bool newConfig = alreadySeen.Add(seen);
                if (!newConfig) {
                    if (outputExtraText) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Player 1 wins the round! (infinite loop prevention)");
                        Console.ResetColor();
                    } //else Console.Write(".");
                    player2.Clear();
                    break;
                }

                if (outputExtraText) {
                    Console.WriteLine("Player 1 plays: " + play1);
                    Console.WriteLine("Player 2 plays: " + play2);
                }

                if (play1 > play2) {
                    if (outputExtraText)
                        Console.WriteLine("Player 1 wins the round!");
                    player1.Enqueue(play1);
                    player1.Enqueue(play2);
                } else {
                    if (outputExtraText)
                        Console.WriteLine("Player 2 wins the round!");
                    player2.Enqueue(play2);
                    player2.Enqueue(play1);
                }

                if (outputExtraText)
                    Console.WriteLine();
            }

            Console.WriteLine("== Post-game results (1) ==");
            Console.WriteLine("Player 1's deck: " + string.Join(", ", player1));
            Console.WriteLine("Player 2's deck: " + string.Join(", ", player2));
            Console.WriteLine();

            winningPlayer = (player1.Any() ? 1 : 2);

            int partA = 0;
            int[] cards = player1.Union(player2).Reverse().ToArray();
            for (int i = 0; i < cards.Length; i++)
                partA += cards[i] * (i + 1);

            return partA;
        }

        private static int SolveB(List<int> deck1, List<int> deck2, bool outputExtraText, int gameNum, out int winningPlayer) {
            if (outputExtraText) {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("=== Game Layer {0} ===", gameNum);
                Console.ResetColor();
            }

            Queue<int> player1 = new Queue<int>(deck1);
            Queue<int> player2 = new Queue<int>(deck2);

            HashSet<string> alreadySeen = new HashSet<string>();

            int round = 0;
            while (player1.Any() && player2.Any()) {
                round++;

                if (outputExtraText) {
                    Console.WriteLine("\r\n-- Round {0} (Game {1}) --", round, gameNum);
                    Console.WriteLine("Player 1's deck: " + string.Join(", ", player1));
                    Console.WriteLine("Player 2's deck: " + string.Join(", ", player2));
                }

                string seen = string.Format("{0}|{1}", string.Join(',', player1), string.Join(',', player2));
                bool newConfig = alreadySeen.Add(seen);
                if (!newConfig) {
                    if (outputExtraText) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Player 1 wins the round! (previously seen configuration)");
                        Console.ResetColor();
                    } //else Console.Write(".");
                    player2.Clear();
                    break;
                }

                int winner = 0;
                int play1 = player1.Dequeue();
                int play2 = player2.Dequeue();
                if (outputExtraText) {
                    Console.WriteLine("Player 1 plays: " + play1);
                    Console.WriteLine("Player 2 plays: " + play2);
                }

                if (play1 <= player1.Count && play2 <= player2.Count) {
                    //Recursive
                    if (outputExtraText)
                        Console.WriteLine("Playing a sub-game to determine the winner...\r\n");
                    SolveB(player1.Take(play1).ToList(), player2.Take(play2).ToList(), outputExtraText, gameNum + 1, out winner);
                    if (outputExtraText)
                        Console.WriteLine("\r\n...anyway, back to game {0}.", gameNum);
                } else {
                    if (play1 > play2)
                        winner = 1;
                    else
                        winner = 2;
                }

                if (winner == 1) {
                    if (outputExtraText)
                        Console.WriteLine("Player 1 wins round {0} of game {1}!", round, gameNum);
                    player1.Enqueue(play1);
                    player1.Enqueue(play2);
                } else if (winner == 2) {
                    if (outputExtraText)
                        Console.WriteLine("Player 2 wins round {0} of game {1}!", round, gameNum);
                    player2.Enqueue(play2);
                    player2.Enqueue(play1);
                } else
                    throw new Exception();
            }

            winningPlayer = (player1.Any() ? 1 : 2);

            if (outputExtraText)
                Console.WriteLine("The winner of game {0} is player {1}!", gameNum, winningPlayer);

            if (gameNum == 1) {
                Console.WriteLine("\r\n== Post-game results (2) ==");
                Console.WriteLine("Player 1's deck: " + string.Join(", ", player1));
                Console.WriteLine("Player 2's deck: " + string.Join(", ", player2));
                Console.WriteLine();
            }

            int partB = 0;
            int[] cards = player1.Union(player2).Reverse().ToArray();
            for (int i = 0; i < cards.Length; i++)
                partB += cards[i] * (i + 1);

            return partB;
        }
    }
}
