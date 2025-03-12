using AoC.Graph;

namespace AoC.Day
{
    public class Day24
    {
        public static void Run(string file) {
            Console.WriteLine("Day 24: Planet of Discord" + Environment.NewLine);

            List<DNode> nodesP1 = new List<DNode>();
            Dictionary<int, List<DNode>> nodesP2 = new Dictionary<int, List<DNode>>();
            nodesP2[0] = new List<DNode>();

            string[] lines = File.ReadAllLines(file);
            for (int y = 0; y < lines.Length; y++) {
                for (int x = 0; x < lines[y].Length; x++) {
                    char c = lines[y][x];
                    DNode n1 = new DNode(x, y, 0, c);
                    nodesP1.Add(n1);

                    if (y == 2 && x == 2)
                        c = '?';
                    DNode n2 = new DNode(x, y, 0, c);
                    nodesP2[0].Add(n2);
                }
            }

            //Part 1
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Part 1");
            Console.ResetColor();
            Console.WriteLine("Initial:");
            DrawMap(nodesP1);

            int time = 0;
            Dictionary<int, int> seen = new Dictionary<int, int>();
            while(true) {
                time++;
                //Check
                for (int y = 0; y < 5; y++) {
                    for (int x = 0; x < 5; x++) {
                        DNode current = nodesP1.Find(n => n.X == x && n.Y == y);
                        int adjacent = DNode.GetNeighbors(nodesP1, current).Where(n => n.Value == '#').Count();

                        if (current.Value == '#') {
                            current.Distance = 1;
                            if (adjacent != 1)
                                current.Distance = 0;
                        } else {
                            current.Distance = 0;
                            if (adjacent == 1 || adjacent == 2)
                                current.Distance = 1;
                        }
                    }
                }

                //Change
                int total = 0;
                for (int y = 0; y < 5; y++) {
                    int here = 0;
                    for (int x = 0; x < 5; x++) {
                        DNode node = nodesP1.Find(n => n.X == x && n.Y == y);
                        if (node.Distance > 0)
                            node.Value = '#';
                        else
                            node.Value = '.';
                        node.Distance = 0;

                        if(node.Value == '#')
                            here = here | (1 << x);
                    }
                    total = (total << 5) + here;
                }

                //Show and stop.
                bool added = seen.TryAdd(total, time);
                if (!added) {
                    Console.WriteLine("After {0} min:", time);
                    DrawMap(nodesP1);
                    break;
                }
            }

            //The current values are a repeat.
            long partA = 0;
            long power = 1;
            for (int y = 0; y < 5; y++) {
                for (int x = 0; x < 5; x++) {
                    DNode node = nodesP1.Find(n => n.X == x && n.Y == y);
                    if (node.Value == '#')
                        partA += power;
                    power += power;
                }
            }

            //Part 2
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Part 2");
            Console.ResetColor();

            int timeLimit = 200;
            if (timeLimit < 11) {    
                Console.WriteLine("Initial:");
                DrawMap(nodesP2[0]);
            }
            
            for(time = 1; time <= timeLimit; time++) {
                //Add layers if needed.
                int min = nodesP2.Keys.Min();
                List<DNode> lowest = nodesP2[min];
                bool addMinus = lowest.Where(n => n.Value == '#').Count() > 0;
                if(addMinus) {
                    min--;
                    nodesP2[min] = new List<DNode>();
                    for (int y = 0; y < 5; y++) {
                        for (int x = 0; x < 5; x++) {
                            char c = '.';
                            if (y == 2 && x == 2)
                                c = '?';
                            DNode n2 = new DNode(x, y, 0, c);
                            nodesP2[min].Add(n2);
                        }
                    }
                }
                int max = nodesP2.Keys.Max();
                List<DNode> highest = nodesP2[max];
                bool addPlus = highest.Where(n => n.Value == '#').Count() > 0;
                if(addPlus) {
                    max++;
                    nodesP2[max] = new List<DNode>();
                    for (int y = 0; y < 5; y++) {
                        for (int x = 0; x < 5; x++) {
                            char c = '.';
                            if (y == 2 && x == 2)
                                c = '?';
                            DNode n2 = new DNode(x, y, 0, c);
                            nodesP2[max].Add(n2);
                        }
                    }
                }

                //Check
                for (int level = min; level <= max; level++) {
                    for (int y = 0; y < 5; y++) {
                        for (int x = 0; x < 5; x++) {
                            if (y == 2 && x == 2)
                                continue;

                            DNode current = nodesP2[level].Find(n => n.X == x && n.Y == y);
                            int adjacent = DNode.GetNeighbors(nodesP2[level], current).Where(n => n.Value == '#').Count();
                            adjacent += GetRecursiveNeighbourBugs(nodesP2, min, level, max, current);

                            if (current.Value == '#') {
                                current.Distance = 1;
                                if (adjacent != 1)
                                    current.Distance = 0;
                            } else {
                                current.Distance = 0;
                                if (adjacent == 1 || adjacent == 2)
                                    current.Distance = 1;
                            }
                        }
                    }
                }

                //Change
                for (int level = min; level <= max; level++) {
                    for (int y = 0; y < 5; y++) {
                        for (int x = 0; x < 5; x++) {
                            if (y == 2 && x == 2)
                                continue;

                            DNode node = nodesP2[level].Find(n => n.X == x && n.Y == y);
                            if (node.Distance > 0)
                                node.Value = '#';
                            else
                                node.Value = '.';
                            node.Distance = 0;
                        }
                    }
                }

                //Show
                if (timeLimit < 11 || time == timeLimit) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("After {0} min:", time);
                    Console.ResetColor();
                    for (int level = nodesP2.Keys.Min(); level <= nodesP2.Keys.Max(); level++) {
                        Console.WriteLine("Depth {0}:", level);
                        DrawMap(nodesP2[level]);
                    }
                }
            }

