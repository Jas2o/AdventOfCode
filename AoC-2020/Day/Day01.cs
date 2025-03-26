namespace AoC.Day
{
    public class Day01
    {
        public static void Run(string file) {
            Console.WriteLine("Day 1: Report Repair" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            int[] ints = Array.ConvertAll(lines, int.Parse);

            int partA = SolveA(ints);
            int partB = SolveB(ints);

			Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 41979
            Console.WriteLine("Part 2: " + partB);
            //Answer: 193416912
        }

        private static int SolveA(int[] ints) {
            for (int i = 0; i < ints.Length; i++) {
                for (int k = i + 1; k < ints.Length - 1; k++) {
                    if (i == k)
                        continue;
                    if (ints[i] + ints[k] == 2020) {
                        Console.WriteLine("{0} + {1}", ints[i], ints[k]);
                        return ints[i] * ints[k];
                    }
                }
            }

            return 0;
        }

        private static int SolveB(int[] ints) {
            for (int i = 0; i < ints.Length; i++) {
                for (int k = 1; k < ints.Length - 1; k++) {
                    if (i == k)
                        continue;
                    for (int m = 2; m < ints.Length - 2; m++) {
                        if (k == m || m == i)
                            continue;
                        if (ints[i] + ints[k] + ints[m] == 2020) {
                            Console.WriteLine("{0} + {1} + {2}", ints[i], ints[k], ints[m]);
                            return ints[i] * ints[k] * ints[m];
                        }
                    }
                }
            }

            return 0;
        }
    }
}
