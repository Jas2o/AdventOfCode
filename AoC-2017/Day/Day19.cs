using AoC.Graph;
using System.Text;

namespace AoC.Day {
    public class Day19
    {
        public static void Run(string file) {
            Console.WriteLine("Day 19: A Series of Tubes" + Environment.NewLine);

            List<DNode> listNodes = new List<DNode>();
            string[] lines = File.ReadAllLines(file);
            for (int y = 0; y < lines.Length; y++) {
                for (int x = 0; x < lines[y].Length; x++) {
                    char c = lines[y][x];
                    if (c == ' ')
                        continue;

                    char v = '#';
                    if (c != '|' && c != '-' && c != '+')
                        v = c;

                    DNode node = new DNode(x, y, int.MaxValue, v);
                    listNodes.Add(node);
                }
            }

            StringBuilder sb = new StringBuilder();

            int steps = 0;
            DNode previousNode = null;
            DNode currentNode = listNodes.First();
            Direction dir = Direction.Down;
            bool loop = true;
            while(loop) {
                if (currentNode != previousNode) {
                    previousNode = currentNode;
                    steps++;
                }
                if (currentNode.Value != '#')
                    sb.Append(currentNode.Value);

                DNode up = listNodes.Find(n => n.X == currentNode.X && n.Y + 1 == currentNode.Y);
                DNode down = listNodes.Find(n => n.X == currentNode.X && n.Y - 1 == currentNode.Y);
                DNode left = listNodes.Find(n => n.X + 1 == currentNode.X && n.Y == currentNode.Y);
                DNode right = listNodes.Find(n => n.X - 1 == currentNode.X && n.Y == currentNode.Y);

                if (dir == Direction.Up) {
                    if(up == null) {
                        if (left != null)
                            dir = Direction.Left;
                        else if (right != null)
                            dir = Direction.Right;
                        else
                            loop = false;
                    } else {
                        currentNode = up;
                    }
                } else if (dir == Direction.Down) {
                    if (down == null) {
                        if (right != null)
                            dir = Direction.Right;
                        else if (left != null)
                            dir = Direction.Left;
                        else
                            loop = false;
                    } else {
                        currentNode = down;
                    }
                } else if (dir == Direction.Left) {
                    if (left == null) {
                        if (down != null)
                            dir = Direction.Down;
                        else if (up != null)
                            dir = Direction.Up;
                        else
                            loop = false;
                    } else {
                        currentNode = left;
                    }
                } else if (dir == Direction.Right) {
                    if (right == null) {
                        if (up != null)
                            dir = Direction.Up;
                        else if (down != null)
                            dir = Direction.Down;
                        else
                            loop = false;
                    } else {
                        currentNode = right;
                    }
                }
            }

            Console.WriteLine("Part 1: " + sb.ToString());
            //Answer: EPYDUXANIT
            Console.WriteLine("Part 2: " + steps);
            //Answer: 17544
        }

        private enum Direction {
            None, Up, Right, Down, Left, Imaginary
        }
    }
}
