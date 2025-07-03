namespace AoC.Day
{
    public class Day08
    {
        public static void Run(string file) {
            Console.WriteLine("Day 8: Treetop Tree House" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            bool outputExtraText = false;
            int dim = lines.Length;
            Dictionary<(int, int), Tree> trees = new Dictionary<(int, int), Tree>();
            for (int y = 0; y < dim; y++) {
                for (int x = 0; x < dim; x++) {
                    int height = (int)char.GetNumericValue(lines[y][x]);
                    trees[(x, y)] = new Tree(height);
                }
            }

            //Part 1
            //Edges down and up.
            for (int x = 0; x < dim; x++) {
                int prev = -1;
                for (int y = 0; y < dim; y++) {
                    Tree tree = trees[(x, y)];
                    if (tree.Height > prev) tree.Visibility++;
                    prev = Math.Max(prev, tree.Height);
                }
                prev = -1;
                for (int y = dim - 1; y >= 0; y--) {
                    Tree tree = trees[(x, y)];
                    if (tree.Height > prev) tree.Visibility++;
                    prev = Math.Max(prev, tree.Height);
                }
            }
            //Edges left and right.
            for (int y = 0; y < dim; y++) {
                int prev = -1;
                for (int x = 0; x < dim; x++) {
                    Tree tree = trees[(x, y)];
                    if (tree.Height > prev) tree.Visibility++;
                    prev = Math.Max(prev, tree.Height);
                }
                prev = -1;
                for (int x = dim - 1; x >= 0; x--) {
                    Tree tree = trees[(x, y)];
                    if (tree.Height > prev) tree.Visibility++;
                    prev = Math.Max(prev, tree.Height);
                }
            }
            int partA = trees.Values.Where(v => v.Visibility > 0).Count();
            DrawMap(trees, dim);

            //Part 2
            for (int y = 1; y < dim - 1; y++) {
                for (int x = 1; x < dim - 1; x++) {
                    Tree tree = trees[(x, y)];
                    int up = Lookout(trees, x, y, dim, 0, -1);
                    int left = Lookout(trees, x, y, dim, -1, 0);
                    int right = Lookout(trees, x, y, dim, 1, 0);
                    int down = Lookout(trees, x, y, dim, 0, 1);
                    tree.ScenicScore = up * left * right * down;

                    if(outputExtraText)
                        Console.WriteLine("{0},{1} has height {2} can see {3},{4},{5},{6} = {7}",
                        x, y, tree.Height, up, left, right, down, tree.ScenicScore);
                }
                if(outputExtraText)
                    Console.WriteLine();
            }
            KeyValuePair<(int, int), Tree> best = trees.MaxBy(t => t.Value.ScenicScore);
            int partB = best.Value.ScenicScore;

            Console.WriteLine("Part 1: {0}", partA);
            //Answer: 1688
            Console.WriteLine("Part 2: {0} (height {1} at {2},{3})", partB, best.Value.Height, best.Key.Item1, best.Key.Item2);
            //Answer: 410400
        }

        private static int Lookout(Dictionary<(int, int), Tree> trees, int x, int y, int dim, int diffX, int diffY) {
            int count = 0;
            Tree from = trees[(x, y)];
            while (true) {
                x += diffX;
                y += diffY;
                if (x < 0 || x == dim || y < 0 || y == dim)
                    break;
                count++;
                if (trees[(x, y)].Height >= from.Height)
                    break;
            }
            return count;
        }

        private static void DrawMap(Dictionary<(int, int), Tree> trees, int limitXY) {
            for (int y = 0; y < limitXY; y++) {
                for (int x = 0; x < limitXY; x++) {
                    Tree tree = trees[(x, y)];
                    if (tree.Visibility > 0)
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    else
                        Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(tree.Height);
                }
                Console.WriteLine();
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        private class Tree {
            public int Height;
            public int Visibility;
            public int ScenicScore;

            public Tree(int height) {
                Height = height;
                Visibility = 0;
                ScenicScore = 0;
            }
        }
    }
}
