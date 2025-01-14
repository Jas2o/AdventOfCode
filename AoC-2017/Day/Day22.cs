using System.Drawing;

namespace AoC.Day {
    public class Day22
    {
        public static void Run(string file) {
            Console.WriteLine("Day 22: Sporifica Virus" + Environment.NewLine);

            Dictionary<(int,int), char> nodesP1 = new Dictionary<(int,int), char>();
            Dictionary<(int,int), char> nodesP2 = new Dictionary<(int,int), char>();

            string[] lines = File.ReadAllLines(file);
            for (int y = 0; y < lines.Length; y++) {
                for (int x = 0; x < lines[y].Length; x++) {
                    char c = lines[y][x];
                    nodesP1.Add((x, y), c);
                    nodesP2.Add((x, y), c);
                }
            }

            int midY = nodesP1.Max(n => n.Key.Item2) / 2;
            int midX = nodesP1.Max(n => n.Key.Item1) / 2;
            int partA = Part1(nodesP1, midX, midY);
            int partB = Part2(nodesP2, midX, midY);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 5406
            Console.WriteLine("Part 2: " + partB);
            //Answer: 2511640
        }

        private static int Part1(Dictionary<(int, int), char> nodes, int midX, int midY) {
            DrawMap(nodes);

            Point carrier = new Point(midX, midY);
            Direction dir = Direction.Up;
            int witnessedInfections = 0;
            int bursts = 10000;
            for (int b = 0; b < bursts; b++) {
                if (!nodes.ContainsKey((carrier.X, carrier.Y)))
                    nodes[(carrier.X, carrier.Y)] = '.';
                char currentValue = nodes[(carrier.X, carrier.Y)];

                if (currentValue == '#') {
                    dir++;
                    if (dir == Direction.Imaginary)
                        dir = Direction.Up;
                } else {
                    dir--;
                    if (dir == Direction.None)
                        dir = Direction.Left;
                }

                //Modify the state of the current node.
                if (currentValue == '.') {
                    currentValue = '#';
                    witnessedInfections++;
                } else
                    currentValue = '.';
                nodes[(carrier.X, carrier.Y)] = currentValue;

                switch (dir) {
                    case Direction.Left: carrier.X--; break;
                    case Direction.Right: carrier.X++; break;
                    case Direction.Up: carrier.Y--; break;
                    case Direction.Down: carrier.Y++; break;
                }

                //if(bursts < 100) DrawMap(nodes);
            }

            DrawMap(nodes);

            return witnessedInfections;
        }

        private static int Part2(Dictionary<(int, int), char> nodes, int midX, int midY) {
            //DrawMap(nodes);
            
            Point carrier = new Point(midX, midY);
            Direction dir = Direction.Up;
            int witnessedInfections = 0;
            int bursts = 10000000;
            for (int b = 0; b < bursts; b++) {
                if(!nodes.ContainsKey((carrier.X, carrier.Y)))
                    nodes[(carrier.X, carrier.Y)] = '.';
                char currentValue = nodes[(carrier.X, carrier.Y)];

                if (currentValue == '.') {
                    dir--;
                    if (dir == Direction.None)
                        dir = Direction.Left;
                } else if (currentValue == 'W') {
                    //No change
                } else if (currentValue == '#') {
                    dir++;
                    if (dir == Direction.Imaginary)
                        dir = Direction.Up;
                } else if (currentValue == 'F') {
                    switch (dir) {
                        case Direction.Left: dir = Direction.Right; break;
                        case Direction.Right: dir = Direction.Left; break;
                        case Direction.Up: dir = Direction.Down; break;
                        case Direction.Down: dir = Direction.Up; break;
                    }
                } else throw new Exception();

                //Modify the state of the current node.
                if (currentValue == '.') {
                    currentValue = 'W';
                } else if (currentValue == 'W') {
                    currentValue = '#';
                    witnessedInfections++;
                } else if (currentValue == '#') {
                    currentValue = 'F';
                } else if (currentValue == 'F') {
                    currentValue = '.';
                } else throw new Exception();
                nodes[(carrier.X, carrier.Y)] = currentValue;

                switch (dir) {
                    case Direction.Left: carrier.X--; break;
                    case Direction.Right: carrier.X++; break;
                    case Direction.Up: carrier.Y--; break;
                    case Direction.Down: carrier.Y++; break;
                }

                //if(bursts < 100) DrawMap(nodes);
            }

            //DrawMap(nodes);

            return witnessedInfections;
        }

        internal enum Direction {
            None, Up, Right, Down, Left, Imaginary
        }

        private static void DrawMap(Dictionary<(int, int), char> nodes) {
            int minY = nodes.Min(n => n.Key.Item2);
            int minX = nodes.Min(n => n.Key.Item1);
            int maxY = nodes.Max(n => n.Key.Item2);
            int maxX = nodes.Max(n => n.Key.Item1);

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    if (nodes.ContainsKey((x, y)))
                        Console.Write(nodes[(x, y)]);
                    else
                        Console.Write(".");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
