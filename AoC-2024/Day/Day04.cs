namespace AoC.Day
{
    public class Day04
    {
        public static void Run(string file) {
            Console.WriteLine("Day 4: Ceres Search" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            int foundA = 0;
            int foundB = 0;
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    char start = lines[y][x];

                    //Part 1
                    if (start == 'X' || start == 'S')
                    {
                        bool canLeft = x - 3 > -1;
                        bool canRight = x + 3 < lines[y].Length;
                        bool canDown = y + 3 < lines.Length;

                        bool right = false;
                        bool down = false;
                        bool downLeft = false;
                        bool downRight = false;

                        if (canRight)
                            right = CheckXMAS(start, lines[y][x + 1], lines[y][x + 2], lines[y][x + 3]);
                        if (canDown)
                        {
                            down = CheckXMAS(start, lines[y + 1][x], lines[y + 2][x], lines[y + 3][x]);

                            if (canLeft)
                                downLeft = CheckXMAS(start, lines[y + 1][x - 1], lines[y + 2][x - 2], lines[y + 3][x - 3]);

                            if (canRight)
                                downRight = CheckXMAS(start, lines[y + 1][x + 1], lines[y + 2][x + 2], lines[y + 3][x + 3]);
                        }

                        if (right) foundA++;
                        if (down) foundA++;
                        if (downLeft) foundA++;
                        if (downRight) foundA++;
                    }

                    //Part 2
                    if (start == 'A')
                    {
                        bool canLeft = x - 1 > -1;
                        bool canRight = x + 1 < lines[y].Length;
                        bool canUp = y - 1 > -1;
                        bool canDown = y + 1 < lines.Length;

                        if (canLeft && canRight && canUp && canDown)
                        {
                            bool upLeftDownRight = CheckMAS(lines[y - 1][x - 1], start, lines[y + 1][x + 1]);
                            bool downLeftUpRight = CheckMAS(lines[y + 1][x - 1], start, lines[y - 1][x + 1]);

                            if (upLeftDownRight && downLeftUpRight)
                                foundB++;
                        }
                    }
                }
            }

            Console.WriteLine("Part 1: " + foundA);
            //Answer: 2500

            Console.WriteLine("Part 2: " + foundB);
            //Answer: 1933
        }

        private static bool CheckXMAS(char a, char b, char c, char d)
        {
            //Check both directions

            if (a == 'X' && b == 'M' && c == 'A' && d == 'S')
                return true;

            if (a == 'S' && b == 'A' && c == 'M' && d == 'X')
                return true;

            return false;
        }

        private static bool CheckMAS(char a, char b, char c)
        {
            //Check from center

            if (a == 'M' && b == 'A' && c == 'S')
                return true;

            if (a == 'S' && b == 'A' && c == 'M')
                return true;

            return false;
        }
    }
}
