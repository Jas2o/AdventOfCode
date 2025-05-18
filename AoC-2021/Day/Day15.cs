using AoC.Graph;

namespace AoC.Day
{
    public class Day15
    {
        //Based on 2018 Day 22

        public static void Run(string file) {
            Console.WriteLine("Day 15: Chiton" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            //Setup
            List<DNode> nodesA = new List<DNode>();
            List<DNode> nodesB = new List<DNode>();
            Dictionary<(int x, int y), CaveArea> caveA = new Dictionary<(int, int), CaveArea>();
            Dictionary<(int x, int y), CaveArea> caveB = new Dictionary<(int, int), CaveArea>();

            int dim = lines.Length;
            for (int y = 0; y < dim; y++) {
                for (int x = 0; x < dim; x++) {
                    char c = lines[y][x];
                    int num = (int)char.GetNumericValue(c);

                    DNode nodeA = new DNode(x, y, int.MaxValue, c);
                    nodesA.Add(nodeA);
                    caveA.Add((x, y), new CaveArea(x, y, num, nodeA));

                    for (int muly = 0; muly < 5; muly++) {
                        int by = dim * muly + y;
                        for (int mulx = 0; mulx < 5; mulx++) {
                            int bx = dim * mulx + x;
                            int bnum = num + (muly + mulx);
                            if (bnum > 9)
                                bnum -= 9;
                            char bc = (char)(bnum + 48); //zero
                            DNode nodeB = new DNode(bx, by, int.MaxValue, bc);
                            nodesB.Add(nodeB);
                            caveB.Add((bx, by), new CaveArea(bx, by, bnum, nodeB));
                        }
                    }
                }
            }

            //Part 1
            DNode endA = nodesA.Last();
            DNode[] bestPathA = [];
            int partA = SolveWithQueue(nodesA, caveA, endA, ref bestPathA);
            DrawMap(caveA, bestPathA);

            //Part 2
            DNode endB = nodesB.Last();
            DNode[] bestPathB = [];
            int partB = SolveWithQueue(nodesB, caveB, endB, ref bestPathB);
            if (dim * 5 < Console.WindowWidth)
                DrawMap(caveB, bestPathB);
            else
                Console.WriteLine("Unable to fit preview.\r\n");

            Console.WriteLine("Part 1: " + partA);
            //Answer: 562
            Console.WriteLine("Part 2: " + partB);
            //Answer: 2874
        }

        private static void DrawMap(Dictionary<(int, int), CaveArea> grid, DNode[] bestPath) {
            int minX = 0;
            int minY = 0;
            int maxX = grid.Keys.Max(k => k.Item1);
            int maxY = grid.Keys.Max(k => k.Item2);

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    CaveArea r = grid[(x, y)];
                    if(bestPath.Contains(r.Node))
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write(r.Risk);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static int SolveWithQueue(List<DNode> nodes, Dictionary<(int, int), CaveArea> cave, DNode end, ref DNode[] bestPath, int bestRisk = int.MaxValue) {
            Dictionary<(int x, int y), int> seen = new Dictionary<(int x, int y), int>();
            PriorityQueue<State, int> priorityQueue = new PriorityQueue<State, int>();
            priorityQueue.Enqueue(new State(cave[(0, 0)].Node), 0);

            State prevState;
            int priority;
            while (priorityQueue.TryDequeue(out prevState, out priority)) {
                CaveArea prevR = cave[(prevState.Current.X, prevState.Current.Y)];
                if (prevR.Neighbors == null)
                    prevR.Neighbors = GetNeighbors(cave, prevR.Node, end.X);

                foreach (DNode option in prevR.Neighbors) {
                    if (prevState.Path.Contains(option))
                        continue;

                    CaveArea optionArea = cave[(option.X, option.Y)];
                    State state = new State(prevState, optionArea);

                    if (state.Risk > bestRisk)
                        continue;
                    if (state.Current == end) {
                        if (state.Risk < bestRisk) {
                            bestRisk = state.Risk;
                            //Console.WriteLine("- Found: {0}", state.Risk);
                            bestPath = state.Path;
                        }
                        continue;
                    }

                    (int X, int Y) key = (option.X, option.Y);
                    bool t = seen.TryAdd(key, state.Risk);
                    if (!t) {
                        if (seen[key] > state.Risk)
                            seen[key] = state.Risk;
                        else
                            continue;
                    }

                    priorityQueue.Enqueue(state, state.Risk);
                }
            }

            return bestRisk;
        }

        private static List<DNode> GetNeighbors(Dictionary<(int, int), CaveArea> cave, DNode? currentNode, int maxXY) {
            List<DNode> neighbors = new List<DNode>();
            if (currentNode.Y > 0) {
                CaveArea up = cave[(currentNode.X, currentNode.Y - 1)];
                neighbors.Add(up.Node);
            }
            if (currentNode.Y < maxXY) {
                CaveArea down = cave[(currentNode.X, currentNode.Y + 1)];
                neighbors.Add(down.Node);
            }
            if (currentNode.X > 0) {
                CaveArea left = cave[(currentNode.X - 1, currentNode.Y)];
                neighbors.Add(left.Node);
            }
            if (currentNode.X < maxXY) {
                CaveArea right = cave[(currentNode.X + 1, currentNode.Y)];
                neighbors.Add(right.Node);
            }
            return neighbors;
        }

        private class State {
            public DNode[] Path;
            public DNode Current;
            public int Risk;

            public State(DNode start) {
                Path = [start];
                Current = start;
                Risk = 0;
            }

            public State(State og, CaveArea Next) {
                Risk = og.Risk + Next.Risk;
                Current = Next.Node;

                Path = new DNode[og.Path.Length + 1];
                Array.Copy(og.Path, Path, og.Path.Length);
                Path[Path.Length - 1] = Current;
            }
        }

        private class CaveArea {
            public int X;
            public int Y;
            public int Risk;
            public DNode Node;
            public List<DNode> Neighbors;

            public CaveArea(int x, int y, int risk, DNode node) {
                X = x;
                Y = y;
                Risk = risk;
                Node = node;
            }
        }
    }
}