            int partB = 0;
            foreach(List<DNode> layer in nodesP2.Values) {
                partB += layer.Where(n => n.Value == '#').Count();
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 25719471
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1916
        }

        private static int GetRecursiveNeighbourBugs(Dictionary<int, List<DNode>> nodes, int min, int level, int max, DNode current) {
            int result = 0;

            if (min < level) {
                if(current.Y == 0) {
                    DNode above = nodes[level - 1].Find(n => n.Y == 1 && n.X == 2);
                    if (above.Value == '#')
                        result++;
                }

                if (current.X == 0) {
                    DNode left = nodes[level - 1].Find(n => n.Y == 2 && n.X == 1);
                    if (left.Value == '#')
                        result++;
                }

                if (current.X == 4) {
                    DNode right = nodes[level - 1].Find(n => n.Y == 2 && n.X == 3);
                    if (right.Value == '#')
                        result++;
                }

                if (current.Y == 4) {
                    DNode below = nodes[level - 1].Find(n => n.Y == 3 && n.X == 2);
                    if (below.Value == '#')
                        result++;
                }
            }

            if (level < max) {
                if (current.Y == 1 && current.X == 2) {
                    IEnumerable<DNode> top = nodes[level + 1].Where(n => n.Y == 0 && n.Value == '#');
                    result += top.Count();
                }

                if (current.Y == 2 && current.X == 1) {
                    IEnumerable<DNode> left = nodes[level + 1].Where(n => n.X == 0 && n.Value == '#');
                    result += left.Count();
                }

                if (current.Y == 2 && current.X == 3) {
                    IEnumerable<DNode> right = nodes[level + 1].Where(n => n.X == 4 && n.Value == '#');
                    result += right.Count();
                }

                if (current.Y == 3 && current.X == 2) {
                    IEnumerable<DNode> bottom = nodes[level + 1].Where(n => n.Y == 4 && n.Value == '#');
                    result += bottom.Count();
                }
            }

            return result;
        }

        private static void DrawMap(List<DNode> nodes) {
            for (int y = 0; y < 5; y++) {
                for (int x = 0; x < 5; x++) {
                    DNode node = nodes.Find(o => o.Y == y && o.X == x);
                    Console.Write(node.Value);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
