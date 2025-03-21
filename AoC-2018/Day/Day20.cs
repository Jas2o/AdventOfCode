using AoC.Graph;
using System.Text;

namespace AoC.Day
{
    public class Day20
    {
        public static void Run(string file) {
            Console.WriteLine("Day 20: A Regular Map" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            //Intention is to have multiple inputs in the test file separated by blank lines.
            foreach (string line in lines) {
                if (!line.StartsWith('^') || !line.EndsWith('$'))
                    continue;

                Console.WriteLine("{0}\r\n", (line.Length > 50 ? "Length: " + line.Length : line));

                //Build map
                Dictionary<(int, int), char> grid = new Dictionary<(int, int), char>();
                grid[(0, 0)] = 'X';
                Build(grid, line, 0, 0);

                //Convert map
                List<DNode> nodes = new List<DNode>();
                foreach(KeyValuePair<(int, int), char> g in grid)
                    nodes.Add(new DNode(g.Key.Item1, g.Key.Item2, int.MaxValue, g.Value));

                //Get the answers
                DNode nodeStart = nodes.Find(n => n.X == 0 && n.Y == 0);
                nodeStart.Distance = 0;
                DNode.Dijkstra(nodes);
                DNode nodeFurthest = nodes.MaxBy(n => n.Distance);

                int partA = nodeFurthest.Distance / 2;
                int partB = nodes.Where(n => n.Distance >= 2000 && n.Value == '.').Count();

                //--

                int minX = grid.Keys.Min(k => k.Item1) - 1;
                int maxX = grid.Keys.Max(k => k.Item1) + 1;
                int width = maxX - minX;

                if (width > Console.WindowWidth) {
                    Console.WriteLine("Part 1: " + partA);
                    Console.WriteLine("Part 2: " + partB);
                    Console.WriteLine();
                    Console.WriteLine("Please resize then press any key to draw visual (or ESC to not).");

                    while (width > Console.WindowWidth) {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                            return;
                    }
                }

                DrawMap(grid);

                Console.WriteLine("Part 1: " + partA);
                //Answer: 3046
                Console.WriteLine("Part 2: " + partB);
                //Answer: 8545
                Console.WriteLine();
            }
        }

        private static void Build(Dictionary<(int, int), char> grid, string line, int x, int y) {
            line = line.Replace(':', '|');

            for (int i = 0; i < line.Length; i++) {
                char c = line[i];
                if (c == '^') continue;
                else if (c == '$') break;
                else if (c == 'N') {
                    y--;
                    grid[(x, y)] = '-';
                    y--;
                    grid[(x, y)] = '.';
                } else if (c == 'E') {
                    x++;
                    grid[(x, y)] = '|';
                    x++;
                    grid[(x, y)] = '.';
                } else if (c == 'S') {
                    y++;
                    grid[(x, y)] = '-';
                    y++;
                    grid[(x, y)] = '.';
                } else if (c == 'W') {
                    x--;
                    grid[(x, y)] = '|';
                    x--;
                    grid[(x, y)] = '.';
                } else if (c == '(') {
                    int depth = 1;
                    StringBuilder sb = new StringBuilder();
                    for (int k = i + 1; k < line.Length; k++) {
                        char kc = line[k];

                        if (kc == '(')
                            depth++;
                        else if (kc == ')')
                            depth--;

                        if (depth == 0) {
                            i = k;
                            break;
                        } else if (depth > 1 && kc == '|') {
                            kc = ':';
                        }

                        sb.Append(kc);
                    }

                    string[] fields = sb.ToString().Split('|');
                    foreach (string field in fields)
                        Build(grid, field, x, y);
                }
            }
        }

        private static void DrawMap(Dictionary<(int, int), char> grid) {
            int minX = grid.Keys.Min(k => k.Item1) - 1;
            int minY = grid.Keys.Min(k => k.Item2) - 1;
            int maxX = grid.Keys.Max(k => k.Item1) + 1;
            int maxY = grid.Keys.Max(k => k.Item2) + 1;

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    if (grid.ContainsKey((x, y))) {
                        char c = grid[(x, y)];
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write(c);
                        Console.ResetColor();
                    } else
                        Console.Write("#");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
