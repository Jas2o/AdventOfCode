using System.Text;

namespace AoC.Day
{
    public class Day14
    {
        public static void Run(string file) {
            Console.WriteLine("Day 14: Extended Polymerization" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            string template = lines[0];
            Dictionary<(char, char), char> rules = new Dictionary<(char, char), char>();
            for (int i = 2; i < lines.Length; i++) {
                char left = lines[i][0];
                char right = lines[i][1];
                char mid = lines[i][6];
                rules.Add((left, right), mid);
            }

            //int partA = SolveA_Slow(template, rules);
            long partA = Solve(template, rules, 10);
            long partB = Solve(template, rules, 40);
            
            Console.WriteLine("Part 1: " + partA);
            //Answer: 2587
            Console.WriteLine("Part 2: " + partB);
            //Answer: 3318837563123
        }

        private static long Solve(string template, Dictionary<(char, char), char> rules, int stepsLimit) {
            Dictionary<(char, char), long> countTemplate = new Dictionary<(char, char), long>();
            foreach(KeyValuePair<(char, char), char> rule in rules)
                countTemplate.Add(rule.Key, 0);

            Dictionary<(char, char), long> countWorking1 = countTemplate.ToDictionary();
            for (int i = 1; i < template.Length; i++)
                countWorking1[(template[i-1], template[i])]++;

            for (int step = 1; step <= stepsLimit; step++) {
                Dictionary<(char, char), long> countWorking2 = countTemplate.ToDictionary();
                foreach(KeyValuePair<(char, char), long> outside in countWorking1) {
                    if (outside.Value == 0)
                        continue;
                    char mid = rules[outside.Key];
                    (char, char) left = (outside.Key.Item1, mid);
                    (char, char) right = (mid, outside.Key.Item2);
                    countWorking2[left] += outside.Value;
                    countWorking2[right] += outside.Value;
                }
                countWorking1 = countWorking2;
            }

            IEnumerable<char> letters = countWorking1.Select(c => c.Key.Item1).Distinct();
            Dictionary<char, long> counts = new Dictionary<char, long>();
            foreach (char letter in letters) {
                long c1 = countWorking1.Where(c => c.Key.Item1 == letter).Sum(c => c.Value);
                long c2 = countWorking1.Where(c => c.Key.Item1 == letter).Sum(c => c.Value);
                long half = (c1 + c2) / 2;
                counts.Add(letter, half);
            }
            counts[template.Last()]++;
            return counts.Max(c => c.Value) - counts.Min(c => c.Value);
        }

        private static int SolveA_Slow(string template, Dictionary<(char, char), char> rules) {
            Console.WriteLine("Template:     " + template);
            string current = template;
            for (int step = 1; step < 11; step++) {
                if (current.Length > 99)
                    Console.WriteLine();

                StringBuilder sb = new StringBuilder();
                char prev = current[0];
                for (int i = 1; i < current.Length; i++) {
                    char next = current[i];
                    char mid = rules[(prev, next)];
                    sb.Append(prev);
                    sb.Append(mid);
                    prev = next;
                }
                sb.Append(prev);
                current = sb.ToString();
                Console.WriteLine("After step {0}: {1}", step, current);
            }
            Console.WriteLine();

            IEnumerable<char> letters = current.ToCharArray().Distinct();
            Dictionary<char, int> counts = new Dictionary<char, int>();
            foreach (char letter in letters) {
                int c = current.Count(l => l == letter);
                counts.Add(letter, c);
            }
            return counts.Max(c => c.Value) - counts.Min(c => c.Value);
        }
    }
}
