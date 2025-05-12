namespace AoC.Day
{
    public class Day06
    {
        public static void Run(string file) {
            Console.WriteLine("Day 6: Lanternfish" + Environment.NewLine);

			string input = File.ReadAllText(file);
            int[] initial = Array.ConvertAll(input.Split(','), int.Parse);

            Dictionary<int, long> fish = new Dictionary<int, long>();
            foreach(int num in initial) {
                if (!fish.TryAdd(num, 1))
                    fish[num]++;
            }

            Console.WriteLine("     Initial: {0} (total: {1})", FishToText(fish), fish.Values.Sum());
            for (int day = 1; day <= 80; day++) {
                fish = Simulate(fish);
                Console.WriteLine("After day {0,2}: {1} (total: {2})", day, FishToText(fish), fish.Values.Sum());
            }
            long partA = fish.Values.Sum();

            for (int day = 81; day <= 256; day++)
                fish = Simulate(fish);
            long partB = fish.Values.Sum();
            Console.WriteLine("\r\nAfter day {0,2}: {1} (total: {2})\r\n", 256, FishToText(fish), partB);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 393019
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1757714216975
        }

        private static Dictionary<int, long> Simulate(Dictionary<int, long> fish) {
            Dictionary<int, long> next = new Dictionary<int, long>();
            foreach (KeyValuePair<int, long> pair in fish) {
                if (pair.Key == 0) {
                    if (next.ContainsKey(6))
                        next[6] += pair.Value;
                    else
                        next[6] = pair.Value;
                    next[8] = pair.Value;
                } else {
                    int n = pair.Key - 1;
                    if (next.ContainsKey(n))
                        next[n] += pair.Value;
                    else
                        next[n] = pair.Value;
                }
            }

            return next;
        }

        private static string FishToText(Dictionary<int, long> fish) {
            Dictionary<int, long> sorted = fish.OrderBy(f => f.Key).ToDictionary();
            List<string> txt = new List<string>();
            foreach (KeyValuePair<int, long> pair in sorted)
                txt.Add(string.Format("{0}:{1}", pair.Key, pair.Value));

            return string.Join(", ", txt);
        }
    }
}
