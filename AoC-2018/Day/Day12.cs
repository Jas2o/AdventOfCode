using System.Text;

namespace AoC.Day
{
    public class Day12
    {
        public static void Run(string file) {
            Console.WriteLine("Day 12: Subterranean Sustainability" + Environment.NewLine);

            //Setup
            Dictionary<string, char> transform = new Dictionary<string, char>();
			string[] lines = File.ReadAllLines(file);
            string initial = lines[0].Substring(15);
            foreach(string line in lines.Skip(2))
                transform.Add(line.Substring(0, 5), line.Last());
            Dictionary<long, char> previous = new Dictionary<long, char>();
            for(int i = 0; i < initial.Length; i++)
                previous.Add(i, initial[i]);

            //Part 1
            for (int gen = 1; gen <= 20; gen++)
                DoGeneration(ref previous, transform);
            long partA = previous.Where(p => p.Value == '#').Sum(p => p.Key);

            //Part 2
            long partB = 0;
            long genTotal = 50000000000;
            string previousCycle = string.Empty;
            long previousScore = 0;
            bool winWasTooSmall = false;
            for (long gen = 21; gen <= genTotal; gen++) {
                DoGeneration(ref previous, transform);

                StringBuilder sb = new StringBuilder();
                long low = previous.Where(p => p.Value == '#').Min(p => p.Key);
                long high = previous.Where(p => p.Value == '#').Max(p => p.Key);
                for (long i = low; i <= high; i++) {
                    if (!previous.ContainsKey(i))
                        previous[i] = '.';
                    sb.Append(previous[i]);
                }

                string thisCycle = sb.ToString();
                long thisScore = previous.Where(p => p.Value == '#').Sum(p => p.Key);

                if (thisCycle.Length < Console.WindowWidth)
                    Console.WriteLine(thisCycle);
                else {
                    if (!winWasTooSmall) {
                        winWasTooSmall = true;
                        Console.WriteLine("(window too small to show the rest)");
                    }
                }

                if (thisCycle == previousCycle) {
                    long diff = thisScore - previousScore;
                    partB = thisScore + (genTotal - gen) * diff;
                    break;
                }
                previousCycle = thisCycle;
                previousScore = thisScore;
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 3051
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1300000000669
        }

        private static void DoGeneration(ref Dictionary<long, char> previous, Dictionary<string, char> transform) {
            long low = previous.Keys.Min() - 4;
            long high = previous.Keys.Max() + 4;
            for (long i = low; i <= high; i++) {
                if (!previous.ContainsKey(i))
                    previous[i] = '.';
            }

            Dictionary<long, char> state = new Dictionary<long, char>();
            foreach (KeyValuePair<long, char> pair in previous) {
                previous.TryGetValue(pair.Key - 2, out char s1);
                previous.TryGetValue(pair.Key - 1, out char s2);
                previous.TryGetValue(pair.Key + 1, out char s4);
                previous.TryGetValue(pair.Key + 2, out char s5);

                string five = string.Format("{0}{1}{2}{3}{4}", s1, s2, pair.Value, s4, s5).Replace('\0', '.');
                bool has = transform.TryGetValue(five, out char t);
                if (has)
                    state[pair.Key] = t;
            }

            previous = state;
        }
    }
}
