using AoC.Graph;

namespace AoC.Day
{
    public class Day22
    {
        //I feel like there's still something wrong with Part 2's solver.
        //Somehow got an answer the website accepted.

        public static void Run(string file) {
            Console.WriteLine("Day 22: Mode Maze" + Environment.NewLine);

            int depth = 0;
            int targetX = 0;
            int targetY = 0;

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                //It's only like this so that I can have a line that says "Test" as a reminder.
                string[] fields = line.Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                if (fields.Length != 2)
                    continue;
                if (fields[0] == "depth") {
                    depth = int.Parse(fields[1]);
                } else if (fields[0] == "target") {
                    string[] t = fields[1].Split(',');
                    targetX = int.Parse(t[0]);
                    targetY = int.Parse(t[1]);
                }
            }

            int areaX = targetX * 3; //For my input, 3 was the minimum, 4 was safe but taking a while.
            int areaY = targetY + 10;

            Dictionary<(int, int), Region> caveSystem = new Dictionary<(int, int), Region>();
            List<DNode> nodes = new List<DNode>();

            //Part 1, generate the cave system.
            int partA = 0;
            for (int x = 0; x <= areaX; x++) {
                for (int y = 0; y <= areaY; y++) {
                    int geoIdx = -1;
                    if (x == 0 && y == 0 || x == targetX && y == targetY)
                        geoIdx = 0;
                    else {
                        if (y == 0)
                            geoIdx = x * 16807;
                        else if (x == 0)
                            geoIdx = y * 48271;
                    }

                    Region r = new Region(x, y, depth, geoIdx);
                    if (r.Type == RegionType.UNKNOWN) {
                        Region r1 = caveSystem[(x - 1, y)];
                        Region r2 = caveSystem[(x, y - 1)];
                        r.UpdateErosionLevel(depth, r1.ErosionLevel * r2.ErosionLevel);
                    }
                    caveSystem.Add((x, y), r);

                    if (x <= targetX && y <= targetY) {
                        if (r.Type == RegionType.Wet)
                            partA += 1;
                        else if (r.Type == RegionType.Narrow)
                            partA += 2;
                    }

                    DNode n = new DNode(x, y, int.MaxValue, RegionTypeC[(int)r.Type]);
                    r.Node = n;
                    nodes.Add(n);
                }
            }

            //Part 2, find some really rough baseline values.
            List<DNode> pathXY = new List<DNode>();
            List<DNode> pathYX = new List<DNode>();
            int tx = 0;
            int ty = 0;
            for (tx = 0; tx <= targetX; tx++) {
                DNode n = nodes.Find(n => n.X == tx && n.Y == ty);
                pathXY.Add(n);
            }
            for (ty = 0; ty <= targetY; ty++) {
                DNode n = nodes.Find(n => n.X == tx && n.Y == ty);
                pathXY.Add(n);
            }
            //--
            tx = 0;
            for (ty = 0; ty <= targetY; ty++) {
                DNode n = nodes.Find(n => n.X == tx && n.Y == ty);
                pathYX.Add(n);
            }
            for (tx = 0; tx <= targetX; tx++) {
                DNode n = nodes.Find(n => n.X == tx && n.Y == ty);
                pathYX.Add(n);
            }
            int baselineTimeXY = BaselinePathToTime(pathXY);
            int baselineTimeYX = BaselinePathToTime(pathYX);
            int baseline = Math.Min(baselineTimeXY, baselineTimeYX);

            //Part 2, find paths that are better than the baseline.
            DNode nodeEnd = nodes.Find(n => n.X == targetX && n.Y == targetY);
            Console.WriteLine("Finding solutions... (X limit: {0} until {1})", targetX, areaX);
            int partB_time = baseline;
            DNode[] bestPath = [];
            for (int limitX = targetX; limitX <= areaX; limitX++) {
                int test = SolveWithQueue(nodes, caveSystem, nodeEnd, limitX, partB_time, ref bestPath);
                if (test < partB_time) {
                    Console.WriteLine("- X:{0}, found time: {1}", limitX, test);
                    partB_time = test;
                }
            }

            //Visualisation
            foreach(DNode n in bestPath)
                n.Distance = 0;
            Console.WriteLine();
            DrawMap(caveSystem, targetX, targetY);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 11462
            Console.WriteLine("Part 2: " + partB_time);
            //Answer: 1054
        }

        private static int BaselinePathToTime(IEnumerable<DNode> path) {
            int time = 0;
            int equip = 1; //0=neither, 1=torch, 2=climbing gear

            DNode current = path.First();
            foreach (DNode next in path) {
                if (current == next)
                    continue;
                int equipBefore = equip;
                if (current.Value != next.Value) {
                    if (current.Value == '.')
                        equip = (next.Value == '=' ? 2 : 1);
                    else if (current.Value == '=')
                        equip = (next.Value == '.' ? 2 : 0);
                    else if (current.Value == '|')
                        equip = (next.Value == '.' ? 1 : 0);
                }
                if (equip != equipBefore)
                    time += 7;
                time++;
                current = next;
            }

            if(equip != 1)
                time += 7;
            return time;
        }

