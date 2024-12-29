using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AoC.Day
{
    public class Day19
    {
        public static void Run(string file) {
            Console.WriteLine("Day 19: Medicine for Rudolph" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            string original = lines.Last();
            List<Tuple<string, string>> listReplacements = new List<Tuple<string, string>>();
            foreach (string line in lines) {
                if (line.Length == 0)
                    break;
                string[] fields = line.Split(" => ");
                listReplacements.Add(new Tuple<string, string>(fields[0], fields[1]));
            }

            Dictionary<string, int> memory = new Dictionary<string, int>();
            GenerateUniqueWithOneReplacement(memory, listReplacements, original);
            foreach(KeyValuePair<string, int> pair in memory) {
                Console.WriteLine(pair.Key);
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + memory.Count);
            //Answer: 509
            Console.WriteLine("Part 2: " + 0);
            //Answer: 
        }

        private static void GenerateUniqueWithOneReplacement(Dictionary<string, int> memory, List<Tuple<string, string>> replacements, string desired) {
            if (desired.Length == 0)
                return;// 1;

            //int numWays = 0;
            Console.WriteLine("## " + desired);
            foreach (Tuple<string, string> replacement in replacements) {
                for (int i = 0; i < desired.Length; i++) {
                    if (!desired.Substring(i).StartsWith(replacement.Item1))
                        continue;
                    string molecule = string.Format("{0}{1}{2}", desired.Substring(0, i), replacement.Item2, desired.Substring(i + replacement.Item1.Length));

                    if (memory.ContainsKey(molecule))
                        memory[molecule]++;
                    else
                        memory[molecule] = 1;
                }
            }

            //return numWays;
        }
    }
}
