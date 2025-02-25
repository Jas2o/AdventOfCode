namespace AoC.Day
{
    public class Day11
    {
        public static void Run(string file) {
            Console.WriteLine("Day 11: Space Police" + Environment.NewLine);

			string input = File.ReadAllText(file);
            if (input.IndexOf(',') == -1) {
                Console.WriteLine("There is no test for this day.");
                return;
            }
            long[] initial = Array.ConvertAll(input.Split(','), long.Parse);

            Dictionary<(int, int), int> panelsA = new Dictionary<(int, int), int>();
            RunRobot(panelsA, initial);
            int partA = panelsA.Keys.Count();

            Dictionary<(int, int), int> panelsB = new Dictionary<(int, int), int>();
            panelsB[(0, 0)] = 1;
            RunRobot(panelsB, initial);
            DrawMap(panelsB);
            //int partB_check = panelsB.Keys.Count();

            Console.WriteLine("Part 1: " + partA);
            //Answer: 2343
            Console.WriteLine("Part 2: (you'll need to use your eyes to read the display above)");
            //Answer: JFBERBUH
        }

        private enum Direction {
            None, Up, Right, Down, Left, Imaginary
        }

        private static void RunRobot(Dictionary<(int, int), int> dPanels, long[] initial) {
            int x = 0;
            int y = 0;
            Direction dir = Direction.Up;

            int first = (dPanels.ContainsKey((x, y)) ? 1 : 0);
            IntCode computerA = new IntCode(initial, [first]);
            while (!computerA.Halted) {
                computerA.Run();

                long paint = computerA.outputQueue.Dequeue();
                long turn = computerA.outputQueue.Dequeue();

                dPanels[(x, y)] = (int)paint;

                if (turn == 0)
                    dir--;
                else if (turn == 1)
                    dir++;
                else
                    throw new Exception();

                if (dir == Direction.None)
                    dir = Direction.Left;
                else if (dir == Direction.Imaginary)
                    dir = Direction.Up;

                //Robot moves after turning
                switch (dir) {
                    case Direction.Up: y--; break;
                    case Direction.Right: x++; break;
                    case Direction.Down: y++; break;
                    case Direction.Left: x--; break;
                }

                if (dPanels.ContainsKey((x, y)))
                    computerA.inputQueue.Enqueue(dPanels[(x, y)]);
                else
                    computerA.inputQueue.Enqueue(0);
            }
        }

        private static void DrawMap(Dictionary<(int, int), int> nodes) {
            int minY = nodes.Min(n => n.Key.Item2);
            int minX = nodes.Min(n => n.Key.Item1);
            int maxY = nodes.Max(n => n.Key.Item2);
            int maxX = nodes.Max(n => n.Key.Item1);

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    if (nodes.ContainsKey((x, y))) {
                        int c = nodes[(x, y)];
                        if (c == 1)
                            Console.BackgroundColor = ConsoleColor.White;
                        Console.Write(' ');
                        Console.ResetColor();
                    } else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
