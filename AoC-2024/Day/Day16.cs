namespace AoC.Day {
    public class Day16
    {
        // Using Dijkstra's and Backtracking algorithms.
        // I hate the code, but it got the answers for my input.

        public static void Run(string file) {
            Console.WriteLine("Day 16: Reindeer Maze" + Environment.NewLine);

            //Build map
            string[] all = File.ReadAllLines(file);
            int[][] map = new int[all.Length][];
            int height = all.Length;
            int width = all[0].Length;

            int startX = -1, startY = -1;
            int endX = -1, endY = -1;

            for (int y = 0; y < height; y++) {
                map[y] = new int[width];
                for (int x = 0; x < width; x++) {
                    char c = all[y][x];
                    if (c == '#')
                        map[y][x] = 8;
                    else if (c == 'S')
                        (startX, startY) = (x, y);
                    else if (c == 'E')
                        (endX, endY) = (x, y);
                }
            }

            //Part 1 using Dijkstra
            //Wikipedia step 1
            List<DNode> listUnvisited = new List<DNode>();
            List<DNode> listVisited = new List<DNode>();
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    //Wikipedia step 2
                    int c = map[y][x];
                    if (c == 8) // or #
                        continue;

                    DNode node = new DNode(x, y, int.MaxValue);
                    if (y == startY && x == startX) {
                        node.Distance = 0;
                        node.Dir = Direction.Right;
                    }
                    listUnvisited.Add(node);
                }
            }

            Console.WriteLine("Dijkstra's algorithm start to end...");
            bool loop = true;
            while (loop) {
                //Wikipedia step 3
                if (listUnvisited.Count == 0) {
                    loop = false;
                    break;
                }

                DNode currentNode = listUnvisited.MinBy(n => n.Distance);

                List<DNode> neighbors = GetNeighbors(listUnvisited, currentNode);
                foreach (DNode nextNode in neighbors) {
                    //Wikipedia step 4
                    if (listVisited.Contains(nextNode))
                        continue;

                    int cost = nextNode.CalcCost(currentNode, out Direction nextDir);
                    int distance = currentNode.Distance + cost;

                    if (distance < nextNode.Distance) {
                        nextNode.Distance = distance;
                        nextNode.Previous = currentNode;
                        nextNode.Cost = cost;
                        if (cost == 1)
                            nextNode.Dir = currentNode.Dir;
                        else
                            nextNode.Dir = nextDir;
                    }
                }

                //Wikipedia step 5
                listVisited.Add(currentNode);
                listUnvisited.Remove(currentNode);
            }
            //Wikipedia step 6

            DNode endNode = listVisited.Find(n => n.X == endX && n.Y == endY);
            //The answer for Part 1 is endNode.Distance.

            //Part 2 with Backtracking
            List<DNode[]> backtrackPaths = new List<DNode[]>();
            List<DNode> currentPath = new List<DNode>();
            Console.WriteLine("Backtracking to check for other paths, poorly...");
            FindPath(listVisited, endNode, endNode.Distance, map, startY, startX, backtrackPaths, currentPath);
            Console.WriteLine("Got paths, checking their costs...");

            foreach (DNode[] path in backtrackPaths) {
                Array.Reverse(path);
                int checkCost = 0;
                DNode previous = null;
                Direction previousDir = Direction.Right;
                foreach (DNode node in path) {
                    if(previous == null) {
                        previous = node;
                        continue;
                    }

                    //We just want the calculated direction, the one saved on the node is not suitable.
                    int cost = node.CalcCost(previous, out Direction nextDir);
                    checkCost += 1;
                    if(previousDir != nextDir) {
                        checkCost += 1000;
                        previousDir = nextDir;
                    }

                    previous = node;
                }
                //If the checked cost is good for the end node, then fill in those spots.
                if (checkCost < endNode.Distance) {
                    foreach (DNode node in path) {
                        map[node.Y][node.X] = 1;
                    }
                }
            }

            int countTilesOnBestPaths = 1; //Including start
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (map[y][x] == 1)
                        countTilesOnBestPaths++;
                }
            }

            DrawMap(ref map, startX, startY, endX, endY);

            Console.WriteLine();
            Console.WriteLine("Part 1: " + endNode.Distance);
            //Answer: 98416
            Console.WriteLine("Part 2: " + countTilesOnBestPaths);
            //Answer: 471
        }

        private static List<DNode> GetNeighbors(List<DNode> listNodes, DNode? currentNode) {
            List<DNode> neighbors = new List<DNode>();

            DNode up = listNodes.Find(n => n.X == currentNode.X && n.Y + 1 == currentNode.Y);
            DNode down = listNodes.Find(n => n.X == currentNode.X && n.Y - 1 == currentNode.Y);
            DNode left = listNodes.Find(n => n.X + 1 == currentNode.X && n.Y == currentNode.Y);
            DNode right = listNodes.Find(n => n.X - 1 == currentNode.X && n.Y == currentNode.Y);

            if (up != null) neighbors.Add(up);
            if (down != null) neighbors.Add(down);
            if (left != null) neighbors.Add(left);
            if (right != null) neighbors.Add(right);

            return neighbors;
        }

        private static void FindPath(List<DNode> listNodes, DNode currentNode, int maxDistance, int[][] map, int destY, int destX, List<DNode[]> ans, List<DNode> currentPath) {
            //For Part 2
            //https://www.geeksforgeeks.org/rat-in-a-maze/
            //"Fun fact", this is what I started with for trying to solve Part 1, but it was too slow for the real input.
            //When being recycled, it was quickly adjusted to work in reverse.

            if (currentNode.Distance > maxDistance)
                return;

            if (currentNode.Y == destY && currentNode.X == destX) {
                ans.Add(currentPath.ToArray());
                return;
            }

            // Mark the current cell as blocked
            map[currentNode.Y][currentNode.X] = 9; //0

            bool canUp = map[currentNode.Y - 1][currentNode.X] == 0;
            bool canDown = map[currentNode.Y + 1][currentNode.X] == 0;
            bool canLeft = map[currentNode.Y][currentNode.X - 1] == 0;
            bool canRight = map[currentNode.Y][currentNode.X + 1] == 0;
            if (canUp) {
                DNode nodeUp = listNodes.Find(n => n.Y == currentNode.Y - 1 && n.X == currentNode.X);
                if (nodeUp.Distance < currentNode.Distance + 1000) {
                    currentPath.Add(nodeUp);
                    FindPath(listNodes, nodeUp, maxDistance, map, destY, destX, ans, currentPath);
                    currentPath.Remove(nodeUp);
                }
            }
            if (canDown) {
                DNode nodeDown = listNodes.Find(n => n.Y == currentNode.Y + 1 && n.X == currentNode.X);
                if (nodeDown.Distance < currentNode.Distance + 1000) {
                    currentPath.Add(nodeDown);
                    FindPath(listNodes, nodeDown, maxDistance, map, destY, destX, ans, currentPath);
                    currentPath.Remove(nodeDown);
                }
            }
            if (canLeft) {
                DNode nodeLeft = listNodes.Find(n => n.Y == currentNode.Y && n.X == currentNode.X - 1);
                if (nodeLeft.Distance < currentNode.Distance + 1000) {
                    currentPath.Add(nodeLeft);
                    FindPath(listNodes, nodeLeft, maxDistance, map, destY, destX, ans, currentPath);
                    currentPath.Remove(nodeLeft);
                }
            }
            if (canRight) {
                DNode nodeRight = listNodes.Find(n => n.Y == currentNode.Y && n.X == currentNode.X + 1);
                if (nodeRight.Distance < currentNode.Distance + 1000) {
                    currentPath.Add(nodeRight);
                    FindPath(listNodes, nodeRight, maxDistance, map, destY, destX, ans, currentPath);
                    currentPath.Remove(nodeRight);
                }
            }

            map[currentNode.Y][currentNode.X] = 0;
        }

        private static void DrawMap(ref int[][] map, int startX, int startY, int endX, int endY) {
            for (int y = 0; y < map.Length; y++) {
                for (int x = 0; x < map[y].Length; x++) {
                    int c = map[y][x];

                    if (y == startY && x == startX) {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write('>');
                        //Console.Write("    0");
                    } else {
                        if (y == endY && x == endX)
                            Console.BackgroundColor = ConsoleColor.Magenta;
                        else if (c == 8)
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        else if(c > 0)
                            Console.BackgroundColor = ConsoleColor.DarkRed;

                        if (c == 0 || c == int.MaxValue)
                            Console.Write(' ');
                            //Console.Write("     ");
                        else
                            Console.Write(c);
                            //Console.Write(string.Format("{0}", c).PadLeft(5));
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        private enum Direction {
            None, Up, Right, Down, Left, Imaginary
        }

        private class DNode {
            public int X;
            public int Y;
            public int Distance; //From start
            public DNode? Previous;
            public Direction Dir;
            public int Cost; //Considering direction of this and previous

            public DNode(int x, int y, int distance) {
                X = x;
                Y = y;
                Distance = distance;
                Dir = Direction.None;
            }

            public override string ToString() {
                return string.Format("{0},{1}", X, Y);
            }

            public int CalcCost(DNode currentNode, out Direction nextDir) {
                nextDir = currentNode.Dir;
                int diffX = currentNode.X - X;
                int diffY = currentNode.Y - Y;

                int cost = 1;
                if (diffY == 1) {
                    if (currentNode.Dir != Direction.Up) {
                        cost += 1000;
                        nextDir = Direction.Up;
                    }
                } else if (diffY == 0) {
                    if (diffX == 1) {
                        if (currentNode.Dir != Direction.Left) {
                            cost += 1000;
                            nextDir = Direction.Left;
                        }
                    } else if (diffX == -1) {
                        if (currentNode.Dir != Direction.Right) {
                            cost += 1000;
                            nextDir = Direction.Right;
                        }
                    } else
                        throw new Exception();
                } else if (diffY == -1 && currentNode.Dir != Direction.Down) {
                    if (currentNode.Dir != Direction.Down) {
                        cost += 1000;
                        nextDir = Direction.Down;
                    }
                }
                return cost;
            }
        }
    }
}
