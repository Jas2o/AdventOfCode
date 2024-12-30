namespace AoC.Day {
    public class Day01
    {
        public static void Run(string file) {
            Console.WriteLine("Day 1: No Time for a Taxicab" + Environment.NewLine);

            string text = File.ReadAllText(file);
            string[] steps = text.Split(", ");

            int x = 0;
            int y = 0;

            Dictionary<(int, int), int> locations = new Dictionary<(int, int), int>() {
                {(0, 0), 0 }
            };
            List<int> revisitedBlocksAway = new List<int>();

            Direction dir = Direction.Up;
            foreach (string step in steps) {
                char turn = step[0];
                int distance = int.Parse(step.Substring(1));

                if (turn == 'R')
                    dir = dir + 1;
                else
                    dir = dir - 1;

                if (dir == Direction.None)
                    dir = Direction.Left;
                else if (dir == Direction.Imaginary)
                    dir = Direction.Up;

                for (int i = 0; i < distance; i++) {
                    switch (dir) {
                        case Direction.Up: y--; break;
                        case Direction.Right: x++; break;
                        case Direction.Down: y++; break;
                        case Direction.Left: x--; break;
                    }

                    int manhattan = Math.Abs(x) + Math.Abs(y);
                    bool added = locations.TryAdd((x, y), manhattan);
                    if (!added) {
                        revisitedBlocksAway.Add(manhattan);
                    }
                }
            }
            revisitedBlocksAway.Add(0);

            Console.WriteLine("Part 1: " + locations.Last().Value);
            //Answer: 242
            Console.WriteLine("Part 2: " + revisitedBlocksAway.First());
            //Answer: 150
        }

        private enum Direction {
            None, Up, Right, Down, Left, Imaginary
        }
    }
}
