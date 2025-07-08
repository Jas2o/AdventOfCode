namespace AoC.Day
{
    public class Day10
    {
        public static void Run(string file) {
            Console.WriteLine("Day 10: Cathode-Ray Tube" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            Queue<string> queue = new Queue<string>(lines);
            string current = "";

            int x = 1;
            int cycles = 0;
            int cyclesNext = 0;
            int addNext = 0;

            int[] checkAt = [20, 60, 100, 140, 180, 220];
            int partA = 0;

            int crtX = -1;
            int crtY = 0;
            Dictionary<(int, int), char> crtXY = new Dictionary<(int, int), char>();
            int[] sprite = [0, 1, 2];

            //Solve Part 1 and 2, but only display Part 1.
            while (true) {
                if (current.Length == 0) {
                    if (!queue.Any())
                        break;
                    current = queue.Dequeue();

                    if (current == "noop")
                        cyclesNext = cycles + 1;
                    else {
                        cyclesNext = cycles + 2;
                        addNext = int.Parse(current.Substring(5));
                    }
                }

                cycles++;
                if (checkAt.Contains(cycles)) {
                    int strength = cycles * x;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0,-3} x={1,-2} strength is {2}", cycles, x, strength);
                    Console.ResetColor();
                    partA += strength;
                }

                crtX++;
                crtXY.Add((crtX, crtY), sprite.Contains(crtX) ? '#' : ' ');
                if (cycles % 40 == 0) {
                    crtX = -1;
                    crtY++;
                }

                if (cycles == cyclesNext) {
                    if (addNext != 0) {
                        x += addNext;
                        addNext = 0;
                        sprite = [x - 1, x, x + 1];
                    }
                    current = "";
                }

                Console.WriteLine("{0,-3} x={1,-2} {2}", cycles, x, current);
            }
            Console.WriteLine();

            //Display Part 2.
            for (crtY = 0; crtY < 6; crtY++) {
                for (crtX = 0; crtX < 40; crtX++)
                    Console.Write(crtXY[(crtX, crtY)]);
                Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("Part 1: " + partA);
            //Answer: 12560
            Console.WriteLine("Part 2: (you'll need to use your eyes to read the display above)");
            //Answer: PLPAFBCL
        }
    }
}
