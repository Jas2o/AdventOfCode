using AoC.Shared;

namespace AoC.Day {
    public class Day19
    {
        public static void Run(string file) {
            Console.WriteLine("Day 19: Medicine for Rudolph" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            string original = lines.Last();
            List<Tuple<string, string, int, int>> listReplacements = new List<Tuple<string, string, int, int>>();
            foreach (string line in lines) {
                if (line.Length == 0)
                    break;
                string[] fields = line.Split(" => ");
                listReplacements.Add(new Tuple<string, string, int, int>(fields[0], fields[1], fields[0].Length, fields[1].Length));
            }

            //Part 1
            Dictionary<string, int> memory = new Dictionary<string, int>();
            Console.WriteLine("## " + original);
            GenerateUniqueWithOneReplacement(memory, listReplacements, original);
            foreach(KeyValuePair<string, int> pair in memory) {
                Console.WriteLine(pair.Key);
            }
            
            Console.WriteLine(Environment.NewLine + "/////" + Environment.NewLine);

            //Part 2
            int part2 = 0;
            Console.WriteLine("## " + original);
            while (original.Length > 1) {
                int max = listReplacements.Max(x => x.Item4);
                int min = listReplacements.Min(x => x.Item4);
                bool foundReplacement = false;
                for (int m = max; m > 0; m--) {
                    List<Tuple<string, string, int, int>> options = listReplacements.FindAll(x => x.Item4 == m).OrderBy(x => x.Item3).ToList();
                    foreach (Tuple<string, string, int, int> option in options) {
                        if (original.Contains(option.Item2)) {
                            original = original.ReplaceFirst(option.Item2, option.Item1);
                            foundReplacement = true;
                            Console.WriteLine(". " + original);
                            part2++;
                        }
                    }
                }

                if (!foundReplacement)
                    break;
            }
            if (original != "e") {
                Console.WriteLine("Part 2 failed :(");
                part2 = 0;
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + memory.Count);
            //Answer: 509
            Console.WriteLine("Part 2: " + part2);
            //Answer: 195
        }

        private static void GenerateUniqueWithOneReplacement(Dictionary<string, int> memory, List<Tuple<string, string, int, int>> replacements, string desired) {
            if (desired.Length == 0)
                return;

            foreach (Tuple<string, string, int, int> replacement in replacements) {
                for (int i = 0; i < desired.Length; i++) {
                    if (!desired.Substring(i).StartsWith(replacement.Item1))
                        continue;
                    string molecule = string.Format("{0}{1}{2}", desired.Substring(0, i), replacement.Item2, desired.Substring(i + replacement.Item3));

                    if (memory.ContainsKey(molecule))
                        memory[molecule]++;
                    else
                        memory[molecule] = 1;
                }
            }
        }
    }
}
