namespace AoC.Day
{
    public class Day22
    {
        public static void Run(string file) {
            Console.WriteLine("Day 22: Grid Computing" + Environment.NewLine);

            //Setup
            List<DNode> nodes = new List<DNode>();
            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                if (!line.StartsWith("/dev/grid/"))
                    continue;

                string[] fields = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string[] devgrid = fields[0].Split('-');
                int x = int.Parse(devgrid[1].Substring(1));
                int y = int.Parse(devgrid[2].Substring(1));
                DNode node = new DNode(x, y, int.MaxValue);
                nodes.Add(node);
                //So uncivilized.
                node.Size = int.Parse(fields[1].Split('T').First());
                node.Used = int.Parse(fields[2].Split('T').First());
                node.Avail = int.Parse(fields[3].Split('T').First());
                node.UsePercent = int.Parse(fields[4].Split('%').First());
            }

            if(nodes.Count == 0) {
                Console.WriteLine("There's no nodes...");
                return;
            }

            int maxX = nodes.Max(n => n.X);
            int maxY = nodes.Max(n => n.Y);
            List<DNode> empties = nodes.FindAll(n => n.Used == 0);
            if (empties.Count != 1)
                throw new ArgumentException();
            DNode nodeEmpty = empties.First();
            DNode nodeZero = nodes.Find(n => n.Y == 0 && n.X == 0);
            DNode nodeGoal = nodes.Find(n => n.Y == 0 && n.X == maxX);
            nodeZero.Value = '@';
            nodeGoal.Value = 'G';

            //Part 1
            int pairs = 0;
            for(int i = 0; i < nodes.Count; i++) {
                if (nodes[i].Used == 0) {
                    nodes[i].Value = '_';
                    continue;
                }
                for (int k = 1; k < nodes.Count - 1; k++) {
                    if (i == k)
                        continue;
                    if (nodes[i].Used <= nodes[k].Avail) {
                        pairs++;
                        nodes[i].Viable = nodes[k].Viable = true;
                    }
                }
            }
            foreach (DNode node in nodes.Where(n => !n.Viable))
                node.Value = '#';

            //Part 2, empty to goal data.
            nodeEmpty.Distance = 0;
            Dijkstra(nodes);
            DNode nodeC1 = nodeGoal;
            List<DNode> listPathEmptyToGoal = new List<DNode>();
            while (nodeC1 != null) {
                if (nodeC1.Value == '.')
                    nodeC1.Value2 = '*';
                listPathEmptyToGoal.Add(nodeC1);
                nodeC1 = nodeC1.Previous;
            }
            //Move the empty spot to before the goal.
            DNode nodeBeforeGoal = listPathEmptyToGoal[1];
            (nodeEmpty.X, nodeBeforeGoal.X) = (nodeBeforeGoal.X, nodeEmpty.X);
            (nodeEmpty.Y, nodeBeforeGoal.Y) = (nodeBeforeGoal.Y, nodeEmpty.Y);
            //Move the goal into the empty spot (which puts empty behind).
            (nodeEmpty.X, nodeGoal.X) = (nodeGoal.X, nodeEmpty.X);
            (nodeEmpty.Y, nodeGoal.Y) = (nodeGoal.Y, nodeEmpty.Y);
            int steps = listPathEmptyToGoal.Count() - 1;
            Console.WriteLine("Empty moves {0} times.", steps);

            //Part 2, goal to us.
            ResetDistances(nodes);
            nodeGoal.Distance = 0;
            Dijkstra(nodes);
            DNode nodeC2 = nodeZero;
            List<DNode> listGoalToZero = new List<DNode>();
            while (nodeC2 != null) {
                listGoalToZero.Add(nodeC2);
                nodeC2 = nodeC2.Previous;
            }
            int distToZero = listGoalToZero.Count() - 1;
            int moreSteps = 5 * (distToZero);
            steps += moreSteps;
            Console.WriteLine("Goal needs to travel {0} times (which is {1} steps).", listGoalToZero.Count() - 1, moreSteps);

            //Draw the path of the empty to goal.
            DrawMap(nodes, maxX, maxY);

            Console.WriteLine("Part 1: " + pairs);
            //Answer: 976
            Console.WriteLine("Part 2: " + steps);
            //Answer: 209
        }

        private class DNode {
            public int X;
            public int Y;
            public int Distance;
            public DNode? Previous;
            public char Value;
            public char Value2;

            //Most of these are not used properly.
            public int Size;
            public int Used;
            public int Avail;
            public int UsePercent;
            public bool Viable;

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
                    if (listVisited.Contains(nextNode) || !nextNode.Viable)
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
            for (int y = 0; y <= maxY; y++) {
                Console.Write(y.ToString().PadRight(3));

                for (int x = 0; x <= maxX; x++) {
                    DNode node = list.Find(o => o.Y == y && o.X == x);
                    if (node == null) {
                        Console.Write("NULL ");
                    } else {
                        if(node.Value == '.' && node.Value2 != 0)
                            Console.Write(node.Value2);
                        else
                            Console.Write(node.Value);
                        //string s = string.Format("{0,3}:{1,3} | ", node.Used, node.Size);
                        //Console.Write(s.PadLeft(8));
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
