namespace AoC.Day {
    public class Day19
    {
        public static void Run(string file) {
            Console.WriteLine("Day 19: Tractor Beam" + Environment.NewLine);

            string input = File.ReadAllText(file);
            if (input.IndexOf(',') == -1) {
                Console.WriteLine("There is no test for this day.");
                return;
            }
            long[] initial = Array.ConvertAll(input.Split(','), long.Parse);

            //Work out how to reset the computer without building a new one.
            IntCode computer = new IntCode(initial, [20, 19]);
            computer.Run();
            List<int> listReset = new List<int>();
            for (int i = 0; i < initial.Length; i++) {
                if (computer.memory[i] != initial[i])
                    listReset.Add(i);
            }

            //Part 1
            Dictionary<(int, int), char> map = new Dictionary<(int, int), char>();
            int limitXY = 50;
            for (int y = 0; y < limitXY; y++) {
                for (int x = 0; x < limitXY; x++) {
                    computer.UnsafeReset(initial, listReset);
                    computer.inputQueue.Enqueue(x);
                    computer.inputQueue.Enqueue(y);
                    computer.Run();
                    long value = computer.outputQueue.Dequeue();
                    map[(x, y)] = (value == 0 ? '.' : '#');
                }
            }
            DrawMap(map, limitXY);
            int partA = map.Where(n => n.Value == '#').Count();

            //Part 2
            int partB = 0;
            int startX = 0;
            for (int y = limitXY; ; y++) {
                bool mode = false;
                int width = 0;
                int x = startX;
                //Find the rightmost in this row
                for (; ; x++) {
                    computer.UnsafeReset(initial, listReset);
                    computer.inputQueue.Enqueue(x);
                    computer.inputQueue.Enqueue(y);
                    computer.Run();
                    long value = computer.outputQueue.Dequeue();
                    if (value != 0) {
                        if (!mode) {
                            startX = x;
                            mode = true;
                        }
                        width++;
                    } else if (mode) {
                        x--;
                        break;
                    }
                }

                //Since we know top right, check bottom left and if true answer will be top left.
                int leftX = x - 99;
                int bottomY = y + 99;

                computer.UnsafeReset(initial, listReset);
                computer.inputQueue.Enqueue(leftX);
                computer.inputQueue.Enqueue(bottomY);
                computer.Run();
                long bottomleft = computer.outputQueue.Dequeue();

                if (bottomleft != 0) {
                    partB = (leftX * 10000) + y;
                    Console.WriteLine("Top left: {0},{1}", leftX, y);
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 154
            Console.WriteLine("Part 2: " + partB);
            //Answer: 9791328
        }

        private static void DrawMap(Dictionary<(int, int), char> nodes, int limitXY) {
            for (int y = 0; y < limitXY; y++) {
                for (int x = 0; x < limitXY; x++) {
                    char c = nodes[(x, y)];
                    Console.Write(c);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
