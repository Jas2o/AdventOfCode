namespace AoC.Day
{
    public class Day11
    {
        //This one is kinda slow, so many for loops.

        public static void Run(string file) {
            Console.WriteLine("Day 11: Chronal Charge" + Environment.NewLine);

            int min = 1;
            int max = 300;

            string input = File.ReadAllText(file);
            int gridSerial = int.Parse(input);

            Dictionary<(int, int), FuelCell> dictionary = new Dictionary<(int, int), FuelCell>();
            for (int y = min; y <= max; y++) {
                for (int x = min; x <= max; x++) {
                    FuelCell fuelCell = new FuelCell(x, y, gridSerial);
                    dictionary.Add((x, y), fuelCell);
                }
            }

            (int p1_bestTotalPower, int p1_bestX, int p1_bestY) = Solve(dictionary, min, max, 3);

            int p2_bestTotalPower = 0;
            int p2_bestX = 0;
            int p2_bestY = 0;
            int p2_bestSize = 0;
            for (int size = 1; size <= max; size++) {
                (int bestTotalPower, int bestX, int bestY) = Solve(dictionary, min, max, size);

                if (bestTotalPower > p2_bestTotalPower) {
                    p2_bestTotalPower = bestTotalPower;
                    p2_bestX = bestX;
                    p2_bestY = bestY;
                    p2_bestSize = size;
                } else if(bestTotalPower == 0) {
                    //We can't end if it was lower than best, because it's possible for there to be a later best.
                    break;
                }

                Console.WriteLine("{0},{1},{2} (with a total power of {3})", bestX, bestY, size, bestTotalPower);
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: {0},{1} (with a total power of {2})", p1_bestX, p1_bestY, p1_bestTotalPower);
            //Answer: 235,85
            Console.WriteLine("Part 2: {0},{1},{2} (with a total power of {3})", p2_bestX, p2_bestY, p2_bestSize, p2_bestTotalPower);
            //Answer: 233,40,13
        }

        private static (int power, int x, int y) Solve(Dictionary<(int, int), FuelCell> dictionary, int min, int max, int size) {
            int bestTotalPower = 0;
            int bestX = -1;
            int bestY = -1;
            for (int y = min; y <= max - size - 1; y++) {
                for (int x = min; x <= max - size - 1; x++) {
                    List<FuelCell> cells = new List<FuelCell>();
                    for (int subY = 0; subY < size; subY++) {
                        for (int subX = 0; subX < size; subX++) {
                            cells.Add(dictionary[(x + subX, y + subY)]);
                        }
                    }

                    /*
                    List<FuelCell> cells = new List<FuelCell>() {
                        dictionary[(x, y+0)], dictionary[(x+1, y+0)], dictionary[(x+2, y+0)],
                        dictionary[(x, y+1)], dictionary[(x+1, y+1)], dictionary[(x+2, y+1)],
                        dictionary[(x, y+2)], dictionary[(x+1, y+2)], dictionary[(x+2, y+2)]
                    };
                    */

                    int total = cells.Sum(c => c.PowerLevel);
                    if (total > bestTotalPower) {
                        bestTotalPower = total;
                        bestX = x;
                        bestY = y;
                    }
                }
            }

            return (bestTotalPower, bestX, bestY);
        }

        private class FuelCell {
            public int X;
            public int Y;
            public int PowerLevel;
            public int RackID;

            public FuelCell(int x, int y, int gridSerial) {
                X = x;
                Y = y;

                RackID = x + 10;
                PowerLevel = ((RackID * y) + gridSerial) * RackID;
                PowerLevel /= 100;
                PowerLevel %= 10;
                PowerLevel -= 5;
            }

        }
    }
}
