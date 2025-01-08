using AoC.Graph;
using System.Drawing;

namespace AoC.Day
{
    public class Day03
    {
        public static void Run(string file) {
            Console.WriteLine("Day 3: Spiral Memory" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                int input = int.Parse(line);
                Point p = GetSpiralNumCoord(input);
                int manhattan = Math.Abs(p.X) + Math.Abs(p.Y);
                List<DNode> part2 = StressTest(input);
                int larger = part2.Last().Distance;

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("{0} at {1},{2}", input, p.X, p.Y);
                Console.WriteLine(string.Join(',', part2.Select(n => n.Distance)));
                Console.ResetColor();

                Console.WriteLine();
                Console.WriteLine("Part 1: " + manhattan);
                //Answer: 430
                Console.WriteLine("Part 2: " + larger);
                //Answer: 312453

                Console.WriteLine();
            }
        }

        private static Point GetSpiralNumCoord(int n) {
            int k = (int)Math.Ceiling((Math.Sqrt(n) - 1) / 2);
            int t = (2 * k + 1);
            int m = (int)Math.Pow(t, 2);
            t = t - 1;
            if (n >= m - t) {
                return new Point(k - (m - n), -k);
            } else {
                m = m - t;
            }
            if (n >= m - t) {
                return new Point(-k, -k + (m - n));
            } else {
                m = m - t;
            }
            if (n >= m - t) {
                return new Point(-k + (m - n), k);
            } else {
                return new Point(k, k - (m - n - t));
            }
        }

        private static List<DNode> StressTest(int input) {
            List<DNode> nodes = new List<DNode>() { new DNode(0, 0, 1) };
            int x = 1;
            int y = 0;
            int d = 1;
            int m = 1;
            while (true) {
                while (2 * x * d < m) {
                    if(StressTestAdd(nodes, x, y, input))
                        return nodes;
                    x = x + d;
                }
                while (2 * y * d < m) {
                    if (StressTestAdd(nodes, x, y, input))
                        return nodes;
                    y = y + d;
                }
                d = -1 * d;
                m = m + 1;
            }
        }

        private static bool StressTestAdd(List<DNode> nodes, int x, int y, int input) {
            DNode node = new DNode(x, y, 0);
            List<DNode> neighbors = DNode.GetNeighborsWithDiagonals(nodes, node);
            node.Distance = neighbors.Sum(n => n.Distance);
            nodes.Add(node);
            return node.Distance > input;
        }
    }
}