        private static int SolveWithQueue(List<DNode> nodes, Dictionary<(int, int), Region> caveSys, DNode end, int limitX, int bestTime, ref DNode[] bestPath) {
            Dictionary<(int x, int y, int eq), int> seen = new Dictionary<(int x, int y, int eq), int>();
            PriorityQueue<State, int> priorityQueue = new PriorityQueue<State, int>();
            priorityQueue.Enqueue(new State(caveSys[(0, 0)].Node), 0);

            State prevState;
            int priority;
            while (priorityQueue.TryDequeue(out prevState, out priority)) {
                Region prevR = caveSys[(prevState.Current.X, prevState.Current.Y)];
                if (prevR.Neighbors == null)
                    prevR.Neighbors = DNode.GetNeighbors(nodes, prevR.Node);

                foreach (DNode option in prevR.Neighbors) {
                    if (prevState.Path.Contains(option) || option.X > limitX)
                        continue;

                    State state = new State(prevState, option);
                    //This will have added on the time and changed equip

                    if (state.Time > bestTime) //|| state.Switches > bestSwitches
                        continue;
                    if (state.Current == end) {
                        if (state.Equip != 1) {
                            //When in the last area, must switch to torch.
                            state.Time += 7;
                            state.Equip = 1;
                            state.Switches++;
                        }

                        if (state.Time < bestTime) {
                            bestTime = state.Time;
                            //Console.WriteLine("- Found: {0} / {1}", state.Time, state.Switches);
                            bestPath = state.Path;
                        }
                        continue;
                    }

                    (int X, int Y, int Equip) key = (option.X, option.Y, state.Equip);
                    bool t = seen.TryAdd(key, state.Time);
                    if (!t) {
                        if (seen[key] > state.Time)
                            seen[key] = state.Time;
                        else
                            continue;
                    }

                    priorityQueue.Enqueue(state, state.Time);
                }
            }

            return bestTime;
        }

        private static void DrawMap(Dictionary<(int, int), Region> grid, int targetX, int targetY) {
            int minX = 0;
            int minY = 0;
            int maxX = grid.Keys.Max(k => k.Item1);
            int maxY = grid.Keys.Max(k => k.Item2);

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    Region r = grid[(x, y)];

                    if (x == 0 && y == 0)
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                    else if (x == targetX && y == targetY)
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    else if(r.Node.Distance == 0)
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    else if (r.Neighbors == null)
                        Console.BackgroundColor = ConsoleColor.DarkGray;

                    if (r.Type == RegionType.Rocky)
                        Console.Write('.');
                    else if (r.Type == RegionType.Wet)
                        Console.Write('=');
                    else if (r.Type == RegionType.Narrow)
                        Console.Write('|');
                    else {
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.Write('?');
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private class State {
            public DNode[] Path;
            public DNode Current;
            public int Time;
            public int Equip;
            public int Switches;

            public State(DNode start) {
                Time = 0;
                Equip = 1;
                Switches = 0;

                Current = start;
                Path = [start];
            }

            public State(State og, DNode Next) {
                Time = og.Time + 1;
                Equip = og.Equip;
                Switches = og.Switches;

                //0=neither, 1=torch, 2=climbing gear
                if (og.Current.Value != Next.Value) {
                    if (og.Current.Value == '.')
                        Equip = (Next.Value == '=' ? 2 : 1);
                    else if (og.Current.Value == '=')
                        Equip = (Next.Value == '.' ? 2 : 0);
                    else if (og.Current.Value == '|')
                        Equip = (Next.Value == '.' ? 1 : 0);
                }
                if (Equip != og.Equip) {
                    Time += 7;
                    Switches++;
                }
                Current = Next;

                Path = new DNode[og.Path.Length + 1];
                Array.Copy(og.Path, Path, og.Path.Length);
                Path[Path.Length - 1] = Next;
            }
        }

        private static char[] RegionTypeC = { '.', '=', '|', '?' };
        private enum RegionType {
            Rocky = 0,
            Wet = 1,
            Narrow = 2,
            UNKNOWN = 3,
        }

        private class Region {
            public int X;
            public int Y;
            public int GeologicIndex { get; private set; }
            public int ErosionLevel { get; private set; }
            public RegionType Type;
            public DNode Node;
            public List<DNode> Neighbors;

            public Region(int x, int y, int depth, int geoIdx) {
                X = x;
                Y = y;
                GeologicIndex = geoIdx;
                UpdateErosionLevel(depth);
            }

            public void UpdateErosionLevel(int depth, int geoIdx=-1) {
                if(geoIdx != -1)
                    GeologicIndex = geoIdx;
                if (GeologicIndex == -1) {
                    Type = RegionType.UNKNOWN;
                    return;
                }

                ErosionLevel = (GeologicIndex + depth) % 20183;
                Type = (RegionType)(ErosionLevel % 3);
            }
        }
    }
}
