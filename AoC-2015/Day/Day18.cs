namespace AoC.Day {
    public class Day18
    {
        public static void Run(string file) {
            Console.WriteLine("Day 18: Like a GIF For Your Yard" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            int width = lines[0].Length;
            int height = lines.Length;

            bool[][] lights = new bool[height][];
            SetupLights(lines, width, height, ref lights);

            //Part 1
            int steps = 100;
            if (width < 10)
                steps = 4;
            Console.WriteLine("After {0} steps", steps);
            for(int s = 0; s < steps; s++) {
                LightsChange(width, height, ref lights, false);
            }
            //DrawMap(ref lights);

            int countOn_A = 0;
            for (int y = 0; y < height; y++) {
                countOn_A += lights[y].Count(x => x == true);
            }

            //Part 2
            SetupLights(lines, width, height, ref lights);
            if (width < 10)
                steps = 5;
            for (int s = 0; s < steps; s++) {
                LightsChange(width, height, ref lights, true);
            }
            DrawMap(ref lights);

            int countOn_B = 0;
            for (int y = 0; y < height; y++) {
                countOn_B += lights[y].Count(x => x == true);
            }

            Console.WriteLine("Part 1: " + countOn_A);
            //Answer: 814
            Console.WriteLine("Part 2: " + countOn_B);
            //Answer: 924
        }

        private static void SetupLights(string[] lines, int width, int height, ref bool[][] lights) {
            for (int y = 0; y < height; y++) {
                lights[y] = new bool[width];
                for (int x = 0; x < width; x++) {
                    lights[y][x] = (lines[y][x] == '#');
                }
            }
        }

        private static void LightsChange(int width, int height, ref bool[][] mapA, bool part2) {
            bool[][] mapB = new bool[height][];

            if(part2) {
                mapA[0][0] = true;
                mapA[0][width - 1] = true;
                mapA[height - 1][0] = true;
                mapA[height - 1][width - 1] = true;
            }

            for (int y = 0; y < height; y++) {
                mapB[y] = new bool[width];
                for (int x = 0; x < width; x++) {
                    int neighbors = 0;
                    bool canUp = y > 0;
                    bool canDown = (y+1) < height;
                    bool canLeft = x > 0;
                    bool canRight = (x+1) < width;

                    if (canUp) {
                        if (canLeft && mapA[y - 1][x - 1])
                            neighbors++;
                        if (mapA[y - 1][x])
                            neighbors++;
                        if (canRight && mapA[y - 1][x + 1])
                            neighbors++;
                    }
                    if (canLeft && mapA[y][x - 1])
                        neighbors++;
                    if (canRight && mapA[y][x + 1])
                        neighbors++;
                    if (canDown) {
                        if (canLeft && mapA[y + 1][x - 1])
                            neighbors++;
                        if (mapA[y + 1][x])
                            neighbors++;
                        if (canRight && mapA[y + 1][x + 1])
                            neighbors++;
                    }

                    if(mapA[y][x]) {
                        //Stays on if 2 or 3 neighbors
                        if (neighbors == 2 || neighbors == 3)
                            mapB[y][x] = true;
                        else
                            mapB[y][x] = false;
                    } else {
                        if (neighbors == 3)
                            mapB[y][x] = true;
                    }
                }
            }

            if (part2) {
                mapB[0][0] = true;
                mapB[0][width - 1] = true;
                mapB[height - 1][0] = true;
                mapB[height - 1][width - 1] = true;
            }

            mapA = mapB;
        }

        private static void DrawMap(ref bool[][] map) {
            for (int y = 0; y < map.Length; y++) {
                for (int x = 0; x < map[y].Length; x++) {
                    bool lit = map[y][x];

                    if (lit) {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write('#');
                    } else {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write('.');
                    }
                }
                Console.ResetColor();
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
