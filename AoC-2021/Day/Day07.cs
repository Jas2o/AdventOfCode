namespace AoC.Day
{
    public class Day07
    {
        public static void Run(string file) {
            Console.WriteLine("Day 7: The Treachery of Whales" + Environment.NewLine);

			string input = File.ReadAllText(file);
            int[] crabs = Array.ConvertAll(input.Split(','), int.Parse);
            int min = crabs.Min();
            int max = crabs.Max();

            int partA = int.MaxValue;
            int partB = int.MaxValue;
            int partA_dest = 0;
            int partB_dest = 0;

            for (int dest = min; dest <= max; dest++) {
                int fuelA = 0;
                int fuelB = 0;
                foreach (int crab in crabs) {
                    int diff = Math.Abs(dest - crab);
                    fuelA += diff;
                    if (fuelB != int.MaxValue) {
                        for (int d = 1; d <= diff; d++)
                            fuelB += d;
                        if (fuelB > partB) {
                            //Skip the rest of adding to B
                            fuelB = int.MaxValue;
                        }
                    }
                }

                if (fuelA < partA) {
                    partA = fuelA;
                    partA_dest = dest;
                }

                if (fuelB < partB) {
                    partB = fuelB;
                    partB_dest = dest;
                }
            }

            Console.WriteLine("Part 1: {0} (at {1})", partA, partA_dest);
            //Answer: 341558 (at 371)
            Console.WriteLine("Part 2: {0} (at {1})", partB, partB_dest);
            //Answer: 93214037 (at 484)
        }
    }
}
