namespace AoC.Day
{
    public class Day06
    {
        public static void Run(string file) {
            Console.WriteLine("Day 6: Tuning Trouble");

			string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                //There's only one line in the real input.
                Console.WriteLine();

                int partA = -1;
                int partB = -1;
                int start = -1;
                while(start < line.Length) {
                    start++;
                    if (partA == -1) {
                        IEnumerable<char> window = line.Substring(start, 4).ToCharArray().Distinct();
                        if (window.Count() == 4)
                            partA = start + 4;
                    }

                    if (partB == -1) {
                        IEnumerable<char> window = line.Substring(start, 14).ToCharArray().Distinct();
                        if (window.Count() == 14) {
                            partB = start + 14;
                            break;
                        }
                    }
                }

                Console.WriteLine("Part 1: " + partA);
                //Answer: 1850
                Console.WriteLine("Part 2: " + partB);
                //Answer: 2823
            }
        }
    }
}
