namespace AoC.Day
{
    public class Day10
    {
        public static void Run(string file) {
            Console.WriteLine("Day 10: The Stars Align" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            List<PointV> points = new List<PointV>();
            foreach (string line in lines) {
                PointV p = new PointV(line);
                points.Add(p);
            }

            int minX = -1;
            int minY = -1;
            int maxX = 0;
            int maxY = 0;
            int seconds = 0;
            int minDistance = int.MaxValue;
            while(true) {
                seconds++;
                foreach (PointV p in points)
                    p.Simulate(1);

                minX = points.Min(n => n.PosX);
                minY = points.Min(n => n.PosY);
                maxX = points.Max(n => n.PosX);
                maxY = points.Max(n => n.PosY);

                int diffX = maxX - minX;
                int diffY = maxY - minY;
                //Console.WriteLine("{0},{1} to {2},{3} is {4},{5}", minX, minY, maxX, maxY, diffX, diffY);
                if (diffX < minDistance) {
                    minDistance = diffX;
                } else {
                    //Rewind the last
                    seconds--;
                    foreach (PointV p in points)
                        p.Simulate(-1);
                    break;
                }
            }
            
            //if(maxX < 500 && maxY < 500)
            DrawMap(points, minX, minY, maxX, maxY);

            Console.WriteLine("Part 1: (you'll need to use your eyes to read the display above)");
            //Answer: GFANEHKJ
            Console.WriteLine("Part 2: " + seconds);
            //Answer: 10086
        }

        private static void DrawMap(List<PointV> points, int minX, int minY, int maxX, int maxY) {
            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    bool find = points.Any(p => p.PosX == x && p.PosY == y);
                    if (find)
                        Console.Write('#');
                    else
                        Console.Write(".");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        //Based on 2024 Day 14
        private class PointV {
            public int InitialPosX;
            public int InitialPosY;
            public int PosX;
            public int PosY;
            public int VelocityX;
            public int VelocityY;

            public PointV(string line) {
                int firstEq = line.IndexOf('=') + 2;
                int firstComma = line.IndexOf(',');
                int firstGT = line.IndexOf('>');
                int lastEq = line.LastIndexOf('=') + 2;
                int lastComma = line.LastIndexOf(',');
                int lastGT = line.LastIndexOf('>');

                string pX = line.Substring(firstEq, firstComma - firstEq);
                string pY = line.Substring(firstComma + 1, firstGT - firstComma - 1);
                string vX = line.Substring(lastEq, lastComma - lastEq);
                string vY = line.Substring(lastComma + 1, lastGT - lastComma - 1);

                PosX = InitialPosX = int.Parse(pX);
                PosY = InitialPosY = int.Parse(pY);
                VelocityX = int.Parse(vX);
                VelocityY = int.Parse(vY);
            }

            public void Reset() {
                PosX = InitialPosX;
                PosY = InitialPosY;
            }

            public void Simulate(int seconds) {
                PosX += VelocityX * seconds;
                PosY += VelocityY * seconds;
            }
        }
    }
}
