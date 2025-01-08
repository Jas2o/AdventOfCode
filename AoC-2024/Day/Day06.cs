namespace AoC.Day {
    public class Day06
    {
        private static bool verbose = false;
        private static bool useWait = false;
        private static int waitTime = 500;

        public static void Run(string file) {
            Console.WriteLine("Day 6: Guard Gallivant" + Environment.NewLine);

            // Build array that uses numbers as a sort of heatmap
            string[] all = File.ReadAllLines(file);
            int[][] orgArray = new int[all.Length][];
            int guardStartY = -1;
            int guardStartX = -1;

            for (int i = 0; i < all.Length; i++) {
                orgArray[i] = new int[all[i].Length];

                for(int j = 0; j < all[i].Length; j++) {
                    int current = all[i][j];
                    if (current == (int)'.') //Nothing
                        current = 0;
                    else if (current == (int)'#') //Obstacle
                        current = -9;
                    else if (current == (int)'^') { //Starting position
                        guardStartY = i;
                        guardStartX = j;
                        current = 1;
                    } else
                        Console.WriteLine("Help");

                    orgArray[i][j] = current;
                }
            }

            // Copy the array and check Part 1
            int[][] array = orgArray.Select(s => s.ToArray()).ToArray();
            bool doesItLoop = PathGuard(ref array, guardStartY, guardStartX);
            int uniquePositions = array.Length * array[0].Length;
            DrawMap(ref array);
            foreach (int[] line in array) {
                uniquePositions -= line.Count(x => x == 0); //Untouched
                uniquePositions -= line.Count(x => x == -9); //Obstacles
            }

            Console.WriteLine("Part 1: " + uniquePositions);
            //Answer: 5086

            if (verbose) {
                if(useWait) {
                    Thread.Sleep(waitTime);
                }
                else
                {
                    Console.WriteLine("Press ENTER to continue.");
                    Console.ReadLine();
                }
                Console.Clear();
                Console.WriteLine("\x1b[3J");
            }

            //Test putting an obstacle at every position to see if it results in a loop.
            int obstructionLoops = 0;
            for (int y = 0; y < array.Length; y++) {
                for (int x = 0; x < array[y].Length; x++) {
                    //Console.WriteLine("{0},{1}", y, x);

                    array = orgArray.Select(s => s.ToArray()).ToArray();
                    int current = array[y][x];
                    if (current != 0)
                        continue;
                    array[y][x] = -9;

                    bool doesItLoopAgain = PathGuard(ref array, guardStartY, guardStartX);
                    if (doesItLoopAgain) {
                        obstructionLoops++;

                        if(verbose) {
                            DrawMap(ref array);
                            if (useWait) {
                                Thread.Sleep(waitTime);
                            } else {
                                Console.WriteLine("Press ENTER to continue.");
                                Console.ReadLine();
                            }
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                        }
                    }
                }
            }

            Console.WriteLine("Part 2: " + obstructionLoops);
            //Answer: 1770
        }

        private static void DrawMap(ref int[][] array) {
            foreach (int[] line in array) {
                foreach (int n in line) {
                    if (n == -9) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("#");
                        Console.ResetColor();
                    } else if (n == 0)
                        Console.Write(" ");
                    else {
                        if (n > 4)
                            Console.ForegroundColor = ConsoleColor.Red;
                        else if (n > 1)
                            Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(n);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Console.WriteLine();
            }
        }

        private enum Direction {
            None, Up, Right, Down, Left, Imaginary, UpDown, LeftRight, Any
        }

        private static bool PathGuard(ref int[][] array, int guardStartY, int guardStartX) {
            Direction dir = Direction.Up;

            int guardY = guardStartY;
            int guardX = guardStartX;

            while (true) {
                int lookY = guardY;
                int lookX = guardX;
                switch (dir) {
                    case Direction.Up: lookY--; break;
                    case Direction.Right: lookX++; break;
                    case Direction.Down: lookY++; break;
                    case Direction.Left: lookX--; break;
                    case Direction.Imaginary:
                        //We are looking in a direction that only exists within out mind.
                        dir = Direction.Up;
                        continue;
                }

                if (lookY < 0 || lookY >= array.Length || lookX < 0 || lookX >= array[lookY].Length) {
                    //We have left the mapped area
                    array[guardY][guardX] = 5;
                    return false;
                }

                int look = array[lookY][lookX];
                if(look > 8) {
                    //We are looking at a super hot spot, it's a loop!
                    //With the input I received, checking greater than 3 was enough.
                    //Made is greater than 8 just to be sure.
                    return true;
                } else if (look == -9)
                    //We are looking at an obstacle, better turn right.
                    dir++;
                else {
                    if (array[guardY][guardX] >= 0) {
                        //Increase this spot's heat
                        array[guardY][guardX]++;
                    }

                    guardY = lookY;
                    guardX = lookX;
                }
            }

            //return false;
        }
    }

}
