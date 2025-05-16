namespace AoC.Day
{
    public class Day13
    {
        public static void Run(string file) {
            Console.WriteLine("Day 13: Transparent Origami" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            List<(int x, int y)> dots = new List<(int x, int y)>();
            List<string> instructions = new List<string>();
            bool showAll = false;

            foreach (string line in lines) {
                if(line.Contains(',')) {
                    string[] parts = line.Split(',');
                    int x = int.Parse(parts[0]);
                    int y = int.Parse(parts[1]);
                    dots.Add((x, y));
                    continue;
                }

                int eq = line.IndexOf('=');
                if(eq > -1) {
                    string sub = line.Substring(eq - 1);
                    instructions.Add(sub);
                }
            }

            int paperHeight = dots.Max(n => n.y);
            int paperWidth = dots.Max(n => n.x);

            if(showAll)
                DrawMap(dots, paperHeight, paperWidth);

            int partA = 0;

            foreach (string instruction in instructions) {
                string[] part = instruction.Split('=');
                int num = int.Parse(part[1]);
                if (part[0] == "y") {
                    List<(int x, int y)> onLine = dots.Where(d => d.y == num).ToList();
                    List<(int x, int y)> belowLine = dots.Where(d => d.y > num).ToList();

                    foreach ((int x, int y) dot in onLine)
                        dots.Remove(dot);
                    foreach ((int x, int y) dot in belowLine) {
                        dots.Remove(dot);
                        int newY = paperHeight - dot.y;

                        int dotIdx = dots.FindIndex(o => o.y == newY && o.x == dot.x);
                        if (dotIdx == -1)
                            dots.Add((dot.x, newY));
                    }

                    paperHeight = num - 1;
                } else { //x
                    List<(int x, int y)> onLine = dots.Where(d => d.x == num).ToList();
                    List<(int x, int y)> rightLine = dots.Where(d => d.x > num).ToList();

                    foreach ((int x, int y) dot in onLine)
                        dots.Remove(dot);
                    foreach ((int x, int y) dot in rightLine) {
                        dots.Remove(dot);
                        int newX = paperWidth - dot.x;
                        
                        int dotIdx = dots.FindIndex(o => o.y == dot.y && o.x == newX);
                        if(dotIdx == -1)
                            dots.Add((newX, dot.y));
                    }

                    paperWidth = num - 1;
                }

                if (partA == 0)
                    partA = dots.Count();

                if (showAll)
                    DrawMap(dots, paperHeight, paperWidth);
            }

            if (!showAll)
                DrawMap(dots, paperHeight, paperWidth);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 716
            Console.WriteLine("Part 2: (you'll need to use your eyes to read the display above)");
            //Answer: RPCKFBLR
        }

        private static void DrawMap(List<(int x, int y)> dots, int height, int width) {
            if (Console.WindowWidth < width) {
                Console.WriteLine("Window cannot fit drawing map." + Environment.NewLine);
                return;
            }

            for (int y = 0; y <= height; y++) {
                //Console.Write("{0,-4}", y);
                for (int x = 0; x <= width; x++) {
                    int dotIdx = dots.FindIndex(o => o.y == y && o.x == x);
                    if(dotIdx == -1)
                        Console.Write(" ");
                    else
                        Console.Write("#");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
