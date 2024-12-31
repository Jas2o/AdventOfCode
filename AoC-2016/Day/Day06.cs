using System.Text;

namespace AoC.Day {
    public class Day06
    {
        public static void Run(string file) {
            Console.WriteLine("Day 6: Signals and Noise" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            StringBuilder sbA = new StringBuilder();
            StringBuilder sbB = new StringBuilder();
            Dictionary<(int, char), int> dictionary = new Dictionary<(int, char), int>();
            for (int pos = 0; pos < lines[0].Length; pos++) {
                foreach (string line in lines) {
                    char c = line[pos];
                    if (dictionary.ContainsKey((pos, c))) {
                        dictionary[(pos, c)]++;
                    } else {
                        dictionary.Add((pos, c), 1);
                    }
                }

                char most = dictionary.Where(x => x.Key.Item1 == pos).MaxBy(x => x.Value).Key.Item2;
                sbA.Append(most);

                char least = dictionary.Where(x => x.Key.Item1 == pos).MinBy(x => x.Value).Key.Item2;
                sbB.Append(least);
            }

            Console.WriteLine("Part 1: " + sbA.ToString());
            //Answer: mshjnduc
            Console.WriteLine("Part 2: " + sbB.ToString());
            //Answer: apfeeebz
        }
    }
}
