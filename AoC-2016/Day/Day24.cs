using AoC.Graph;
using AoC.Shared;

namespace AoC.Day
{
    public class Day24
    {
        public static void Run(string file) {
            Console.WriteLine("Day 24: Air Duct Spelunking" + Environment.NewLine);

            //Setup
            List<DNode> nodesAll = new List<DNode>();
            List<DNode> nodes = new List<DNode>();
            List<DNode> nodesNum = new List<DNode>();

            string[] lines = File.ReadAllLines(file);
            for (int y = 0; y < lines.Length; y++) {
                for (int x = 0; x < lines[y].Length; x++) {
                    char c = lines[y][x];
                    DNode node = new DNode(x, y, int.MaxValue, c);
                    nodesAll.Add(node);
                    if (c != '#') {
                        nodes.Add(node);
                        if (c != '.')
                            nodesNum.Add(node);
                    }
                }
            }
            nodesNum = nodesNum.OrderBy(n => n.Value).ToList();
            int maxX = lines[0].Length;
            int maxY = lines.Length;

            DrawMap(nodesAll, maxX, maxY);

            Console.WriteLine("Calculating...\r\n");

            //Find all the paths and the distances.
            Dictionary<(char, char), int> paths = new Dictionary<(char, char), int>();
            for(int n1 = 0; n1 < nodesNum.Count(); n1++) {
                for (int n2 = 0; n2 < nodesNum.Count(); n2++) {
                    if (n1 == n2)
                        continue;
                    DNode node1 = nodesNum[n1];
                    DNode node2 = nodesNum[n2];

                    DNode.ResetDistances(nodes);
                    node1.Distance = 0;
                    DNode.Dijkstra(nodes);

                    List<DNode> listPathEmptyToGoal = DNode.GetPath(node2, '.', '*');
                    paths.Add((node1.Value, node2.Value), listPathEmptyToGoal.Count - 1);
                }
            }

            DrawMap(nodesAll, maxX, maxY);

            //Find which are the best options for starting at 0 and visiting all others.
            //For Part 2 there will also be a return trip to 0.
            char[] zero = new char[] { '0' };
            char[] bestPath_A = new char[0];
            char[] bestPath_B = new char[0];
            int bestTotal_A = int.MaxValue;
            int bestTotal_B = int.MaxValue;
            char[] otherNums = nodesNum.Select(n => n.Value).Skip(1).ToArray();
            List<IList<char>> perms = Permutations.Get(otherNums);
            foreach (IList<char> perm in perms) {
                char previous = '0';
                int total_A = 0;
                int total_B = 0;
                foreach (char c in perm) {
                    total_A += paths[(previous, c)];
                    previous = c;
                }
                total_B = total_A + paths[(previous, '0')];
                if (total_A < bestTotal_A) {
                    bestTotal_A = total_A;
                    bestPath_A = zero.Concat(perm).ToArray();
                }
                if (total_B < bestTotal_B) {
                    bestTotal_B = total_B;
                    bestPath_B = zero.Concat(perm).Concat(zero).ToArray();
                }
            }

            Console.WriteLine("Part 1: {0} going {1}", bestTotal_A, string.Join(',', bestPath_A));
            //Answer: 464
            Console.WriteLine("Part 2: {0} going {1}", bestTotal_B, string.Join(',', bestPath_B));
            //Answer: 652
        }

        private static void DrawMap(List<DNode> list, int maxX, int maxY) {
            for (int y = 0; y < maxY; y++) {
                for (int x = 0; x < maxX; x++) {
                    DNode node = list.Find(o => o.Y == y && o.X == x);
                    if (node == null) {
                        Console.Write("?");
                    } else {
                        if (node.Value == '#')
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        else if (node.Value == '*')
                            Console.ForegroundColor = ConsoleColor.Green;
                        else if (node.Value != '.')
                            Console.ForegroundColor = ConsoleColor.Cyan;

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
