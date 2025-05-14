using System.Text;

namespace AoC.Day
{
    public class Day10
    {
        public static void Run(string file) {
            Console.WriteLine("Day 10: Syntax Scoring" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            char[] open = ['(', '[', '{', '<'];
            char[] close = [')', ']', '}', '>'];
            int[] lookupA = [3, 57, 1197, 25137];
            Dictionary<int, int> illegal = new Dictionary<int, int>() { {0, 0}, {1, 0}, {2, 0}, {3, 0}, };

            bool outputText = true;

            int partA = 0;
            List<long> scores = new List<long>();
            StringBuilder missing = new StringBuilder();
            foreach (string line in lines) {
                bool corrupted = false;
                Stack<char> stack = new Stack<char>();
                foreach(char c in line) {
                    if (open.Contains(c))
                        stack.Push(c);
                    else {
                        int idxOpen = Array.IndexOf(open, stack.Peek());
                        int idxClose = Array.IndexOf(close, c);
                        if (idxOpen == idxClose)
                            stack.Pop();
                        else {
                            if (outputText) {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine("{0} - Expected {1}, but found {2} instead.", line, close[idxOpen], close[idxClose]);
                                Console.ResetColor();
                            }
                            corrupted = true;
                            illegal[idxClose]++;
                            break;
                        }
                    }
                }

                if (!corrupted) {
                    long score = 0;
                    while (stack.Any()) {
                        char c = stack.Pop();
                        int idxOpen = Array.IndexOf(open, c);
                        if(outputText)
                            missing.Append(close[idxOpen]);
                        score *= 5;
                        score += (idxOpen + 1);
                    }
                    scores.Add(score);
                    if (outputText) {
                        Console.WriteLine("{0} - Complete by adding {1} - worth {2} points", line, missing.ToString(), score);
                        missing.Clear();
                    }
                }
            }

            foreach(KeyValuePair<int, int> pair in illegal)
                partA += pair.Value * lookupA[pair.Key];

            scores.Sort();
            long partB = scores[scores.Count / 2];

            if(outputText)
			    Console.WriteLine();

            Console.WriteLine("Part 1: " + partA);
            //Answer: 387363
            Console.WriteLine("Part 2: " + partB);
            //Answer: 4330777059
        }
    }
}
