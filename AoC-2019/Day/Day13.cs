namespace AoC.Day {
    public class Day13
    {
        public static void Run(string file, bool interactive) {
            Console.WriteLine("Day 13: Care Package" + Environment.NewLine);

            string input = File.ReadAllText(file);
            if (input.IndexOf(',') == -1) {
                Console.WriteLine("There is no test for this day.");
                return;
            }
            long[] initial = Array.ConvertAll(input.Split(','), long.Parse);

            int partA = 0, partB = 0;
            Dictionary<(int, int), int> screen = new Dictionary<(int, int), int>();

            //Part 1
            IntCode computerA = new IntCode(initial, []);
            if (!computerA.Run()) {
                Console.WriteLine("A - Did not complete");
                return;
            } else {
                while (computerA.outputQueue.Count > 2) {
                    int x = (int)computerA.outputQueue.Dequeue();
                    int y = (int)computerA.outputQueue.Dequeue();
                    int tileId = (int)computerA.outputQueue.Dequeue();
                    screen[(x, y)] = tileId;
                }
                partA = screen.Values.Where(t => t == 2).Count();
            }

            //Part 2
            int cheatMode = 0;
            if (interactive) {
                Console.Beep(150, 100);
                Console.Beep(200, 100);

                Console.WriteLine("Part 1: " + partA);
                Console.WriteLine("\r\nPress ENTER to continue with the interactive experience for Part 2.");
                Console.WriteLine("(at the start press C to enable cheat mode)");
                Console.ReadLine();

                screen.Clear();
            } else
                cheatMode = 1;

            IntCode computerB = new IntCode(initial, []);
            computerB.memory[0] = 2;
            while (!computerB.Halted) {
                bool complete = computerB.Run();
                while (computerB.outputQueue.Count > 2) {
                    int x = (int)computerB.outputQueue.Dequeue();
                    int y = (int)computerB.outputQueue.Dequeue();
                    int tileId = (int)computerB.outputQueue.Dequeue();
                    if (x == -1 && y == 0)
                        partB = tileId;
                    else
                        screen[(x, y)] = tileId;
                }

                if (interactive) {
                    if(cheatMode == 2)
                        Thread.Sleep(20);
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    DrawMap(screen);
                    Console.WriteLine(" Score: " + partB);

                    //(int, int) ball = screen.First(t => t.Value == 4).Key;
                    //(int, int) paddle = screen.FirstOrDefault(t => t.Value == 3).Key;
                    //Console.WriteLine("  Ball: {0},{1}", ball.Item1, ball.Item2);
                    //Console.WriteLine("Paddle: {0}", paddle.Item1);
                }

                if (cheatMode == 1) {
                    bool cheat = ActivateCheat(computerB);
                    if (cheat)
                        cheatMode = 2;
                }
                if (cheatMode == 2) {
                    computerB.inputQueue.Enqueue(0);
                    continue;
                }

                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.C) {
                    bool cheat = ActivateCheat(computerB);
                    if(cheat) {
                        cheatMode = 2;
                        if (interactive) {
                            Console.WriteLine("\bCheat mode activated, will run to the end shortly...");
                            Thread.Sleep(1000);
                        }
                        continue;
                    } else {
                        Console.WriteLine("\bCouldn't find paddle in memory");
                        key = Console.ReadKey();
                    }
                }

                if (key.Key == ConsoleKey.LeftArrow)
                    computerB.inputQueue.Enqueue(-1);
                else if (key.Key == ConsoleKey.RightArrow)
                    computerB.inputQueue.Enqueue(1);
                else
                    computerB.inputQueue.Enqueue(0);
            }

            if(interactive)
                Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 361
            Console.WriteLine("Part 2: " + partB);
            //Answer: 17590

            Console.WriteLine("\r\nYou can run the interactive version by including letter 'i' in the command.");
        }

        private static bool ActivateCheat(IntCode computer) {
            List<KeyValuePair<long, long>> paddles = computer.memory.Where(m => m.Value == 3).ToList();
            int memPaddle = -1;
            foreach (KeyValuePair<long, long> p in paddles) {
                if (computer.memory[p.Key - 1] == 0 && computer.memory[p.Key + 1] == 0) {
                    memPaddle = (int)p.Key;
                    break;
                }
            }
            if (memPaddle != -1) {
                computer.memory[memPaddle] = 1;
                for (int i = 1; ; i++) {
                    int left = (int)computer.memory[memPaddle - i];
                    int right = (int)computer.memory[memPaddle + i];
                    if (left == 0)
                        computer.memory[memPaddle - i] = 1;
                    if (right == 0)
                        computer.memory[memPaddle + i] = 1;
                    if (left != 0 || right != 0)
                        break;
                }
                return true;
            }
            return false;
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
                        if (c == 0)
                            Console.Write(' ');
                        else if (c == 1) {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Write(' '); //#
                        } else if (c == 2)
                            Console.Write('B');
                        else if (c == 3)
                            Console.Write('-');
                        else if (c == 4) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write('o');
                        } else
                            Console.Write(c);
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
