using AoC.Graph;

namespace AoC.Day {
    public class Day15
    {
        private const char start = 's';
        private const char oxygen = 'e';

        public static void Run(string file) {
            Console.WriteLine("Day 15: Oxygen System" + Environment.NewLine);

			string input = File.ReadAllText(file);
            if (input.IndexOf(',') == -1) {
                Console.WriteLine("There is no test for this day.");
                return;
            }
            long[] initial = Array.ConvertAll(input.Split(','), long.Parse);
            List<DNode> nodesAll = new List<DNode>();

            //Part 1
            int partA = RunRobot(nodesAll, initial);

            //Part 2
            DNode.ResetDistancesAndIgnore(nodesAll);
            DNode nodeSource = nodesAll.Find(n => n.Value == oxygen);
            FloodFill(nodesAll, nodeSource);
            int partB = nodesAll.Max(n => n.Distance);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 266
            Console.WriteLine("Part 2: " + partB);
            //Answer: 274
        }

        private enum Direction {
            None, Up, Right, Down, Left, Imaginary
        }

        private static int RunRobot(List<DNode> nodes, long[] initial) {
            int x = 0;
            int y = 0;
            nodes.Add(new DNode(x, y, int.MaxValue, start));

            //When the robot finds it, it will then continue the same logic back until the start point to complete the map.
            bool oxygenFound = false;

            Direction dir = Direction.Up;
            IntCode computerA = new IntCode(initial, []);
            while (!computerA.Halted) {
                int tryX = x;
                int tryY = y;

                if (dir == Direction.Up) {
                    tryY--; //North
                    computerA.inputQueue.Enqueue(1);
                } else if (dir == Direction.Left) {
                    tryX--; //West
                    computerA.inputQueue.Enqueue(3);
                } else if (dir == Direction.Right) {
                    tryX++; //East
                    computerA.inputQueue.Enqueue(4);
                } else if (dir == Direction.Down) {
                    tryY++; //South
                    computerA.inputQueue.Enqueue(2);
                }

                computerA.Run();
                int response = (int)computerA.outputQueue.Dequeue();
                if (response == 0) {
                    //Hit a wall, turn right.
                    dir++;
                } else {
                    //Not a wall, forward and turn left.
                    x = tryX;
                    y = tryY;
                    dir--;
                    DNode test = nodes.Find(n => n.X == x && n.Y == y);
                    if (test == null) {
                        if (response == 1)
                            nodes.Add(new DNode(x, y, int.MaxValue, ' '));
                        else if (response == 2) {
                            nodes.Add(new DNode(x, y, int.MaxValue, oxygen));
                            oxygenFound = true;
                        }
                    }
                }

                if (dir == Direction.None)
                    dir = Direction.Left;
                else if (dir == Direction.Imaginary)
                    dir = Direction.Up;

                if (oxygenFound && x == 0 && y == 0) {
                    //It should have explored all other areas before returning to start.
                    break;
                }
            }

            //Get the number of steps between start and oxygen.
            List<DNode> nodesSpace = nodes.Where(n => n.Value != '#').ToList();
            List<DNode> nodeVisited = new List<DNode>();
            DNode nodeEnd = nodesSpace.Find(n => n.Value == oxygen);
            DNode nodeStart = nodesSpace.Find(n => n.X == 0 && n.Y == 0);
            nodeStart.Distance = 0;
            DNode.Dijkstra(nodesSpace, nodeVisited);
            List<DNode> listPath = DNode.GetPath(nodeEnd);

            //Draw the map, highlighed will be the path.
            DrawMap(nodes, listPath);

            return nodeEnd.Distance;
        }

        private static void DrawMap(List<DNode> nodes, List<DNode> path) {
            int minY = nodes.Min(n => n.Y);
            int minX = nodes.Min(n => n.X);
            int maxY = nodes.Max(n => n.Y);
            int maxX = nodes.Max(n => n.X);

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    DNode node = nodes.Find(o => o.Y == y && o.X == x);
                    if (node != null) {
                        if (path.Contains(node))
                            Console.BackgroundColor = ConsoleColor.Green;
                        if (node.Value != ' ')
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write(node.Value);
                    } else
                        Console.Write("#");

                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        //Slimmed down from 2018 Day 15
        private static void FloodFill(List<DNode> nodes, DNode homeNode) {
            Queue<DNode> queue = new Queue<DNode>();
            homeNode.Distance = 0;
            queue.Enqueue(homeNode);

            while (queue.Any()) {
                DNode currentNode = queue.Dequeue();
                List<DNode> neighbours = DNode.GetNeighbors(nodes, currentNode);

                foreach (DNode n in neighbours) {
                    if (n.Distance == int.MaxValue && !n.Ignore) {
                        queue.Enqueue(n);
                        n.Ignore = true;
                    }
                }

                int minDist = neighbours.Min(n => n.Distance);
                if (currentNode != homeNode)
                    currentNode.Distance = minDist + 1;
            }
        }
    }
}
