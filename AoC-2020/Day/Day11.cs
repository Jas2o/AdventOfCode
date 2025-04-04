using AoC.Graph;

namespace AoC.Day
{
    public class Day11
    {
        public static void Run(string file) {
            Console.WriteLine("Day 11: Seating System" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            int height = lines.Length;
            int width = lines[0].Length;
            bool verbose = (height < 15);
            List<DNode> nodes = new List<DNode>();
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    char c = lines[y][x];
                    if (c == '.')
                        continue;
                    DNode n1 = new DNode(x, y, 0, c);
                    nodes.Add(n1);
                }
            }

            //Part 1
            if (verbose) {
                Console.WriteLine("Initial:");
                DrawMap(nodes, height, width);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Part 1");
                Console.ResetColor();
            }

            Console.WriteLine("Preparing...");
            Dictionary<DNode, Seat> seats = new Dictionary<DNode, Seat>();
            foreach (DNode node in nodes) {
                List<DNode> neighbors = DNode.GetNeighborsWithDiagonals(nodes, node);
                Seat seat = new Seat(node, neighbors);
                seats.Add(node, seat);
            }

            Console.WriteLine("Solving...\r\n");
            int partA = Solve(nodes, seats, false, verbose, height, width);

            //Part 2
            if (verbose) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Part 2");
                Console.ResetColor();
            }

            Console.WriteLine("Preparing...");
            foreach (KeyValuePair<DNode, Seat> pair in seats) {
                pair.Key.Value = pair.Value.Start;
                pair.Value.Neighbors = GetAnyNeighborsWithDiagonals(nodes, pair.Key, height, width);
            }
            Console.WriteLine("Solving...\r\n");

            int partB = Solve(nodes, seats, true, verbose, height, width);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 2316
            Console.WriteLine("Part 2: " + partB);
            //Answer: 2128
        }

        private static int Solve(List<DNode> nodes, Dictionary<DNode, Seat> seats, bool isPartB, bool verbose=false, int height=0, int width=0) {
            int compare = (isPartB ? 5 : 4);

            List<DNode> becomeOccupied = new List<DNode>();
            List<DNode> becomeEmpty = new List<DNode>();

            int round = 0;
            while (true) {
                round++;

                foreach (DNode node in nodes) {
                    int nOccupied = seats[node].Neighbors.Where(n => n.Value == '#').Count();
                    if (node.Value == 'L') {
                        //Seat is empty
                        if (nOccupied == 0)
                            becomeOccupied.Add(node);
                    } else {
                        //Seat is occupied
                        if (nOccupied >= compare)
                            becomeEmpty.Add(node);
                    }
                }

                if (becomeOccupied.Any() || becomeEmpty.Any()) {
                    foreach (DNode node in becomeOccupied)
                        node.Value = '#';
                    foreach (DNode node in becomeEmpty)
                        node.Value = 'L';
                    becomeOccupied.Clear();
                    becomeEmpty.Clear();

                    if (verbose) {
                        Console.WriteLine("Round {0}:", round);
                        DrawMap(nodes, height, width);
                    }
                } else
                    break;
            }

            return nodes.Where(n => n.Value == '#').Count();
        }

        private static void DrawMap(List<DNode> nodes, int height, int width) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    DNode node = nodes.Find(o => o.Y == y && o.X == x);
                    if(node == null)
                        Console.Write('.');
                    else
                        Console.Write(node.Value);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static List<DNode> GetAnyNeighborsWithDiagonals(List<DNode> listNodes, DNode currentNode, int height, int width) {
            List<DNode> neighbors = new List<DNode>();

            IOrderedEnumerable<DNode> up = listNodes.FindAll(n => n.X == currentNode.X && n.Y < currentNode.Y).OrderBy(n => n.Y);
            IOrderedEnumerable<DNode> right = listNodes.FindAll(n => n.X > currentNode.X && n.Y == currentNode.Y).OrderBy(n => n.X);
            IOrderedEnumerable<DNode> down = listNodes.FindAll(n => n.X == currentNode.X && n.Y > currentNode.Y).OrderBy(n => n.Y);
            IOrderedEnumerable<DNode> left = listNodes.FindAll(n => n.X < currentNode.X && n.Y == currentNode.Y).OrderBy(n => n.X);

            if (up.Any()) neighbors.Add(up.Last());
            if (right.Any()) neighbors.Add(right.First());
            if (down.Any()) neighbors.Add(down.First());
            if (left.Any()) neighbors.Add(left.Last());

            DNode UR = null;
            DNode UL = null;
            DNode DR = null;
            DNode DL = null;

            for(int offset = 1; ; offset++) {
                int xL = currentNode.X - offset;
                int xR = currentNode.X + offset;
                int yU = currentNode.Y - offset;
                int yD = currentNode.Y + offset;

                if (UR != null && UL != null && DR != null && DL != null)
                    break;
                if (xL < 0 && xR >= width && yU < 0 && yD >= height)
                    break;

                if (UR == null) {
                    UR = listNodes.Find(n => n.X == xR && n.Y == yU);
                    if (UR != null)
                        neighbors.Add(UR);
                }

                if (UL == null) {
                    UL = listNodes.Find(n => n.X == xL && n.Y == yU);
                    if (UL != null)
                        neighbors.Add(UL);
                }

                if (DR == null) {
                    DR = listNodes.Find(n => n.X == xR && n.Y == yD);
                    if (DR != null)
                        neighbors.Add(DR);
                }

                if (DL == null) {
                    DL = listNodes.Find(n => n.X == xL && n.Y == yD);
                    if (DL != null)
                        neighbors.Add(DL);
                }
            }

            return neighbors;
        }

        private class Seat {
            public DNode Self;
            public List<DNode> Neighbors;
            public char Start;

            public Seat(DNode node, List<DNode> neighbors) {
                Self = node;
                Neighbors = neighbors;
                Start = node.Value;
            }
        }
    }
}
