using AoC.Graph;

namespace AoC.Day
{
    public class Day18
    {
        //This won't find all possible best paths (I couldn't get one of the samples), but it should find the anwser.
        private const bool SHOW_ALL_BEST_FOUND = false;

        public static void Run(string file) {
            Console.WriteLine("Day 18: Many-Worlds Interpretation" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            List<DNode> nodes = new List<DNode>();
            for (int y = 0; y < lines.Length; y++) {
                for (int x = 0; x < lines[y].Length; x++) {
                    char c = lines[y][x];
                    if (c == '#')
                        continue;

                    DNode node = new DNode(x, y, int.MaxValue, c);
                    nodes.Add(node);
                }
            }

            //Part 1
            DNode start = nodes.Find(n => n.Value == '@');

            List<DNode> keys = new List<DNode>();
            foreach (DNode node in nodes) {
                if (node.Value == '.' || node.Value == '@')
                    continue;
                else if (node.Value > 96) {
                    //Lowercase
                    keys.Add(node);
                }
            }

            Queue<MazeStateV1> queueV1 = new Queue<MazeStateV1>();
            Dictionary<(DNode, DNode), List<DNode>> keyPaths = new Dictionary<(DNode, DNode), List<DNode>>();

            Console.WriteLine("Finding paths...");

            int goal = 0;
            foreach(DNode key in keys) {
                int keyNum = key.Value - 97;
                goal = goal | (1 << keyNum);

                DNode.ResetDistances(nodes);
                key.Distance = 0;
                DNode.Dijkstra(nodes);

                List<DNode> path = DNode.GetPath(start);
                keyPaths.Add((start, key), path);
                queueV1.Enqueue(new MazeStateV1(path));

                foreach (DNode other in keys) {
                    if (key == other)
                        continue;

                    path = DNode.GetPath(other, true);
                    keyPaths.Add((key, other), path);
                }
            }

            Console.WriteLine("Solving...");

            Dictionary<(int, char, char), int> memoryA = new Dictionary<(int, char, char), int>(); //(KeysMask, lastKey, nextKey), stepCount
            Dictionary<string, int> solutions = new Dictionary<string, int>();

            while (queueV1.Any()) {
                MazeStateV1 state = queueV1.Dequeue();

                bool solved = false;
                bool success = true;
                foreach (DNode node in state.Path) {
                    if (node.Value == '.' || node.Value == '@') {
                    } else if (node.Value < 97) {
                        //Uppercase door
                        char reqKey = (char)(node.Value + 32);
                        int reqKeyNum = reqKey - 97;

                        if (!IsBitSet(state.Keys, reqKeyNum)) {
                            success = false;
                            break;
                        }
                    } else {
                        int keyNum = node.Value - 97;
                        if (!IsBitSet(state.Keys, keyNum)) {
                            state.Order = state.Order + node.Value;
                            state.Keys = state.Keys | (1 << keyNum);
                            
                            if (state.Keys == goal) {
                                solved = true;
                                break;
                            }
                        }
                    }
                }

                if (success) {
                    state.Steps += state.Path.Count() - 1;

                    if (solved) {
                        bool solAdded = solutions.TryAdd(state.Order, state.Steps);
                        if (!solAdded && state.Steps < solutions[state.Order])
                            solutions[state.Order] = state.Steps;
                    }

                    DNode nextStart = state.Path.Last();
                    IEnumerable<KeyValuePair<(DNode, DNode), List<DNode>>> options = keyPaths.Where(o => o.Key.Item1 == nextStart);
                    foreach(KeyValuePair<(DNode, DNode), List<DNode>> option in options) {
                        (int Keys, char Last, char Next) k = (state.Keys, state.Path.Last().Value, option.Key.Item2.Value);
                        bool added = memoryA.TryAdd(k, state.Steps);
                        if (!added) {
                            if (state.Steps < memoryA[k])
                                memoryA[k] = state.Steps;
                            else
                                continue;
                        }

                        if (!state.Order.Contains(option.Key.Item2.Value)) {
                            MazeStateV1 next = new MazeStateV1(state, option.Value);
                            queueV1.Enqueue(next);
                        }
                    }
                }
            }

            int partA = 0;
            if (solutions.Any()) {
                partA = solutions.Min(s => s.Value);
                if (SHOW_ALL_BEST_FOUND) {
                    foreach (KeyValuePair<string, int> solution in solutions.Where(s => s.Value == partA))
                        Console.WriteLine("{0} = {1}", solution.Key, solution.Value);
                } else {
                    IEnumerable<KeyValuePair<string, int>> filtered = solutions.Where(s => s.Value == partA);
                    Console.WriteLine("{0}, one of them being: {1} = {2}", filtered.Count(), filtered.First().Key, filtered.First().Value);
                }
                
            } else {
                Console.WriteLine("Found no solution.");
            }

            //Part 2
            keyPaths.Clear();
            memoryA.Clear();
            solutions.Clear();
            Queue<MazeStateV2> queueV2 = new Queue<MazeStateV2>();
            Dictionary<(int, string, char), int> memoryB = new Dictionary<(int, string, char), int>(); //(KeysMask, robots, nextKey), stepCount

            Console.WriteLine("\r\nFinding paths again...");

            DNode r1 = nodes.Find(n => n.X == start.X - 1 && n.Y == start.Y - 1);
            DNode r2 = nodes.Find(n => n.X == start.X + 1 && n.Y == start.Y - 1);
            DNode r3 = nodes.Find(n => n.X == start.X - 1 && n.Y == start.Y + 1);
            DNode r4 = nodes.Find(n => n.X == start.X + 1 && n.Y == start.Y + 1);
            r1.Value = r2.Value = r3.Value = r4.Value = '@';

            DNode wallN = nodes.Find(n => n.X == start.X && n.Y == start.Y - 1);
            DNode wallW = nodes.Find(n => n.X == start.X - 1 && n.Y == start.Y);
            DNode wallE = nodes.Find(n => n.X == start.X + 1 && n.Y == start.Y);
            DNode wallS = nodes.Find(n => n.X == start.X && n.Y == start.Y + 1);
            nodes.Remove(wallN);
            nodes.Remove(wallW);
            nodes.Remove(wallE);
            nodes.Remove(wallS);
            nodes.Remove(start);

            //DrawMap(nodes);

            foreach (DNode key in keys) {
                int keyNum = key.Value - 97;

                DNode.ResetDistances(nodes);
                key.Distance = 0;
                DNode.Dijkstra(nodes);

                List<DNode> path1 = DNode.GetPath(r1);
                if (path1.Count > 1) {
                    keyPaths.Add((r1, key), path1);
                    queueV2.Enqueue(new MazeStateV2(path1));
                }
                List<DNode> path2 = DNode.GetPath(r2);
                if (path2.Count > 1) {
                    keyPaths.Add((r2, key), path2);
                    queueV2.Enqueue(new MazeStateV2(path2));
                }
                List<DNode> path3 = DNode.GetPath(r3);
                if (path3.Count > 1) {
                    keyPaths.Add((r3, key), path3);
                    queueV2.Enqueue(new MazeStateV2(path3));
                }
                List<DNode> path4 = DNode.GetPath(r4);
                if (path4.Count > 1) {
                    keyPaths.Add((r4, key), path4);
                    queueV2.Enqueue(new MazeStateV2(path4));
                }

                foreach (DNode other in keys) {
                    if (key == other)
                        continue;

                    List<DNode> path = DNode.GetPath(other, true);
                    if (path.Count > 1)
                        keyPaths.Add((key, other), path);
                }
            }

            Console.WriteLine("Solving...");

            int partB = int.MaxValue;
            while (queueV2.Any()) {
                MazeStateV2 state = queueV2.Dequeue();

                bool solved = false;
                bool success = true;
                foreach (DNode node in state.Path) {
                    if (node.Value == '.' || node.Value == '@') {
                    } else if (node.Value < 97) {
                        //Uppercase door
                        char reqKey = (char)(node.Value + 32);
                        int reqKeyNum = reqKey - 97;

                        if (!IsBitSet(state.Keys, reqKeyNum)) {
                            success = false;
                            break;
                        }
                    } else {
                        int keyNum = node.Value - 97;
                        if (!IsBitSet(state.Keys, keyNum)) {
                            state.Order = state.Order + node.Value;
                            state.Keys = state.Keys | (1 << keyNum);

                            if (state.Keys == goal) {
                                solved = true;
                                break;
                            }
                        }
                    }
                }

                if (success) {
                    char pathStart = state.Path.First().Value;
                    DNode pathEnd = state.Path.Last();
                    state.Steps += state.Path.Count() - 1;
                    int r = Array.IndexOf(state.Robot, pathStart);
                    if (r == -1)
                        throw new Exception();
                    state.Robot[r] = pathEnd.Value;
                    //state.OrderRobot = state.OrderRobot + r;

                    if (solved) {
                        if (state.Steps < partB) {
                            solutions.Clear();
                            partB = state.Steps;
                        }
                        //bool solAdded = solutions.TryAdd(state.OrderRobot + ":" + state.Order, state.Steps);
                        bool solAdded = solutions.TryAdd(state.Order, state.Steps);
                        if (!solAdded && state.Steps < solutions[state.Order])
                            solutions[state.Order] = state.Steps;
                    } else {
                        if (state.Steps > partB)
                            continue;
                        string rs = string.Join("", state.Robot);
                        foreach (KeyValuePair<(DNode, DNode), List<DNode>> option in keyPaths) {
                            if (!state.Order.Contains(option.Key.Item2.Value) && state.Robot.Contains(option.Key.Item1.Value)) {
                                (int Keys, string Robots, char Next) k = (state.Keys, rs, option.Key.Item2.Value);
                                bool added = memoryB.TryAdd(k, state.Steps);
                                if (!added) {
                                    if (state.Steps < memoryB[k])
                                        memoryB[k] = state.Steps;
                                    else
                                        continue;
                                }

                                MazeStateV2 next = new MazeStateV2(state, option.Value);
                                queueV2.Enqueue(next);
                            }
                        }
                    }
                }
            }

            if (solutions.Any()) {
                //partB = solutions.Min(s => s.Value);
                if (SHOW_ALL_BEST_FOUND) {
                    foreach (KeyValuePair<string, int> solution in solutions.Where(s => s.Value == partB))
                        Console.WriteLine("{0} = {1}", solution.Key, solution.Value);
                } else {
                    IEnumerable<KeyValuePair<string, int>> filtered = solutions.Where(s => s.Value == partB);
                    Console.WriteLine("{0}, one of them: {1} = {2}", filtered.Count(), filtered.First().Key, filtered.First().Value);
                }
            } else {
                partB = 0;
                Console.WriteLine("Found no solution.");
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 6098 (2 found, one of them: iatgwqsdfeykxnmjvpozbluhcr)
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1698 (48 found, one of them: iawqvytgsdefkxmnjzopbluhcr)
        }

        private static void DrawMap(List<DNode> nodes) { //, List<DNode> path
            int minY = nodes.Min(n => n.Y) - 1;
            int minX = nodes.Min(n => n.X) - 1;
            int maxY = nodes.Max(n => n.Y) + 1;
            int maxX = nodes.Max(n => n.X) + 1;

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    DNode node = nodes.Find(o => o.Y == y && o.X == x);
                    if (node != null) {
                        if (node.Value != '.')
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(node.Value);
                    } else {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("#");
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static bool IsBitSet(int num, int pos) {
            return (num & (1 << pos)) != 0;
        }

        private class MazeStateV1 {
            public int Keys;
            public int Steps;
            public List<DNode> Path;
            public string Order;

            public MazeStateV1(List<DNode> path) {
                Keys = 0;
                Steps = 0;
                Path = path;
                Order = string.Empty;
            }

            public MazeStateV1(MazeStateV1 prevState, List<DNode> path) {
                Keys = prevState.Keys;
                Steps = prevState.Steps;
                Path = path;
                Order = prevState.Order;
            }
        }

        private class MazeStateV2 {
            public int Keys;
            public int Steps;
            public List<DNode> Path;
            public string Order;
            public char[] Robot;
            //public string OrderRobot;

            public MazeStateV2(List<DNode> path) {
                Keys = 0;
                Steps = 0;
                Path = path;
                Order = string.Empty;
                Robot = new char[] { '@', '@', '@', '@' };
                //OrderRobot = string.Empty;
            }

            public MazeStateV2(MazeStateV2 prevState, List<DNode> path) {
                Keys = prevState.Keys;
                Steps = prevState.Steps;
                Path = path;
                Order = prevState.Order;
                Robot = new char[4];
                Array.Copy(prevState.Robot, Robot, 4);
                //OrderRobot = prevState.OrderRobot;
            }
        }
    }
}
