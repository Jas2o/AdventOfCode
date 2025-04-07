namespace AoC.Day
{
    public class Day12
    {
        public static void Run(string file) {
            Console.WriteLine("Day 12: Rain Risk" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            int partA = SolveA(lines);
            int partB = SolveB(lines);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 1710
            Console.WriteLine("Part 2: " + partB);
            //Answer: 62045
        }

        private enum Direction {
            North, East, South, West
        }

        private static int SolveA(string[] lines, bool verbose = false) {
            int shipX = 0, shipY = 0;
            Direction dir = Direction.East;

            foreach (string line in lines) {
                char first = line[0];
                int num = int.Parse(line.Substring(1));

                switch (first) {
                    case 'N': shipY -= num; break;
                    case 'S': shipY += num; break;
                    case 'E': shipX += num; break;
                    case 'W': shipX -= num; break;
                    case 'L':
                        int left = (360 - num) / 90;
                        dir = (Direction)((int)(dir + left) % 4);
                        break;
                    case 'R':
                        int right = num / 90;
                        dir = (Direction)((int)(dir + right) % 4);
                        break;
                    case 'F':
                        switch (dir) {
                            case Direction.North: shipY -= num; break;
                            case Direction.South: shipY += num; break;
                            case Direction.East: shipX += num; break;
                            case Direction.West: shipX -= num; break;
                        }
                        break;
                }

                if (verbose) {
                    string sx = (shipX > -1 ? "east " : "west ") + Math.Abs(shipX);
                    string sy = (shipY < 1 ? "north " : "south ") + Math.Abs(shipY);
                    Console.WriteLine("{0}\t{1}, {2}\t\t({3})", line, sx, sy, dir.ToString());
                }
            }

            if (verbose)
                Console.WriteLine();

            return Math.Abs(shipX) + Math.Abs(shipY);
        }

        private static int SolveB(string[] lines, bool verbose = false) {
            int shipX = 0, shipY = 0;
            int wayX = 10, wayY = -1;

            foreach (string line in lines) {
                char first = line[0];
                int num = int.Parse(line.Substring(1));

                switch (first) {
                    case 'N': wayY -= num; break;
                    case 'S': wayY += num; break;
                    case 'E': wayX += num; break;
                    case 'W': wayX -= num; break;
                    case 'L':
                        (wayX, wayY) = Rotate(shipX, shipY, wayX, wayY, (360 - num));
                        break;
                    case 'R':
                        (wayX, wayY) = Rotate(shipX, shipY, wayX, wayY, num);
                        break;
                    case 'F':
                        shipX += (num * wayX);
                        shipY += (num * wayY);
                        break;
                }

                if (verbose) {
                    string sx = (shipX > -1 ? "east " : "west ") + Math.Abs(shipX);
                    string sy = (shipY < 1 ? "north " : "south ") + Math.Abs(shipY);
                    Console.WriteLine("{0}\t{1}, {2}\t\t({3},{4})", line, sx, sy, wayX, wayY);
                }
            }

            if (verbose)
                Console.WriteLine();

            return Math.Abs(shipX) + Math.Abs(shipY);
        }

        private static (int wayX, int wayY) Rotate(int shipX, int shipY, int wayX, int wayY, int degrees) {
            double rad = Math.PI * degrees / 180.0;
            (double sin, double cos) = Math.SinCos(rad);
            double wX = wayX * cos - wayY * sin;
            double wY = wayY * cos + wayX * sin;
            return ((int)Math.Round(wX), (int)Math.Round(wY));
        }
    }
}
