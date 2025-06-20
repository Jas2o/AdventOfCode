using AoC.Graph;

namespace AoC.Day
{
    public class Day23
    {
        // Currently VERY SLOW, 3 min 45 sec for the example, 5 min 49 sec for my given input.

        public static void Run(string file) {
            Console.WriteLine("Day 23: Amphipod" + Environment.NewLine);

			string[] linesA = File.ReadAllLines(file);

            string[] linesB = new string[] {
                linesA[0],
                linesA[1],
                linesA[2],
                "  #D#C#B#A#",
                "  #D#B#A#C#",
                linesA[3],
                linesA[4],
            };

            Console.WriteLine("Working on Part 1...");
            int partA = Solve(linesA);
            Console.Beep();

            Console.WriteLine();
            Console.WriteLine("Working on Part 2...");
            int partB = Solve(linesB);
            Console.Beep();

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 13495
            Console.WriteLine("Part 2: " + partB);
            //Answer: 53767
        }

        private static int Solve(string[] lines) {
            Dictionary<(int, int), char> stateOriginal = new Dictionary<(int, int), char>();
            List<DNode> nodes = new List<DNode>();

            List<(int, int)> inHallway = new List<(int, int)>() { (1, 1), (2, 1), (4, 1), (6, 1), (8, 1), (10, 1), (11, 1) };
            Dictionary<char, List<(int, int)>> starts = new Dictionary<char, List<(int, int)>>() {
                { 'A', new List<(int, int)>() },
                { 'B', new List<(int, int)>() },
                { 'C', new List<(int, int)>() },
                { 'D', new List<(int, int)>() }
            };
            List<(int, int)> destA = new List<(int, int)>() { (3, 2), (3, 3) };
            List<(int, int)> destB = new List<(int, int)>() { (5, 2), (5, 3) };
            List<(int, int)> destC = new List<(int, int)>() { (7, 2), (7, 3) };
            List<(int, int)> destD = new List<(int, int)>() { (9, 2), (9, 3) };

            if (lines.Length > 5) {
                destA.Add((3, 4)); destA.Add((3, 5));
                destB.Add((5, 4)); destB.Add((5, 5));
                destC.Add((7, 4)); destC.Add((7, 5));
                destD.Add((9, 4)); destD.Add((9, 5));
            }

            char[] lookfor = ['A', 'B', 'C', 'D', '.'];
            for (int y = 0; y < lines.Length; y++) {
                for (int x = 0; x < lines[y].Length; x++) {
                    char c = lines[y][x];
                    if (!lookfor.Contains(c))
                        continue;

                    (int, int) xy = (x, y);
                    stateOriginal[xy] = c;
                    DNode node = new DNode(x, y, int.MaxValue, c);
                    nodes.Add(node);

                    if (c == 'A') starts['A'].Add(xy);
                    if (c == 'B') starts['B'].Add(xy);
                    if (c == 'C') starts['C'].Add(xy);
                    if (c == 'D') starts['D'].Add(xy);
                }
            }

            DrawState(stateOriginal);

            Dictionary<char, List<PathOption>> paths = new Dictionary<char, List<PathOption>>() {
                { 'A', new List<PathOption>() },
                { 'B', new List<PathOption>() },
                { 'C', new List<PathOption>() },
                { 'D', new List<PathOption>() }
            };

            FindPaths(nodes, starts['A'], destA, paths['A']);
            FindPaths(nodes, starts['B'], destB, paths['B']);
            FindPaths(nodes, starts['C'], destC, paths['C']);
            FindPaths(nodes, starts['D'], destD, paths['D']);

            FindPaths(nodes, starts['A'], inHallway, paths['A']);
            FindPaths(nodes, starts['B'], inHallway, paths['B']);
            FindPaths(nodes, starts['C'], inHallway, paths['C']);
            FindPaths(nodes, starts['D'], inHallway, paths['D']);

            FindPaths(nodes, inHallway, destA, paths['A']);
            FindPaths(nodes, inHallway, destB, paths['B']);
            FindPaths(nodes, inHallway, destC, paths['C']);
            FindPaths(nodes, inHallway, destD, paths['D']);

            Dictionary<string, int> seen = new Dictionary<string, int>();
            PriorityQueue<Dictionary<(int, int), char>, int> priorityQueue = new PriorityQueue<Dictionary<(int, int), char>, int>();
            priorityQueue.Enqueue(stateOriginal.ToDictionary(), 0);
            seen.Add(new string(stateOriginal.Values.ToArray()), 0);

            int best = int.MaxValue;

            Dictionary<(int, int), char> prevState;
            int priority;
            while (priorityQueue.TryDequeue(out prevState, out priority)) {
                if (priority > best)
                    continue;

                List<KeyValuePair<(int, int), char>> optionsFirst = new List<KeyValuePair<(int, int), char>>();
                IEnumerable<KeyValuePair<(int, int), char>> optionsA = prevState.Where(d => d.Value == 'A');
                IEnumerable<KeyValuePair<(int, int), char>> optionsB = prevState.Where(d => d.Value == 'B');
                IEnumerable<KeyValuePair<(int, int), char>> optionsC = prevState.Where(d => d.Value == 'C');
                IEnumerable<KeyValuePair<(int, int), char>> optionsD = prevState.Where(d => d.Value == 'D');
                if (optionsA.Any(o => o.Key.Item1 != 3)) optionsFirst.AddRange(optionsA);
                if (optionsB.Any(o => o.Key.Item1 != 5)) optionsFirst.AddRange(optionsB);
                if (optionsC.Any(o => o.Key.Item1 != 7)) optionsFirst.AddRange(optionsC);
                if (optionsD.Any(o => o.Key.Item1 != 9)) optionsFirst.AddRange(optionsD);

                if (!optionsFirst.Any()) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    DrawState(prevState);
                    if (priority < best) {
                        best = priority;
                        Console.WriteLine(priority);
                    }
                    Console.ResetColor();
                }

                foreach (KeyValuePair<(int, int), char> optionFirst in optionsFirst) {
                    IEnumerable<PathOption> destinations = paths[optionFirst.Value].Where(p => p.Start == optionFirst.Key);

                    foreach (PathOption destination in destinations) {
                        bool valid = true;
                        for (int i = 1; i < destination.Path.Count; i++) {
                            if (prevState[destination.Path[i]] != '.') {
                                valid = false;
                                break;
                            }
                        }

                        if (valid) {
                            Dictionary<(int, int), char> newState = prevState.ToDictionary();
                            newState[destination.Start] = prevState[destination.End];
                            newState[destination.End] = prevState[destination.Start];

                            int cost = 0;
                            switch (optionFirst.Value) {
                                case 'A': cost = 1; break;
                                case 'B': cost = 10; break;
                                case 'C': cost = 100; break;
                                case 'D': cost = 1000; break;
                            }
                            cost *= (destination.Path.Count() - 1);
                            
                            int newPriority = priority + cost;
                            string s = new string(newState.Values.ToArray());

                            bool t = seen.TryAdd(s, newPriority);
                            if (!t) {
                                if (seen[s] > newPriority)
                                    seen[s] = newPriority;
                                else
                                    continue;
                            }

                            priorityQueue.Enqueue(newState, newPriority);
                        }
                    }
                }
            }

