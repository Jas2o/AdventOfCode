namespace AoC.Day {
    public class Day02
    {
        private static string alphabet = "abcdefghijklmnopqrstuvwxyz";

        public static void Run(string file) {
            Console.WriteLine("Day 2: Inventory Management System" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            int partA = Part1(lines);
            string partB = Part2(lines);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 7936
            Console.WriteLine("Part 2: " + partB);
            //Answer: lnfqdscwjyteorambzuchrgpx
        }

        private static int Part1(string[] lines) {
            int two = 0;
            int three = 0;

            foreach (string line in lines) {
                Dictionary<char, int> check = new Dictionary<char, int>();
                foreach (char c in alphabet) {
                    int count = line.Count(x => x == c);
                    if (count > 1)
                        check.Add(c, count);
                }

                if (check.Values.Any(x => x == 2))
                    two++;
                if (check.Values.Any(x => x == 3))
                    three++;
            }

            return two * three;
        }

        private static string Part2(string[] lines) {
            for (int i = 0; i < lines.Length; i++) {
                for (int k = i + 1; k < lines.Length; k++) {
                    if (i == k)
                        continue;

                    int diff = 0;
                    char c = '?';
                    for (int z = 0; z < lines[i].Length; z++) {
                        if (lines[i][z] != lines[k][z]) {
                            diff++;
                            c = lines[i][z];
                        }
                    }

                    if(diff == 1)
                        return lines[i].Replace(c.ToString(), "");
                }
            }
            return string.Empty;
        }
    }
}
