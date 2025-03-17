using AoC.Graph;
using System.Text;

namespace AoC.Day
{
    public class Day18
    {
        public static void Run(string file) {
            Console.WriteLine("Day 18: Settlers of The North Pole" + Environment.NewLine);

            List<DNode> nodes = new List<DNode>();
            Dictionary<(int, int), Acre> acres = new Dictionary<(int, int), Acre>();

            string[] lines = File.ReadAllLines(file);
            for (int y = 0; y < lines.Length; y++) {
                for (int x = 0; x < lines[y].Length; x++) {
                    char c = lines[y][x];
                    DNode node = new DNode(x, y, 0, c);
                    nodes.Add(node);
                    acres.Add((x, y), new Acre(node));
                }
            }

            foreach (Acre acre in acres.Values) {
                acre.Adjacent = DNode.GetNeighborsWithDiagonals(nodes, acre.Node);
            }

            int dimension = lines[0].Length;
            //Part 1
            if (dimension < 11) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Part 1");
                Console.ResetColor();
                Console.WriteLine("Initial:");
                DrawMap(nodes, dimension);
            }

            Dictionary<string, int> memory = new Dictionary<string, int>();
            StringBuilder sb = new StringBuilder();

            int time = 1;
            for (; time <= 10; time++) {
                DoThing(acres, dimension);

                sb.Clear();
                foreach (DNode node in nodes)
                    sb.Append(node.Value);
                bool added = memory.TryAdd(sb.ToString(), time);

                //Show
                if (dimension < 11) {
                    Console.WriteLine("After {0} min:", time);
                    DrawMap(nodes, dimension);
                }
            }

            int partA_trees = nodes.Where(n => n.Value == '|').Count();
            int partA_lumberyards = nodes.Where(n => n.Value == '#').Count();
            int partA = partA_trees * partA_lumberyards;

            if (dimension < 11) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Part 2");
                Console.ResetColor();
            }

            int cap = 1000000000;
            bool skipped = false;
            for (; time <= cap; time++) {
                DoThing(acres, dimension);

                sb.Clear();
                foreach (DNode node in nodes)
                    sb.Append(node.Value);
                bool added = memory.TryAdd(sb.ToString(), time);

                if (!added) {
                    if (!skipped) {
                        //DrawMap(nodes, dimension);
                        int previous = memory[sb.ToString()];
                        int between = time - previous;
                        int remaining = ((cap - previous) % between);
                        Console.WriteLine("Repeat of {0} min seen at {1} min, there is {2} min between.", previous, time, between);
                        time = cap - remaining;
                        Console.WriteLine("Skipping to {0} min, {1} min remaining.", time, remaining);
                        skipped = true;
                    }
                }
            }

            int partB_trees = nodes.Where(n => n.Value == '|').Count();
            int partB_lumberyards = nodes.Where(n => n.Value == '#').Count();
            int partB = partB_trees * partB_lumberyards;

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 543312
            Console.WriteLine("Part 2: " + partB);
            //Answer: 199064
        }

        private static void DrawMap(List<DNode> nodes, int dimension) {
            for (int y = 0; y < dimension; y++) {
                for (int x = 0; x < dimension; x++) {
                    DNode node = nodes.Find(o => o.Y == y && o.X == x);
                    Console.Write(node.Value);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void DoThing(Dictionary<(int, int), Acre> acres, int dimension) {
            //Check
            for (int y = 0; y < dimension; y++) {
                for (int x = 0; x < dimension; x++) {
                    Acre current = acres[(x, y)];
                    int a_trees = current.Adjacent.Where(n => n.Value == '|').Count();
                    int a_lumber = current.Adjacent.Where(n => n.Value == '#').Count();

                    if (current.Node.Value == '.') { //0:open ground
                        if (a_trees > 2)
                            current.Node.Distance = 1;
                        else
                            current.Node.Distance = 0;
                    } else if (current.Node.Value == '|') { //1:trees
                        if (a_lumber > 2)
                            current.Node.Distance = 2;
                        else
                            current.Node.Distance = 1;
                    } else if (current.Node.Value == '#') { //2:lumberyard
                        if (a_trees > 0 && a_lumber > 0)
                            current.Node.Distance = 2;
                        else
                            current.Node.Distance = 0;
                    }
                }
            }

            //Change
            foreach (Acre acre in acres.Values) {
                if (acre.Node.Distance == 0)
                    acre.Node.Value = '.';
                else if (acre.Node.Distance == 1)
                    acre.Node.Value = '|';
                else if (acre.Node.Distance == 2)
                    acre.Node.Value = '#';
            }
        }

        private class Acre {
            public DNode Node;
            public List<DNode> Adjacent;

            public Acre(DNode node) {
                Node = node;
            }
        }
    }
}