            return best;
        }

        private static void FindPaths(List<DNode> nodes, List<(int, int)> coord1, List<(int, int)> coord2, List<PathOption> paths) {
            foreach ((int, int) c1 in coord1) {
                DNode.ResetDistances(nodes);
                DNode n1 = nodes.Find(n => n.X == c1.Item1 && n.Y == c1.Item2);
                n1.Distance = 0;
                DNode.Dijkstra(nodes);

                foreach ((int, int) c2 in coord2) {
                    if (c1 == c2)
                        continue;
                    DNode n2 = nodes.Find(n => n.X == c2.Item1 && n.Y == c2.Item2);
                    List<DNode> pathN = DNode.GetPath(n2, true);
                    List<(int, int)> path = new List<(int, int)>();
                    foreach (DNode n in pathN)
                        path.Add((n.X, n.Y));

                    paths.Add(new PathOption(c1, c2, path));
                }
            }
        }

        private class PathOption {
            public (int, int) Start;
            public (int, int) End;
            public List<(int, int)> Path;

            public PathOption((int,int) s, (int, int) e, List<(int, int)> path) {
                Start = s;
                End = e;
                Path = path;
            }

            public override string ToString() {
                return string.Format("{0},{1} to {2},{3}", Start.Item1, Start.Item2, End.Item1, End.Item2);
            }
        }

        private static void DrawState(Dictionary<(int, int), char> state) {
            Console.WriteLine("{0}{1}.{2}.{3}.{4}.{5}{6}", state[(1, 1)], state[(2, 1)], state[(4, 1)], state[(6, 1)], state[(8, 1)], state[(10, 1)], state[(11, 1)]);
            Console.WriteLine("  {0} {1} {2} {3}", state[(3, 2)], state[(5, 2)], state[(7, 2)], state[(9, 2)]);
            Console.WriteLine("  {0} {1} {2} {3}", state[(3, 3)], state[(5, 3)], state[(7, 3)], state[(9, 3)]);

            if (state.ContainsKey((9, 5))) {
                Console.WriteLine("  {0} {1} {2} {3}", state[(3, 4)], state[(5, 4)], state[(7, 4)], state[(9, 4)]);
                Console.WriteLine("  {0} {1} {2} {3}", state[(3, 5)], state[(5, 5)], state[(7, 5)], state[(9, 5)]);
            }
        }
    }
}
