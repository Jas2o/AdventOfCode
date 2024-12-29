using System;
using System.Globalization;
using System.Text;

namespace AoC.Day
{
    public class Day10 {
        public static void Run(string file) {
            Console.WriteLine("Day 10: Hoof It" + Environment.NewLine);

            List<int[]> listZero = new List<int[]>();
            List<int[]> listNine = new List<int[]>();

            //Build map
            string[] all = File.ReadAllLines(file);
            int[][] map = new int[all.Length][];
            int height = all.Length;
            int width = all[0].Length;

            bool verbose = (height < 10);

            for (int y = 0; y < height; y++) {
                map[y] = new int[width];

                for (int x = 0; x < width; x++) {
                    int num = (int)Char.GetNumericValue(all[y][x]);
                    map[y][x] = num;

                    if (num == 0)
                        listZero.Add([y, x]);
                    else if (num == 9)
                        listNine.Add([y, x]);
                }
            }

            //From each the zeros, check every direction.
            int total = 0;
            int totalrating = 0;
            foreach (int[] zero in listZero) {
                if(verbose)
                    Console.WriteLine("{0},{1}", zero[1], zero[0]);

                int[][] visited = BlankVisitedMap(height, width);
                //Recursive direction check updating visited counts
                TryPath(ref map, ref visited, zero, -1);
                if (verbose)
                    DrawMap(ref map, ref visited, zero);

                //Look at counts of the nines to determine the scores
                int sub = 0;
                int subrating = 0;
                foreach (int[] yx in listNine) {
                    if (visited[yx[0]][yx[1]] > 0) {
                        sub++;
                        subrating += visited[yx[0]][yx[1]];
                    }
                }
                total += sub;
                totalrating += subrating;

                if (verbose) {
                    Console.WriteLine(sub + " and rating of " + subrating);
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Part 1: " + total);
            //Answer: 557
            Console.WriteLine("Part 2: " + totalrating);
            //Answer: 1062
        }

        private static int[][] BlankVisitedMap(int height, int width) {
            int[][] map = new int[height][];
            for (int y = 0; y < height; y++) {
                map[y] = new int[width];
            }
            return map;
        }

        private static void DrawMap(ref int[][] map, ref int[][] visited, int[] yx) {
            for (int y = 0; y < map.Length; y++) {
                for (int x = 0; x < map[y].Length; x++) {
                    int c = map[y][x];
                    bool v = visited[y][x] > 0;
                    if (v) {
                        if (c == 9)
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        else
                            Console.ForegroundColor = ConsoleColor.Green;
                    } else
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    if (y == yx[0] && x == yx[1])
                        Console.ForegroundColor = ConsoleColor.Red;
                    
                    Console.Write(c);
                    Console.ResetColor();

                    //For Part 2
                    int heat = visited[y][x];
                    Console.Write(" " + heat + "\t");
                }
                Console.WriteLine();
            }
        }

        private static void TryPath(ref int[][] map, ref int[][] visited, int[] yx, int previous) {
            if (yx[0] < 0 || yx[0] >= map.Length || yx[1] < 0 || yx[1] >= map[0].Length)
                return;

            int current = map[yx[0]][yx[1]];
            int diff = current - previous;
            if (diff != 1)
                return;
            visited[yx[0]][yx[1]]++;

            //Up
            TryPath(ref map, ref visited, [yx[0]-1, yx[1]], current);
            //Right
            TryPath(ref map, ref visited, [yx[0], yx[1]+1], current);
            //Down
            TryPath(ref map, ref visited, [yx[0] + 1, yx[1]], current);
            //Left
            TryPath(ref map, ref visited, [yx[0], yx[1]-1], current);
        }
    }
}
