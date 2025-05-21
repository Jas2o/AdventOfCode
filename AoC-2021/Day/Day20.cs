using System.Collections;

namespace AoC.Day
{
    public class Day20
    {
        public static void Run(string file) {
            Console.WriteLine("Day 20: Trench Map" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            string iea = lines[0];
            string[] input = lines.Skip(2).ToArray();
            HashSet<(int x, int y)> pixels = new HashSet<(int, int)>();
            for(int y = 0; y < input.Length; y++) {
                for (int x = 0; x < input[y].Length; x++) {
                    if (input[y][x] == '#')
                        pixels.Add((x, y));
                }
            }

            Draw(pixels);
            Enhance(iea, ref pixels, 2, true);
            int partA = pixels.Count();

            Enhance(iea, ref pixels, 48, false);
            int partB = pixels.Count();
            Draw(pixels);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 5218
            Console.WriteLine("Part 2: " + partB);
            //Answer: 15527
        }

        private static void Enhance(string iea, ref HashSet<(int x, int y)> pixels, int numTimes, bool drawDo) {
            int minX = pixels.Min(p => p.x) - numTimes - 1;
            int maxX = pixels.Max(p => p.x) + numTimes + 1;
            int minY = pixels.Min(p => p.y) - numTimes - 1;
            int maxY = pixels.Max(p => p.y) + numTimes + 1;

            int[][] matrix = [
                [ -1,-1 ], [ 0,-1 ], [ 1,-1 ],
                [ -1, 0 ], [ 0, 0 ], [ 1, 0 ],
                [ -1, 1 ], [ 0, 1 ], [ 1, 1 ],
            ];

            bool flipDo = (iea[0] == '#');
            bool flip = false;
            for (int times = 0; times < numTimes; times++) {
                HashSet<(int x, int y)> pixelsBuffer = new HashSet<(int, int)>();

                for (int y = minY; y <= maxY; y++) {
                    for (int x = minX; x <= maxX; x++) {
                        BitArray consider = new BitArray(9);
                        for (int i = 0; i < 9; i++) {
                            int[] m = matrix[i];
                            if (pixels.Contains((x + m[0], y + m[1])))
                                consider[8 - i] = true;
                            else if (flip) {
                                if (x <= minX || x >= maxX || y <= minY || y >= maxY)
                                    consider[8 - i] = true;
                            }
                        }
                        int considerNum = Shared.Number.GetIntFromBitArray(consider);
                        char lookup = iea[considerNum];
                        if (lookup == '#')
                            pixelsBuffer.Add((x, y));
                    }
                }
                pixels = pixelsBuffer;
                if (!flipDo || flip) {
                    if (drawDo)
                        Draw(pixels);
                }
                if (flipDo)
                    flip = !flip;
            }
        }

        private static void Draw(HashSet<(int x, int y)> pixels) {
            int minX = pixels.Min(p => p.x) - 1;
            int maxX = pixels.Max(p => p.x) + 1;
            int minY = pixels.Min(p => p.y) - 1;
            int maxY = pixels.Max(p => p.y) + 1;

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    if (pixels.Contains((x, y)))
                        Console.Write('#');
                    else
                        Console.Write('.');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
