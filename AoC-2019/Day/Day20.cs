using AoC.Graph;
using System.Data;

namespace AoC.Day
{
    public class Day20
    {
        //There's some unpleasant code in this one.

        private const int LEVEL_LIMIT = 30; //Suggest 30, but even up to 100 worked for my input (but increases time).

        public static void Run(string file) {
            Console.WriteLine("Day 20: Donut Maze" + Environment.NewLine);

            //Part 0 - Parse the input into something we can work with.
            string[] lines = File.ReadAllLines(file);
            List<DNode> nodes = new List<DNode>();
            for (int y = 0; y < lines.Length; y++) {
                for (int x = 0; x < lines[y].Length; x++) {
                    char c = lines[y][x];
                    if (c == '#' || c == ' ')
                        continue;
                    DNode node = new DNode(x, y, int.MaxValue, c);
                    nodes.Add(node);
                }
            }

            // Values for outside detection (the "inner" of the two letters).
            int minY = 1;// nodes.Min(n => n.Y) + 1;
            int minX = 1;// nodes.Min(n => n.X) + 1;
            int maxY = lines.Length - 2;// nodes.Max(n => n.Y) - 1;
            int maxX = nodes.Max(n => n.X) - 1;

            Dictionary<DNode, (string,char)> portals = new Dictionary<DNode, (string,char)>();
            DNode aa = null, zz = null;
            foreach(DNode letter in nodes.Where(n => n.Value != '.')) {
                List<DNode> neighbours = DNode.GetNeighbors(nodes, letter);
                if (neighbours.Any(n => n.Value == '.'))
                    continue;
                //We've got a letter that's on the outside.
                letter.Ignore = true; //Only the "inner" letter is needed for pathfinding.
                DNode n = neighbours[0];

                if(n.Value == letter.Value) {
                    if (letter.Value == 'A')
                        aa = n;
                    else if (letter.Value == 'Z')
                        zz = n;
                }

                string combined = (letter.X < n.X || letter.Y < n.Y) ? string.Concat(letter.Value, n.Value) : string.Concat(n.Value, letter.Value);
                char type = (n.X == minX || n.X == maxX || n.Y == minY || n.Y == maxY) ? '-' : '+';

                portals.Add(n, (combined, type));
            }

            List<List<((string, char), DNode)>> groups = new List<List<((string, char), DNode)>>();
            foreach (KeyValuePair<DNode, (string,char)> portal in portals) {
                List<((string,char), DNode)> hits = new List<((string, char), DNode)>();
                FloodFill(nodes, portals, portal.Key, 0, hits);
                if(hits.Any())
                    groups.Add(hits);
            }

            if (maxX > Console.WindowWidth - 2)
                Console.WriteLine("Window cannot fit drawing map." + Environment.NewLine);
            else
                DrawMap(nodes);

            //Go through the groups and work out the distances between portals in those groups.
            Dictionary<(string, string), int> distances = new Dictionary<(string, string), int>();
            foreach (List<((string,char), DNode)> group in groups) {
                foreach (((string, char), DNode) g1 in group) {
                    foreach (((string, char), DNode) g2 in group) {
                        if(g1 == g2) continue;
                        DNode.ResetDistances(nodes);
                        g1.Item2.Distance = 0;
                        DNode.Dijkstra(nodes);
                        List<DNode> path = DNode.GetPath(g2.Item2);

                        string g1n = string.Format("{0}{1}", g1.Item1.Item1, g1.Item1.Item2);
                        string g2n = string.Format("{0}{1}", g2.Item1.Item1, g2.Item1.Item2);
                        distances.Add((g1n, g2n), path.Count - 3);
                    }
                }
            }

            /* //Only applicable to the first example.
            if(distances.ContainsKey(("AA-","ZZ-"))) {
                int baseline = distances[("AA-", "ZZ-")];
                distances = distances.Where(d => d.Value <= baseline).ToDictionary();
            }
            */

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("# Part 1");
            Console.ResetColor();
            int partA = SolveA(distances, "AA-", "ZZ-");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("# Part 2");
            Console.ResetColor();
            int partB = SolveB(distances, "AA-", "ZZ-");

            Console.WriteLine("Part 1: " + partA);
            //Answer: 644
            Console.WriteLine("Part 2: " + partB);
            //Answer: 7798
        }

        private static int SolveA(Dictionary<(string, string), int> distances, string start, string end) {
            Dictionary<string, int> solutions = new Dictionary<string, int>();

            IEnumerable<KeyValuePair<(string, string), int>> options = distances.Where(d => d.Key.Item1 == start);
            foreach(KeyValuePair<(string, string), int> option in options) {
                List<string> path = new List<string>() { start, option.Key.Item2 };
                SolveASub(distances, end, solutions, path, option.Value);
            }

            if(solutions.Count == 1)
                Console.WriteLine("{0} solution found.", solutions.Count);
            else
                Console.WriteLine("{0} solutions found.", solutions.Count);
            solutions = solutions.OrderByDescending(s => s.Value).ToDictionary();
            foreach(KeyValuePair<string, int> solution in solutions) {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("> ");
                Console.ResetColor();
                Console.WriteLine("{0} = {1}", solution.Key, solution.Value);
            }
            Console.WriteLine();

            return solutions.Last().Value;
        }

