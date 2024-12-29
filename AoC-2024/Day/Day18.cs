using System;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace AoC.Day
{
    public class Day18
    {
        // Reusing Dijkstra's algorithm from Day 16 of 2024.

        public static void Run(string file) {
            Console.WriteLine("Day 18: RAM Run" + Environment.NewLine);

            List<DNode> listUnvisited = new List<DNode>();
            List<DNode> listVisited = new List<DNode>();
            List<DNode> listCorrupted = new List<DNode>();

            string[] lines = File.ReadAllLines(file);
            (int range, int sims) = (71, 1024);
            if (lines.Length < 30)
                (range,sims) = (7, 13);
            char[][] grid = new char[range][];
            for (int y = 0; y < range; y++) {
                grid[y] = new char[range];
                for (int x = 0; x < range; x++) {
                    DNode node = new DNode(x, y, int.MaxValue);
                    listUnvisited.Add(node);
                    //grid[y][x] = '.';
                    grid[y][x] = ' ';
                }
            }

            DNode nodeStart = listUnvisited.Find(n => n.X == 0 && n.Y == 0);
            DNode nodeEnd = listUnvisited.Find(n => n.X == (range - 1) && n.Y == (range - 1));
            nodeStart.Distance = 0;
            nodeStart.Value = 'S';
            nodeEnd.Value = 'E';
            grid[0][0] = 'S';
            grid[range - 1][range - 1] = 'E';

            for (int i = 0; i < sims; i++) {
                string[] fields = lines[i].Split(',');
                int x = int.Parse(fields[0]);
                int y = int.Parse(fields[1]);
                DNode node = listUnvisited.Find(n => n.X == x && n.Y == y);
                node.Value = '#';
                listCorrupted.Add(node);
                listUnvisited.Remove(node);
                grid[y][x] = '#';
            }

            Console.WriteLine("Dijkstra's algorithm start to end...\r\n");
            Dijkstra(listUnvisited, listVisited);

            int steps = 0;
            DNode nodeCurrent = nodeEnd;
            List<DNode> listSafePath = new List<DNode>();
            while(nodeCurrent.Previous != null) {
                nodeCurrent = nodeCurrent.Previous;
                listSafePath.Add(nodeCurrent);
                grid[nodeCurrent.Y][nodeCurrent.X] = 'O';
                steps++;
            }

            for (int y = 0; y < range; y++) {
                for (int x = 0; x < range; x++) {
                    char c = grid[y][x];
                    if (c == '#')
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(c);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            Console.WriteLine("\r\nStart the madness!\r\n");

            //Part 2
            string partB = string.Empty;
            for (int i = sims; i < lines.Length; i++) {
                //Get the next corrupted coordinate and take it out of the pool.
                string[] fields = lines[i].Split(',');
                int x = int.Parse(fields[0]);
                int y = int.Parse(fields[1]);
                DNode simNode = listVisited.Find(n => n.X == x && n.Y == y);
                simNode.Value = '#';
                listCorrupted.Add(simNode);
                listVisited.Remove(simNode);
                grid[y][x] = '#';

                //This new corruption doesn't impact the path.
                if (!listSafePath.Contains(simNode))
                    continue;

                //It does impact, reset things.
                listUnvisited.AddRange(listVisited);
                listVisited.Clear();
                foreach(DNode vNode in listUnvisited) {
                    vNode.Distance = int.MaxValue;
                    vNode.Previous = null;
                }
                nodeStart.Distance = 0;

                //Run it again
                Dijkstra(listUnvisited, listVisited);
                DNode testEnd = nodeEnd;
                while(true) {
                    if (testEnd.Previous == null)
                        break;
                    grid[testEnd.Y][testEnd.X] = 'O';
                    //Might be a new safe path
                    if(!listSafePath.Contains(testEnd))
                        listSafePath.Add(testEnd);
                    testEnd = testEnd.Previous;
                }
                if(testEnd != nodeStart) {
                    partB = string.Format("{0},{1}", x, y);
                    grid[y][x] = 'X';
                    break;
                }
            }

            for (int y = 0; y < range; y++) {
                for (int x = 0; x < range; x++) {
                    char c = grid[y][x];
                    if (c == '#')
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    else if (c == 'X')
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(c);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + steps);
            //Answer: 318
            Console.WriteLine("Part 2: " + partB);
            //Answer: 56,29
        }

        private static void Dijkstra(List<DNode> listUnvisited, List<DNode> listVisited) {
            bool loop = true;
            while (loop) {
                if (listUnvisited.Count == 0) {
                    loop = false;
                    break;
                }
                DNode currentNode = listUnvisited.MinBy(n => n.Distance);
                List<DNode> neighbors = GetNeighbors(listUnvisited, currentNode);
                foreach (DNode nextNode in neighbors) {
                    if (listVisited.Contains(nextNode))
                        continue;
                    int distance = currentNode.Distance + 1;
                    if (distance < nextNode.Distance) {
                        nextNode.Distance = distance;
                        nextNode.Previous = currentNode;
                    }
                }
                listVisited.Add(currentNode);
                listUnvisited.Remove(currentNode);
            }
        }

        //--

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

        private class DNode {
            public int X;
            public int Y;
            public int Distance;
            public DNode? Previous;
            public char Value;

            public DNode(int x, int y, int distance, char value = '.') {
                X = x;
                Y = y;
                Distance = distance;
            }

            public override string ToString() {
                return string.Format("{0},{1}", X, Y);
            }

        }
    }
}
