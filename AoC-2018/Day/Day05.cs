namespace AoC.Day
{
    public class Day05
    {
        private static string alphabet = "abcdefghijklmnopqrstuvwxyz";

        public static void Run(string file) {
            Console.WriteLine("Day 5: Alchemical Reduction" + Environment.NewLine);

			string input = File.ReadAllText(file);

            List<char> text = input.ToCharArray().ToList();
            React(text);
            int partA = text.Count;

            int partB = partA;
            char partB_type1 = '?';
            char partB_type2 = '?';
            foreach(char lower in alphabet) {
                char upper = (char)(lower - 32);
                List<char> trial = text.ToList();
                trial.RemoveAll(x => x == upper || x == lower);
                React(trial);
                int res = trial.Count;
                if (res < partB) {
                    partB = res;
                    partB_type1 = upper;
                    partB_type2 = lower;
                }
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 9116
            Console.WriteLine("Part 2: {0} (removing all {1}/{2})", partB, partB_type1, partB_type2);
            //Answer: 6890
        }

        private static void React(List<char> text) {
            while (true) {
                bool didRemoval = false;
                for (int i = 1; i < text.Count; i++) {
                    char prev = text[i - 1];
                    char cur = text[i];

                    char upper = (char)Math.Min(prev, cur);
                    char lower = (char)Math.Max(prev, cur);

                    if (upper + 32 == lower) {
                        text.RemoveAt(i);
                        text.RemoveAt(i - 1);
                        didRemoval = true;
                        break;
                    }
                }

                if (!didRemoval)
                    break;
            }
        }
    }
}