        private static void SolveASub(Dictionary<(string, string), int> distances, string end, Dictionary<string, int> solutions, List<string> path, int current) {
            if (path.Contains(end)) {
                solutions.Add(string.Join(',', path), current + path.Count - 2);
                return;
            }

            string previous = path.Last();
            previous = previous.EndsWith('-') ? previous.Replace('-', '+') : previous.Replace('+', '-');

            IEnumerable<KeyValuePair<(string, string), int>> options = distances.Where(d => d.Key.Item1 == previous);
            foreach (KeyValuePair<(string, string), int> option in options) {
                if (path.Contains(option.Key.Item2))
                    continue;
                path.Add(option.Key.Item2);
                SolveASub(distances, end, solutions, path, current + option.Value);
                path.Remove(option.Key.Item2);
            }
        }

        private static int SolveB(Dictionary<(string, string), int> distances, string start, string end) {
            Dictionary<string, int> solutions = new Dictionary<string, int>();
            int bestresult = int.MaxValue;

            IEnumerable<KeyValuePair<(string, string), int>> options = distances.Where(d => d.Key.Item1 == start);
            foreach (KeyValuePair<(string, string), int> option in options) {
                Stack<string> path = new Stack<string>();
                path.Push(start);
                path.Push(option.Key.Item2);
                int level = option.Key.Item2.EndsWith('+') ? 1 : -1;

                SolveBSub(distances, start, end, solutions, path, option.Value, level, ref bestresult);
            }

            if (solutions.Count == 0)
                return -1;
            else if (solutions.Count == 1)
                Console.WriteLine("{0} solution found.", solutions.Count);
            else
                Console.WriteLine("{0} solutions found.", solutions.Count);
            solutions = solutions.OrderByDescending(s => s.Value).ToDictionary();
            foreach (KeyValuePair<string, int> solution in solutions.TakeLast(5)) {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("> ");
                Console.ResetColor();
                Console.WriteLine("{0} = {1}", solution.Key, solution.Value);
            }
            Console.WriteLine();

            return solutions.Last().Value;
        }

        private static void SolveBSub(Dictionary<(string, string), int> distances, string start, string end, Dictionary<string, int> solutions,
            Stack<string> path, int current, int level, ref int bestresult) {

            if (level == -1) {
                if (path.Peek() == end) {
                    int result = current + path.Count - 2;
                    solutions.Add(string.Join(',', path.Reverse()), result);

                    if (result < bestresult)
                        bestresult = result;
                    return;
                }
                return;
            }

            if (level > LEVEL_LIMIT || current >= bestresult) //current comparison was enough to prevent overflow with my input.
                return;

            string prev = path.Peek();
            prev = (prev.EndsWith('-') ? prev.Replace('-', '+') : prev.Replace('+', '-'));

            IEnumerable<KeyValuePair<(string, string), int>> options = distances.Where(d => d.Key.Item1 == prev);
            foreach (KeyValuePair<(string, string), int> option in options) {
                if (option.Key.Item2 == start || option.Key.Item2 == prev || (level != 0 && option.Key.Item2 == end))
                    continue;

                path.Push(option.Key.Item2);
                int leveldiff = option.Key.Item2.EndsWith('+') ? 1 : -1;
                SolveBSub(distances, start, end, solutions, path, current + option.Value, level + leveldiff, ref bestresult);
                path.Pop();
            }
        }

        private static void FloodFill(List<DNode> nodes, Dictionary<DNode, (string, char)> portals, DNode node, int regionID, List<((string, char), DNode)> hits) {
            List<DNode> neighbours = DNode.GetNeighbors(nodes, node);
            foreach (DNode n in neighbours) {
                if (n.Ignore || n.Distance != int.MaxValue)
                    continue;

                KeyValuePair<DNode, (string, char)> pair = portals.FirstOrDefault(p => p.Key == n);
                if (pair.Key != null)
                    hits.Add((pair.Value, pair.Key));

                n.Distance = regionID;
                FloodFill(nodes, portals, n, regionID+1, hits);
            }
        }

        private static void DrawMap(List<DNode> nodes) {
            int minY = nodes.Min(n => n.Y);
            int minX = nodes.Min(n => n.X);
            int maxY = nodes.Max(n => n.Y);
            int maxX = nodes.Max(n => n.X);

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    DNode node = nodes.Find(o => o.Y == y && o.X == x);
                    if (node != null) {
                        if (node.Ignore)
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                        else if (node.Distance == int.MaxValue)
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        else
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write(node.Value);
                    } else
                        Console.Write(" ");

                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
