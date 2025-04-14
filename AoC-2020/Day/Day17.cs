namespace AoC.Day {
    public class Day17 {

        //Part 2 is very slow and could be revisited in future.

        private const int X = 0;
        private const int Y = 1;
        private const int Z = 2;
        private const int W = 3;

        public static void Run(string file) {
            Console.WriteLine("Day 17: Conway Cubes" + Environment.NewLine);

            List<int[]> nodesP1 = new List<int[]>();
            List<int[]> nodesP2 = new List<int[]>();

            string[] lines = File.ReadAllLines(file);
            for (int y = 0; y < lines.Length; y++) {
                for (int x = 0; x < lines[y].Length; x++) {
                    if (lines[y][x] == '#') {
                        nodesP1.Add([x, y, 0, 0]);
                        nodesP2.Add([x, y, 0, 0]);
                    }
                }
            }

            Console.WriteLine("Initial:");
            DrawMultiMap(nodesP1, false);
            int partA = Solve(nodesP1, false, true);

            Console.WriteLine("Working on Part 2, this can take a while..." + Environment.NewLine);
            int partB = Solve(nodesP2, true, false);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 293
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1816
        }

        private static int Solve(List<int[]> nodes, bool is4D, bool display) {
            List<int[]> coords = new List<int[]>();
            for (int w = -1; w <= 1; w++) {
                if (!is4D && w != 0)
                    continue;
                for (int z = -1; z <= 1; z++) {
                    for (int y = -1; y <= 1; y++) {
                        for (int x = -1; x <= 1; x++) {
                            if (x == 0 && y == 0 && z == 0 && w == 0)
                                continue;
                            coords.Add([x, y, z, w]);
                        }
                    }
                }
            }

            //--

            int timeLimit = 6;
            for (int time = 1; time <= timeLimit; time++) {
                int minX = nodes.Min(n => n[X]) - 1;
                int maxX = nodes.Max(n => n[X]) + 1;
                int minY = nodes.Min(n => n[Y]) - 1;
                int maxY = nodes.Max(n => n[Y]) + 1;
                int minZ = nodes.Min(n => n[Z]) - 1;
                int maxZ = nodes.Max(n => n[Z]) + 1;
                int minW = (is4D ? nodes.Min(n => n[W]) - 1 : 0);
                int maxW = (is4D ? nodes.Max(n => n[W]) + 1 : 0);

                List<int[]> becomeActive = new List<int[]>();
                List<int[]> becomeInactive = new List<int[]>();
                
                //Check
                for (int w = minW; w <= maxW; w++) {
                    for (int z = minZ; z <= maxZ; z++) {
                        for (int y = minY; y <= maxY; y++) {
                            for (int x = minX; x <= maxX; x++) {
                                int[] node = nodes.Find(o => o[X] == x && o[Y] == y && o[Z] == z && o[W] == w);
                                List<int[]> neighbors = GetNeighbors(nodes, [x, y, z, w], coords);
                                int neighborsA = neighbors.Count();

                                if (node != null) {
                                    if (neighborsA < 2 || neighborsA > 3)
                                        becomeInactive.Add(node);
                                } else {
                                    if (neighborsA == 3)
                                        becomeActive.Add([x, y, z, w]);
                                }
                            }
                        }
                    }
                }

                //Change
                foreach (int[] n in becomeActive)
                    nodes.Add(n);
                foreach (int[] n in becomeInactive)
                    nodes.Remove(n);

                //Show
                if (display) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("After {0} min:", time);
                    Console.ResetColor();
                    DrawMultiMap(nodes, is4D);
                }
            }

            return nodes.Count();
        }

        private static void DrawMultiMap(List<int[]> nodes, bool is4D) {
            int minX = nodes.Min(n => n[X]);
            int maxX = nodes.Max(n => n[X]);
            int minY = nodes.Min(n => n[Y]);
            int maxY = nodes.Max(n => n[Y]);
            int minZ = nodes.Min(n => n[Z]);
            int maxZ = nodes.Max(n => n[Z]);
            int minW = nodes.Min(n => n[W]);
            int maxW = nodes.Max(n => n[W]);

            for (int w = minW; w <= maxW; w++) {
                for (int z = minZ; z <= maxZ; z++) {
                    if(is4D)
                        Console.WriteLine("Z: {0}, W: {1}", z, w);
                    else
                        Console.WriteLine("Z: " + z);

                    for (int y = minY; y <= maxY; y++) {
                        for (int x = minX; x <= maxX; x++) {
                            int[] node = nodes.Find(o => o[Y] == y && o[X] == x && o[Z] == z);
                            if (node != null)
                                Console.Write('#');
                            else
                                Console.Write('.');
                            }
                            Console.WriteLine();
                        }
                    Console.WriteLine();
                }
            }
        }

        public static List<int[]> GetNeighbors(List<int[]> listNodes, int[] origin, List<int[]> coords) {
            List<int[]> neighbors = new List<int[]>();

            foreach (int[] c in coords) {
                int[] node = listNodes.Find(n =>
                    n[X] == origin[X] + c[X] &&
                    n[Y] == origin[Y] + c[Y] &&
                    n[Z] == origin[Z] + c[Z] &&
                    n[W] == origin[W] + c[W]);
                if (node != null)
                    neighbors.Add(node);
            }

            return neighbors;
        }
    }

}
