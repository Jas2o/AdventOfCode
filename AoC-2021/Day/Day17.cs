namespace AoC.Day
{
    public class Day17
    {
        public static void Run(string file) {
            Console.WriteLine("Day 17: Trick Shot" + Environment.NewLine);

			string input = File.ReadAllText(file);
            Console.WriteLine(input + Environment.NewLine);

            string sY = input.Substring(input.LastIndexOf('=') + 1);
            string sX = input.Substring(input.IndexOf('=') + 1);
            sX = sX.Substring(0, sX.IndexOf(','));
            int[] nX = Array.ConvertAll(sX.Split(".."), int.Parse);
            int[] nY = Array.ConvertAll(sY.Split(".."), int.Parse);
            int limitY = Math.Abs(nY[0]);

            int partA = 0;
            int partB = 0;
            for(int velocityX = 0; velocityX <= nX[1]; velocityX++) {
                for (int velocityY = nY[0]; velocityY <= limitY; velocityY++) {
                    bool success = Test(velocityX, velocityY, nX, nY, out int bestY, out int probeX, out int probeY);
                    if (!success)
                        continue;
                    partB++;
                    if (bestY > partA) {
                        partA = bestY;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    Console.WriteLine("{0},{1} reached y:{2}, within at {3},{4}", velocityX, velocityY, bestY, probeX, probeY);
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 13203 (velocity 16,162, within target area at 91,-163)
            Console.WriteLine("Part 2: " + partB);
            //Answer: 5644
        }

        private static bool Test(int velocityX, int velocityY, int[] nX, int[] nY, out int bestY, out int probeX, out int probeY) {
            bestY = int.MinValue;
            probeX = 0;
            probeY = 0;

            while (true) {
                probeX += velocityX;
                probeY += velocityY;
                if (probeY > bestY)
                    bestY = probeY;

                if (velocityX > 0)
                    velocityX--;
                else if (velocityX < 0)
                    velocityX++;
                velocityY--;

                if (nX[0] <= probeX && probeX <= nX[1] && nY[0] <= probeY && probeY <= nY[1])
                    return true;
                if (probeY < nY[0])
                    return false;
            }
        }
    }
}
