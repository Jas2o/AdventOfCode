using AoC.Graph;

namespace AoC.Day
{
    public class Day09
    {
        public static void Run(string file) {
            Console.WriteLine("Day 9: Smoke Basin" + Environment.NewLine);

			string input = File.ReadAllText(file);
			string[] lines = File.ReadAllLines(file);

            int height = lines.Length;
            int width = lines[0].Length;
            List<DNode> nodes = new List<DNode>();
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    char c = lines[y][x];
                    int num = (int)char.GetNumericValue(c);
                    DNode n = new DNode(x, y, num, c);
                    if (num == 9)
                        n.Ignore = true;
                    nodes.Add(n);
                }
            }

            int partA = 0;
            List<DNode> lowPoints = new List<DNode>();
            foreach (DNode node in nodes) {
                List<DNode> neighbors = DNode.GetNeighbors(nodes, node);
                int min = neighbors.Min(n => n.Distance);

                if (node.Distance < min) {
                    partA += node.Distance + 1;
                    node.Distance = 0; //Just for DrawMap
                    lowPoints.Add(node);
                }
            }
            DrawMap(nodes, height, width);

            List<int> sizes = new List<int>();
            foreach(DNode low in lowPoints) {
                DNode.ResetDistances(nodes);
                low.Distance = 0;
                DNode.Dijkstra(nodes);
                int size = 0;
                foreach (DNode node in nodes) {
                    if (node.Distance != int.MaxValue)
                        size++;
                }
                sizes.Add(size);
            }

            sizes.Sort();
            IEnumerable<int> last = sizes.TakeLast(3);

            int partB = 1;
            foreach (int i in last)
                partB *= i;

            Console.WriteLine("Part 1: " + partA);
            //Answer: 502
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1330560
        }

        private static void DrawMap(List<DNode> nodes, int height, int width) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    DNode node = nodes.Find(o => o.Y == y && o.X == x);
                    if (node.Value == '9')
                        Console.Write(' ');
                    else {
                        if (node.Distance == 0)
                            Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(node.Value);
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
