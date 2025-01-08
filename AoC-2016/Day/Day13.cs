using AoC.Graph;

namespace AoC.Day
{
    public class Day13
    {
        private static char space = '.';
        private static char wall = '#';
        private static char path = 'O';
        private static char within = 'X';

        public static void Run(string file) {
            Console.WriteLine("Day 13: A Maze of Twisty Little Cubicles" + Environment.NewLine);

            string input = File.ReadAllText(file);
            int designerNum = int.Parse(input);

            (int limitX, int limitY) = (50, 50);
            (int startX, int startY) = (1, 1);
            (int getX, int getY) = (31, 39);
            if (designerNum == 10)
                (getX, getY) = (7, 4);

            //Setup
            List<DNode> nodesAll = new List<DNode>();
            for (int y = 0; y < limitY; y++) {
                for (int x = 0; x < limitX; x++) {
                    int find = (x * x) + (3 * x) + (2 * x * y) + y + (y * y);
                    find += designerNum;
                    //string sbin = Convert.ToString(find,  2);
                    int numOfOne = Convert.ToString(find,  2).Replace("0", "").Length;
                    char c = (numOfOne % 2 == 0 ? space : wall);
                    DNode node = new DNode(x, y, int.MaxValue, c);
                    nodesAll.Add(node);
                }
            }

            //Solve
            List<DNode> nodesSpace = nodesAll.Where(n => n.Value == space).ToList();
            List<DNode> nodeVisited = new List<DNode>();
            DNode nodeEnd = nodesSpace.Find(n => n.X == getX && n.Y == getY);
            DNode nodeStart = nodesSpace.Find(n => n.X == startX && n.Y == startY);
            nodeStart.Distance = 0;
            DNode.Dijkstra(nodesSpace, nodeVisited);
            List<DNode> listPathNormal = DNode.GetPath(nodeEnd, path);

            int partA = nodeEnd.Distance;
            List<DNode> within50 = nodeVisited.Where(n => n.Distance <= 50).ToList();
            int partB = within50.Count();

            //Visualise
            int visLimitX = Math.Max(listPathNormal.Max(n => n.X), within50.Max(n => n.X)) + 2;
            int visLimitY = Math.Max(listPathNormal.Max(n => n.Y), within50.Max(n => n.Y)) + 2;
            foreach (DNode node in nodeVisited) {
                if (node.Distance > 50)
                    continue;
                if (node.Value != path)
                    node.Value = within;
            }
            DrawMap(nodesAll, visLimitX, visLimitY);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 90
            Console.WriteLine("Part 2: " + partB);
            //Answer: 135
        }

        private static void DrawMap(List<DNode> list, int maxX, int maxY) {
            for (int y = 0; y < maxY; y++) {
                Console.Write(y.ToString().PadRight(4));

                for (int x = 0; x < maxX; x++) {
                    DNode node = list.Find(o => o.Y == y && o.X == x);
                    char c = node.Value;
                    if (c == path)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else if (c == within)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (c == space)
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
