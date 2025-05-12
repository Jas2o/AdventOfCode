using System.Data;

namespace AoC.Day
{
    public class Day05
    {
        public static void Run(string file) {
            Console.WriteLine("Day 5: Hydrothermal Venture" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            Dictionary<(int x, int y), int> ventsA = new Dictionary<(int x, int y), int>();
            Dictionary<(int x, int y), int> ventsB = new Dictionary<(int x, int y), int>();

            foreach (string line in lines) {
                int spaceLeft = line.IndexOf(' ');
                int spaceRight = line.LastIndexOf(' ') + 1;
                string[] left = line.Substring(0, spaceLeft).Split(',');
                string[] right = line.Substring(spaceRight).Split(',');

                int x1 = int.Parse(left[0]);
                int y1 = int.Parse(left[1]);
                int x2 = int.Parse(right[0]);
                int y2 = int.Parse(right[1]);

                int offsetX = (x2 == x1 ? 0 : (x2 > x1 ? 1 : -1));
                int offsetY = (y2 == y1 ? 0 : (y2 > y1 ? 1 : -1));
                bool diag = (x1 != x2 && y1 != y2);

                while (true) {
                    (int x, int y) key = (x1, y1);

                    if (!diag) {
                        if (ventsA.ContainsKey(key))
                            ventsA[key]++;
                        else
                            ventsA[key] = 1;
                    }

                    if (ventsB.ContainsKey(key))
                        ventsB[key]++;
                    else
                        ventsB[key] = 1;

                    if (x1 == x2 && y1 == y2)
                        break;
                    x1 += offsetX;
                    y1 += offsetY;
                }
            }

            DrawMapIfFits(ventsA);
            DrawMapIfFits(ventsB);

            int partA = ventsA.Values.Where(v => v > 1).Count();
            int partB = ventsB.Values.Where(v => v > 1).Count();

            Console.WriteLine("Part 1: " + partA);
            //Answer: 6666
            Console.WriteLine("Part 2: " + partB);
            //Answer: 19081
        }

        private static void DrawMapIfFits(Dictionary<(int x, int y), int> nodes) {
            int minY = nodes.Min(n => n.Key.y);
            int minX = nodes.Min(n => n.Key.x);
            int maxY = nodes.Max(n => n.Key.y);
            int maxX = nodes.Max(n => n.Key.x);

            if (Console.WindowWidth < maxX - minX)
                return;

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    if (nodes.ContainsKey((x, y))) {
                        int c = nodes[(x, y)];
                        Console.Write(c);
                    } else
                        Console.Write(".");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
