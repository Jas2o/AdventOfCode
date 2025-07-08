namespace AoC.Day
{
    public class Day09
    {
        public static void Run(string file) {
            Console.WriteLine("Day 9: Rope Bridge" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            bool displayText = lines.Length < 10;

            int partA = Solve(lines, 1, displayText);
            int partB = Solve(lines, 9, displayText);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 6376
            Console.WriteLine("Part 2: " + partB);
            //Answer: 2607
        }

        private class Knot {
            public int ID;
            public int X;
            public int Y;

            public Knot(int id) {
                ID = id;
                X = 0;
                Y = 0;
            }
        }

        private static int Solve(string[] lines, int afterHead, bool displayText) {
            Dictionary<int, Knot> rope = new Dictionary<int, Knot>();
            for(int i = 0; i <= afterHead; i++)
                rope.Add(i, new Knot(i));

            HashSet<(int, int)> tailVisited = new HashSet<(int, int)>();

            if (displayText) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("== Initial State ==");
                Console.ResetColor();
                Display(rope);
            }

            foreach (string line in lines) {
                if (displayText) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("== {0} ==", line);
                    Console.ResetColor();
                }

                char dir = line[0];
                int num = int.Parse(line.Substring(2));

                for (int step = 0; step < num; step++) {
                    switch (dir) {
                        case 'U': rope[0].Y--; break;
                        case 'D': rope[0].Y++; break;
                        case 'L': rope[0].X--; break;
                        case 'R': rope[0].X++; break;
                    }

                    for (int pos = 1; pos <= afterHead; pos++) {
                        int diffX = rope[pos - 1].X - rope[pos].X;
                        int diffY = rope[pos - 1].Y - rope[pos].Y;

                        if (diffX < -1 || diffX > 1 || diffY < -1 || diffY > 1) {
                            if (diffX != 0)
                                rope[pos].X += (diffX > 0 ? 1 : -1);
                            if (diffY != 0)
                                rope[pos].Y += (diffY > 0 ? 1 : -1);
                        }

                        if (pos == afterHead)
                            tailVisited.Add((rope[pos].X, rope[pos].Y));
                    }
                    if (displayText) Display(rope);
                }
            }

            return tailVisited.Count();
        }

        private static void Display(Dictionary<int, Knot> rope) {
            int minX = Math.Min(rope.Min(r => r.Value.X), 0);
            int minY = Math.Min(rope.Min(r => r.Value.Y), -5);
            int maxX = Math.Max(rope.Max(r => r.Value.X), 5);
            int maxY = Math.Max(rope.Max(r => r.Value.Y), 0);

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    Knot first = rope.Values.Where(k => k.X == x && k.Y == y).FirstOrDefault();
                    if (first != null) {
                        if(first.ID == 0)
                            Console.Write('H');
                        else
                            Console.Write(first.ID);
                    } else if (x == 0 && y == 0)
                        Console.Write('s');
                    else
                        Console.Write('.');
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("{0,3}", y);
                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }
}
