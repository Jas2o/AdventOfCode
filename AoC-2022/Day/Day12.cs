using AoC.Graph;

namespace AoC.Day
{
    public class Day12
    {
        //Based on 2021 Day 15

        public static void Run(string file) {
            Console.WriteLine("Day 12: Hill Climbing Algorithm" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            //Setup
            List<DNode> nodes = new List<DNode>();
            Dictionary<(int x, int y), ClimbArea> hill = new Dictionary<(int, int), ClimbArea>();
            ClimbArea start = null;
            ClimbArea end = null;

            for (int y = 0; y < lines.Length; y++) {
                for (int x = 0; x < lines[y].Length; x++) {
                    char c = lines[y][x];
                    int num = c;
                    if(c == 'S')
                        num = 'a';
                    else if (c == 'E')
                        num = 'z';
                    num -= 97;

                    DNode nodeA = new DNode(x, y, int.MaxValue, c);
                    nodes.Add(nodeA);
                    ClimbArea climbArea = new ClimbArea(x, y, num, nodeA);
                    hill.Add((x, y), climbArea);

                    if (c == 'S')
                        start = climbArea;
                    else if (c == 'E')
                        end = climbArea;
                }
            }

            int maxX = hill.Max(h => h.Value.X);
            int maxY = hill.Max(h => h.Value.Y);
            foreach (ClimbArea area in hill.Values)
                area.Neighbors = GetNeighbors(hill, area, maxX, maxY);

            //Solve
            DNode[] bestPathA = [];
            int partA = SolveWithQueue(nodes, hill, start, end, ref bestPathA, int.MaxValue);

            DNode[] bestPathB = [];
            int partB = partA;
            ClimbArea[] startsB = hill.Values.Where(v => v.Height == 0).ToArray();
            foreach(ClimbArea sB in startsB) {
                DNode[] path = [];
                int result = SolveWithQueue(nodes, hill, sB, end, ref path, partB);
                if (result < partB) {
                    partB = result;
                    bestPathB = path;
                    Console.WriteLine("{0},{1} resulted in {2}", sB.X, sB.Y, result);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 517
            Console.WriteLine("Part 2: " + partB);
            //Answer: 512
        }

        private static int SolveWithQueue(List<DNode> nodes, Dictionary<(int, int), ClimbArea> hill, ClimbArea start, ClimbArea end, ref DNode[] bestPath, int bestSteps = int.MaxValue) {
            HashSet<DNode> explored = new HashSet<DNode>();
            PriorityQueue<State, int> priorityQueue = new PriorityQueue<State, int>();
            priorityQueue.Enqueue(new State(start), 0);

            State prevState;
            int priority;
            while (priorityQueue.TryDequeue(out prevState, out priority)) {
                ClimbArea prevR = hill[(prevState.Current.X, prevState.Current.Y)];

                foreach (DNode option in prevR.Neighbors) {
                    if (prevState.Path.Contains(option))
                        continue;

                    ClimbArea optionArea = hill[(option.X, option.Y)];
                    State state = new State(prevState, optionArea);

                    if (state.Steps > bestSteps)
                        continue;
                    if (state.Current == end) {
                        if (state.Steps < bestSteps) {
                            bestSteps = state.Steps;
                            //Console.WriteLine("- Found: {0}", state.Steps);
                            bestPath = state.Path;
                        }
                        continue;
                    }

                    bool ex = explored.Add(option);
                    if (!ex)
                        continue;

                    priorityQueue.Enqueue(state, state.Steps);
                }
            }

            return bestSteps;
        }

        private static List<DNode> GetNeighbors(Dictionary<(int, int), ClimbArea> hill, ClimbArea current, int maxX, int maxY) {
            List<DNode> neighbors = new List<DNode>();
            if (current.Y > 0) {
                ClimbArea up = hill[(current.X, current.Y - 1)];
                if(current.Height >= up.Height || current.Height == up.Height - 1)
                    neighbors.Add(up.Node);
            }
            if (current.Y < maxY) {
                ClimbArea down = hill[(current.X, current.Y + 1)];
                if (current.Height >= down.Height || current.Height == down.Height - 1)
                    neighbors.Add(down.Node);
            }
            if (current.X > 0) {
                ClimbArea left = hill[(current.X - 1, current.Y)];
                if (current.Height >= left.Height || current.Height == left.Height - 1)
                    neighbors.Add(left.Node);
            }
            if (current.X < maxX) {
                ClimbArea right = hill[(current.X + 1, current.Y)];
                if (current.Height >= right.Height || current.Height == right.Height - 1)
                    neighbors.Add(right.Node);
            }
            return neighbors;
        }

        private class State {
            public DNode[] Path;
            public ClimbArea Current;
            public int Height;
            public int Steps;

            public State(ClimbArea start) {
                Path = [start.Node];
                Current = start;
                Height = 0;
                Steps = 0;
            }

            public State(State og, ClimbArea Next) {
                Current = Next; 
                Height = Next.Height;
                Steps = og.Steps + 1;

                Path = new DNode[og.Path.Length + 1];
                Array.Copy(og.Path, Path, og.Path.Length);
                Path[Path.Length - 1] = Current.Node;
            }
        }

        private class ClimbArea {
            public int X;
            public int Y;
            public int Height;
            public DNode Node;
            public List<DNode> Neighbors;

            public ClimbArea(int x, int y, int height, DNode node) {
                X = x;
                Y = y;
                Height = height;
                Node = node;
            }
        }
    }
}
