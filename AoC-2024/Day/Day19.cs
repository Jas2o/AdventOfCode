using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace AoC.Day
{
    public class Day19
    {
        public static void Run(string file) {
            Console.WriteLine("Day 19: Linen Layout" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            string[] available = lines[0].Split(", ");
            List<string> listDesired = new List<string>();
            for (int y = 2; y < lines.Length; y++) {
                listDesired.Add(lines[y]);
            }

            List<KnownPattern> memory = new List<KnownPattern>();

            int numPossible = 0;
            long numCombinations = 0;
            foreach (string desired in listDesired) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(desired);

                long combinations = DFS(memory, available, desired);
                if (combinations > 0) {
                    numPossible++;
                    numCombinations += combinations;

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(" possible in ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(combinations);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(" ways.");
                    //Console.WriteLine("{0} possible im {1} ways.", desired, combinations);
                } else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" impossible");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(".");
                }
                Console.WriteLine();
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + numPossible);
            //Answer: 269
            Console.WriteLine("Part 2: " + numCombinations);
            //Answer: 758839075658876
        }

        private static long DFS(List<KnownPattern> memory, string[] available, string desired) {
            if (desired.Length == 0)
                return 1;
            KnownPattern? known = memory.Find(p => p.Text == desired);
            if(known != null)
                return known.Count;

            long numWays = 0;
            foreach (string towel in available) {
                if (desired.StartsWith(towel)) {
                    string remainder = desired.Substring(towel.Length);
                    numWays += DFS(memory, available, remainder);
                }
            }

            if (numWays > 0) {
                known = new KnownPattern(desired, numWays);
                memory.Add(known);
            }

            return numWays;
        }

        private class KnownPattern {

            public string Text;
            public long Count;

            public KnownPattern(string text, long count) {
                Text = text;
                Count = count;
            }

            public override string ToString() {
                return string.Format("{0} {1}", Text, Count);
            }
        }
    }
}
