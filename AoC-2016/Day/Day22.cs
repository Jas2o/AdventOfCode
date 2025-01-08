using AoC.Graph;

namespace AoC.Day
{
    public class Day22
    {
        public static void Run(string file) {
            Console.WriteLine("Day 22: Grid Computing" + Environment.NewLine);

            //Setup
            List<DNode> nodes = new List<DNode>();
            List<SNode> storageNodes = new List<SNode>();
            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                if (!line.StartsWith("/dev/grid/"))
                    continue;

                string[] fields = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string[] devgrid = fields[0].Split('-');
                int x = int.Parse(devgrid[1].Substring(1));
                int y = int.Parse(devgrid[2].Substring(1));

                DNode node = new DNode(x, y, int.MaxValue);
                storageNodes.Add(new SNode(node, fields));
                nodes.Add(node);
            }

            if(nodes.Count == 0) {
                Console.WriteLine("There's no nodes...");
                return;
            }

            int maxX = nodes.Max(n => n.X);
            int maxY = nodes.Max(n => n.Y);
            List<SNode> empties = storageNodes.FindAll(n => n.Used == 0);
            if (empties.Count != 1)
                throw new ArgumentException("Only expected one empty storage node.");
            DNode nodeEmpty = empties.First().dNode;
            DNode nodeZero = nodes.Find(n => n.Y == 0 && n.X == 0);
            DNode nodeGoal = nodes.Find(n => n.Y == 0 && n.X == maxX);
            nodeZero.Value = '@';
            nodeGoal.Value = 'G';

            //Part 1
            int pairs = 0;
            for(int i = 0; i < storageNodes.Count; i++) {
                if (storageNodes[i].Used == 0) {
                    nodes[i].Value = '_';
                    continue;
                }
                for (int k = 1; k < nodes.Count - 1; k++) {
                    if (i == k)
                        continue;
                    if (storageNodes[i].Used <= storageNodes[k].Avail) {
                        pairs++;
                        storageNodes[i].Viable = storageNodes[k].Viable = true;
                    }
                }
            }
            foreach (SNode snode in storageNodes.Where(n => !n.Viable)) {
                snode.dNode.Ignore = true;
                snode.dNode.Value = '#';
            }

            //Part 2, empty to goal data.
            nodeEmpty.Distance = 0;
            DNode.Dijkstra(nodes);
            List<DNode> listPathEmptyToGoal = DNode.GetPath(nodeGoal, '.', '*');
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
            DNode.ResetDistances(nodes);
            nodeGoal.Distance = 0;
            DNode.Dijkstra(nodes);
            List<DNode> listGoalToZero = DNode.GetPath(nodeZero);
            int distToZero = listGoalToZero.Count() - 1;
            int moreSteps = 5 * (distToZero);
            steps += moreSteps;
            Console.WriteLine("Goal needs to travel {0} times (which is {1} steps).", listGoalToZero.Count() - 1, moreSteps);

            //Draw the path of the empty to goal.
            DrawMap(nodes, maxX, maxY);
            //DrawStorageMap(storageNodes, maxX, maxY);

            Console.WriteLine("Part 1: " + pairs);
            //Answer: 976
            Console.WriteLine("Part 2: " + steps);
            //Answer: 209
        }

        private class SNode {
            public DNode dNode;
            public int Size;
            public int Used;
            public int Avail;
            public int UsePercent;
            public bool Viable;

            public SNode(DNode node, string[] fields) {
                dNode = node;
                //So uncivilized.
                Size = int.Parse(fields[1].Split('T').First());
                Used = int.Parse(fields[2].Split('T').First());
                Avail = int.Parse(fields[3].Split('T').First());
                UsePercent = int.Parse(fields[4].Split('%').First());
            }
        }

        private static void DrawMap(List<DNode> list, int maxX, int maxY) {
            for (int y = 0; y <= maxY; y++) {
                Console.Write(y.ToString().PadRight(3));

                for (int x = 0; x <= maxX; x++) {
                    DNode node = list.Find(o => o.Y == y && o.X == x);
                    if (node == null)
                        Console.Write("NULL ");
                    else
                        Console.Write(node.Value);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void DrawStorageMap(List<SNode> list, int maxX, int maxY) {
            for (int y = 0; y <= maxY; y++) {
                Console.Write(y.ToString().PadRight(3));

                for (int x = 0; x <= maxX; x++) {
                    SNode snode = list.Find(o => o.dNode.Y == y && o.dNode.X == x);
                    if (snode == null) {
                        Console.Write("NULL ");
                    } else {
                        string s = string.Format("{0,3}:{1,3} | ", snode.Used, snode.Size);
                        Console.Write(s.PadLeft(8));
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
