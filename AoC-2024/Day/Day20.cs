using AoC.Graph;

namespace AoC.Day
{
    public class Day20
    {
        public static void Run(string file) {
            Console.WriteLine("Day 20: Race Condition" + Environment.NewLine);

            List<DNode> listUnvisited = new List<DNode>();
            List<DNode> listVisited = new List<DNode>();
            List<DNode> listWall = new List<DNode>();

            string[] lines = File.ReadAllLines(file);
            int height = lines.Length;
            int width = lines[0].Length;

            (int startX, int startY) = (-1, -1);
            (int endX, int endY) = (-1, -1);

            DNode nodeStart = null;
            DNode nodeEnd = null;
            char[][] grid = new char[height][];
            for (int y = 0; y < height; y++) {
                grid[y] = new char[width];
                for (int x = 0; x < width; x++) {
                    char c = lines[y][x];
                    if (c == '.')
                        c = ' ';
                    grid[y][x] = c;

                    DNode node = new DNode(x, y, int.MaxValue, c);

                    if (c == '#')
                        listWall.Add(node);
                    else
                        listUnvisited.Add(node);

                    if (c == 'S') {
                        (startX, startY) = (x, y);
                        nodeStart = node;
                        nodeStart.Distance = 0;
                    } else if (c == 'E') {
                        (endX, endY) = (x, y);
                        nodeEnd = node;
                    }
                }
            }
            if (nodeStart == null || nodeEnd == null)
                return;

            //DrawMap(grid, startX, startY, endX, endY);

            DNode.Dijkstra(listUnvisited, listVisited);
            List<DNode> listPathNormal = DNode.GetPath(nodeEnd, 'O');
            int stepsNormally = listPathNormal.Count() - 1;
            Console.WriteLine("Without cheating, it takes {0} picoseconds.", stepsNormally);

            Dictionary<int, int> dictionaryCheats = new Dictionary<int, int>();

            //Scan listPathNormal and find all the walls next to them.
            List<DNode> listCheatWall = new List<DNode>();
            foreach (DNode normal in listPathNormal) {
                List<DNode> walls = DNode.GetNeighbors(listWall, normal);
                foreach (DNode wall in walls) {
                    if (listCheatWall.Contains(wall))
                        continue;
                    listCheatWall.Add(wall);
                    if (wall.Y == 0 || wall.Y == height - 1 || wall.X == 0 || wall.X == width - 1)
                        continue;

                    List<DNode> neighbors = DNode.GetNeighbors(listPathNormal, wall);
                    foreach (DNode nextnormal in neighbors) {
                        if (nextnormal == normal)
                            continue;

                        int diff = Math.Abs(normal.Distance - nextnormal.Distance) - 2;
                        if (diff < 1)
                            continue;
                        int stepsCheatingTest = stepsNormally - diff;
                        if (dictionaryCheats.ContainsKey(stepsCheatingTest))
                            dictionaryCheats[stepsCheatingTest]++;
                        else
                            dictionaryCheats[stepsCheatingTest] = 1;
                    }
                }
            }

            foreach (KeyValuePair<int,int> pair in dictionaryCheats.OrderByDescending(d => d.Key)) {
                if (pair.Value == 1)
                    Console.WriteLine("There is one cheat that saves {0} picoseconds.", stepsNormally - pair.Key);
                else
                    Console.WriteLine("There are {0} cheats that save {1} picoseconds.", pair.Value, stepsNormally - pair.Key);
            }
            Console.WriteLine();

            if(height < 20)
                DrawMap(grid, startX, startY, endX, endY);

            int howManyAtLeast100 = dictionaryCheats.Where(d => stepsNormally - d.Key >= 100).Sum(d => d.Value);

            dictionaryCheats.Clear(); //FOR TESTING

            //Part 2
            int howManyAtLeast100b = 0;
            for(int n1 = 0; n1 < listPathNormal.Count; n1++) {
                for (int n2 = n1 + 1; n2 < listPathNormal.Count; n2++) {
                    //Avoid duplicates by just not starting at the same place.
                    DNode node1 = listPathNormal[n1];
                    DNode node2 = listPathNormal[n2];

                    int manhattan = Math.Abs(node1.X - node2.X) + Math.Abs(node1.Y - node2.Y);
                    if (manhattan > 20)
                        continue;

                    int diff = Math.Abs(node1.Distance - node2.Distance) - manhattan;
                    //if (diff < 50) //Test
                    if (diff < 100) //Real
                        continue;

                    howManyAtLeast100b++;
                    //Too slow to store them in a dictionary
                }
            }

            Console.WriteLine("Part 1: " + howManyAtLeast100);
            //Answer: 1417
            Console.WriteLine("Part 2: " + howManyAtLeast100b);
            //Answer: 1014683
        }

        private static void DrawMap(char[][] map, int startX, int startY, int endX, int endY) {
            for (int y = 0; y < map.Length; y++) {
                for (int x = 0; x < map[y].Length; x++) {
                    char c = map[y][x];
                    if (y == startY && x == startX)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if(y == endY && x == endX)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else if(c == '#')
                        Console.ForegroundColor = ConsoleColor.DarkGray;

                    Console.Write(c);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
