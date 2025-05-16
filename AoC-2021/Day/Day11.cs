using AoC.Graph;

namespace AoC.Day
{
    public class Day11
    {
        public static void Run(string file) {
            Console.WriteLine("Day 11: Dumbo Octopus" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            int dim = lines.Length;
            List<DNode> nodes = new List<DNode>();
            for (int y = 0; y < dim; y++) {
                for (int x = 0; x < dim; x++) {
                    char c = lines[y][x];
                    int num = (int)char.GetNumericValue(c);
                    DNode n = new DNode(x, y, num, '#');
                    nodes.Add(n);
                }
            }

            Console.WriteLine("Before any steps:");
            DrawMap(nodes, dim);

            int partA = 0;
            int partB = 0;
            List<DNode> listFlash1 = new List<DNode>();
            List<DNode> listFlash2 = new List<DNode>();

            for (int step = 1; step <= 9999; step++) {
                listFlash1.Clear();
                foreach (DNode node in nodes) {
                    node.Distance++;
                    if (node.Distance > 9)
                        listFlash1.Add(node);
                }

                while (listFlash1.Any()) {
                    listFlash2.Clear();
                    foreach (DNode node in listFlash1) {
                        if (node.Ignore)
                            continue;
                        node.Ignore = true;
                        List<DNode> neighbors = DNode.GetNeighborsWithDiagonals(nodes, node);
                        foreach (DNode n in neighbors) {
                            n.Distance++;
                            if (n.Distance > 9)
                                listFlash2.Add(n);
                        }
                    }
                    (listFlash1, listFlash2) = (listFlash2, listFlash1);
                }

                Console.WriteLine("After step {0}:", step);
                DrawMap(nodes, dim);

                int flashes = 0;
                foreach (DNode node in nodes) {
                    if (node.Ignore) {
                        node.Ignore = false;
                        node.Distance = 0;
                        flashes++;
                    }
                }

                if (step <= 100)
                    partA += flashes;
                if(flashes == 100) {
                    partB = step;
                    break;
                }
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 1659
            Console.WriteLine("Part 2: " + partB);
            //Answer: 227
        }

        private static void DrawMap(List<DNode> nodes, int dim = 10) {
            for (int y = 0; y < dim; y++) {
                for (int x = 0; x < dim; x++) {
                    DNode node = nodes.Find(o => o.Y == y && o.X == x);
                    if (node.Distance > 9) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(0);
                        Console.ResetColor();
                    } else {
                        Console.Write(node.Distance);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
