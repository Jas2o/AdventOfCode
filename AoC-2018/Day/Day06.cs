namespace AoC.Day
{
    public class Day06
    {
        public static void Run(string file) {
            Console.WriteLine("Day 6: Chronal Coordinates" + Environment.NewLine);

            Dictionary<(int, int), int> nodes = new Dictionary<(int, int), int>();
            Dictionary<(int, int), int> grid = new Dictionary<(int, int), int>();
            Dictionary<(int, int), int> safe = new Dictionary<(int, int), int>();

            //Populate nodes.
            string[] lines = File.ReadAllLines(file);
            int id = 1;
            foreach (string line in lines) {
                int[] nums = Array.ConvertAll(line.Split(','), int.Parse);
                nodes.Add((nums[0], nums[1]), id);
                id++;
            }

            int minY = nodes.Min(n => n.Key.Item2) - 1;
            int minX = nodes.Min(n => n.Key.Item1) - 1;
            int maxY = nodes.Max(n => n.Key.Item2) + 1;
            int maxX = nodes.Max(n => n.Key.Item1) + 1;

            int within = (lines.Length < 10 ? 32 : 10000);

            //Populate grid and safe.
            //Within known space, for each coordinate check the manhattan distances to nodes.
            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    int bestManhattan = int.MaxValue; //Part 1
                    int totalP2 = 0; //Part 2
                    foreach(var node in nodes) {
                        int manhattan = Math.Abs(node.Key.Item1 - x) + Math.Abs(node.Key.Item2 - y);
                        totalP2 += manhattan;
                        if (manhattan < bestManhattan) {
                            bestManhattan = manhattan;
                            grid[(x, y)] = node.Value;
                        } else if (manhattan == bestManhattan)
                            grid[(x, y)] = 0;
                    }

                    //Part 2
                    if (totalP2 < within)
                        safe[(x, y)] = 1;
                    else
                        safe[(x, y)] = 0;
                }
            }

            if (Math.Abs(maxX - minX) < Console.WindowWidth) {
                Console.WriteLine("Nodes:");
                DrawMap(nodes, 1);
                Console.WriteLine("If safe:");
                DrawMap(safe);
            }

            //Check along the perimeter and remove grid matches (cause they're infinite).
            for (int x = minX; x <= maxX; x++) {
                int xMinY = grid[(x, minY)];
                int xMaxY = grid[(x, maxY)];

                var matches = grid.Where(n => n.Value != 0 && (n.Value == xMinY || n.Value == xMaxY));
                foreach(var match in matches) {
                    grid[match.Key] = 0;
                }
            }
            for (int y = minY; y <= maxY; y++) {
                int yMinX = grid[(minX, y)];
                int yMaxX = grid[(maxX, y)];

                var matches = grid.Where(n => n.Value != 0 && (n.Value == yMinX || n.Value == yMaxX));
                foreach (var match in matches) {
                    grid[match.Key] = 0;
                }
            }

            //Of what's left in grid, check which is most common.
            int partA = 0;
            int[] vals = grid.Values.Where(v => v != 0).Distinct().ToArray();
            foreach(int val in vals) {
                int count = grid.Values.Where(v => v == val).Count();
                if (count > partA)
                    partA = count;
            }

            if (Math.Abs(maxX - minX) < Console.WindowWidth / 2) {
                Console.WriteLine("If dangerous:");
                DrawMap(grid);
            }

            int partB = safe.Values.Where(v => v == 1).Count();

            Console.WriteLine("Part 1: " + partA);
            //Answer: 6047
            Console.WriteLine("Part 2: " + partB);
            //Answer: 46320
        }

        private static void DrawMap(Dictionary<(int, int), int> nodes, int edge=0) {
            int minY = nodes.Min(n => n.Key.Item2) - edge;
            int minX = nodes.Min(n => n.Key.Item1) - edge;
            int maxY = nodes.Max(n => n.Key.Item2) + edge;
            int maxX = nodes.Max(n => n.Key.Item1) + edge;

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    nodes.TryGetValue((x, y), out int val);
                    if (val != 0)
                        Console.Write(val);
                    else
                        Console.Write(".");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
