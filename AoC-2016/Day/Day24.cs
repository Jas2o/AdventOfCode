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

                    ResetDistances(nodes);
                    node1.Distance = 0;
                    Dijkstra(nodes);

                    DNode nodeCurrent = node2;
                    List<DNode> listPathEmptyToGoal = new List<DNode>();
                    while (nodeCurrent != null) {
                        if (nodeCurrent.Value == '.')
                            nodeCurrent.Value = '*';
                        listPathEmptyToGoal.Add(nodeCurrent);
                        nodeCurrent = nodeCurrent.Previous;
                    }

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
            List<IList<char>> perms = GetPermutations(otherNums);
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

        private class DNode {
            public int X;
            public int Y;
            public int Distance;
            public DNode? Previous;
            public char Value;

            public DNode(int x, int y, int distance, char value = '.') {
                X = x;
                Y = y;
                Distance = distance;
                Value = value;
            }

            public override string ToString() {
                return string.Format("{0},{1} {2}", X, Y, Value);
            }
        }

        private static void Dijkstra(List<DNode> listNodes) {
            List<DNode> listVisited = new List<DNode>();
            List<DNode> listUnvisited = new List<DNode>();
            listUnvisited.AddRange(listNodes);

            bool loop = true;
            while (loop) {
                if (listUnvisited.Count == 0) {
                    loop = false;
                    break;
                }
                DNode currentNode = listUnvisited.MinBy(n => n.Distance);
                if (currentNode.Distance == int.MaxValue)
                    break;
                List<DNode> neighbors = GetNeighbors(listUnvisited, currentNode);
                foreach (DNode nextNode in neighbors) {
                    if (listVisited.Contains(nextNode))
                        continue;
                    int distance = currentNode.Distance + 1;
                    if (distance < nextNode.Distance) {
                        nextNode.Distance = distance;
                        nextNode.Previous = currentNode;
                    }
                }
                listVisited.Add(currentNode);
                listUnvisited.Remove(currentNode);
            }
        }

        private static List<DNode> GetNeighbors(List<DNode> listNodes, DNode? currentNode) {
            List<DNode> neighbors = new List<DNode>();

            DNode up = listNodes.Find(n => n.X == currentNode.X && n.Y + 1 == currentNode.Y);
            DNode down = listNodes.Find(n => n.X == currentNode.X && n.Y - 1 == currentNode.Y);
            DNode left = listNodes.Find(n => n.X + 1 == currentNode.X && n.Y == currentNode.Y);
            DNode right = listNodes.Find(n => n.X - 1 == currentNode.X && n.Y == currentNode.Y);

            if (up != null) neighbors.Add(up);
            if (down != null) neighbors.Add(down);
            if (left != null) neighbors.Add(left);
            if (right != null) neighbors.Add(right);

            return neighbors;
        }

        private static void ResetDistances(List<DNode> nodes) {
            foreach (DNode node in nodes) {
                node.Distance = int.MaxValue;
                node.Previous = null;
            }
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

        //https://chadgolden.com/blog/finding-all-the-permutations-of-an-array-in-c-sharp
        private static List<IList<T>> GetPermutations<T>(T[] input) {
            var list = new List<IList<T>>();
            return PermutationsRecursive(input, 0, input.Length - 1, list);
        }

        private static List<IList<T>> PermutationsRecursive<T>(T[] input, int start, int end, List<IList<T>> list) {
            if (start == end) {
                // We have one of our possible n! solutions,
                // add it to the list.
                list.Add(new List<T>(input));
            } else {
                for (var i = start; i <= end; i++) {
                    Swap(ref input[start], ref input[i]);
                    PermutationsRecursive(input, start + 1, end, list);
                    Swap(ref input[start], ref input[i]);
                }
            }

            return list;
        }

        private static void Swap<T>(ref T a, ref T b) {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}
