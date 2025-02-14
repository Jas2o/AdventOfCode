namespace AoC.Day
{
    public class Day11
    {
        public static void Run(string file) {
            Console.WriteLine("Day 11: Space Police" + Environment.NewLine);

			string input = File.ReadAllText(file);
            if (input.IndexOf(',') == -1) {
                Console.WriteLine("There is no test for this day.");
                return;
            }
            long[] initial = Array.ConvertAll(input.Split(','), long.Parse);

            Dictionary<(int, int), int> panelsA = new Dictionary<(int, int), int>();
            RunRobot(panelsA, initial);
            int partA = panelsA.Keys.Count();

            Dictionary<(int, int), int> panelsB = new Dictionary<(int, int), int>();
            panelsB[(0, 0)] = 1;
            RunRobot(panelsB, initial);
            DrawMap(panelsB);
            //int partB_check = panelsB.Keys.Count();

            Console.WriteLine("Part 1: " + partA);
            //Answer: 2343
            Console.WriteLine("Part 2: (you'll need to use your eyes to read the display above)");
            //Answer: JFBERBUH
        }

        private enum Direction {
            None, Up, Right, Down, Left, Imaginary
        }

        private static void RunRobot(Dictionary<(int, int), int> dPanels, long[] initial) {
            int x = 0;
            int y = 0;
            Direction dir = Direction.Up;

            int first = (dPanels.ContainsKey((x, y)) ? 1 : 0);
            Computer computerA = new Computer(initial, [first]);
            while (!computerA.Halted) {
                computerA.Run();

                long paint = computerA.outputQueue.Dequeue();
                long turn = computerA.outputQueue.Dequeue();

                dPanels[(x, y)] = (int)paint;

                if (turn == 0)
                    dir--;
                else if (turn == 1)
                    dir++;
                else
                    throw new Exception();

                if (dir == Direction.None)
                    dir = Direction.Left;
                else if (dir == Direction.Imaginary)
                    dir = Direction.Up;

                //Robot moves after turning
                switch (dir) {
                    case Direction.Up: y--; break;
                    case Direction.Right: x++; break;
                    case Direction.Down: y++; break;
                    case Direction.Left: x--; break;
                }

                if (dPanels.ContainsKey((x, y)))
                    computerA.inputQueue.Enqueue(dPanels[(x, y)]);
                else
                    computerA.inputQueue.Enqueue(0);
            }
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
                        if (c == 1)
                            Console.BackgroundColor = ConsoleColor.White;
                        Console.Write(' ');
                        Console.ResetColor();
                    } else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        //Copied from Day 9
        private class Computer {
            private Dictionary<long, long> memory;
            private long pos;
            private long relativeBase;
            private bool Waiting;
            public bool Halted;
            public bool Verbose;

            public Queue<long> inputQueue;
            public Queue<long> outputQueue;
            public long OutputLast;

            public Computer(long[] initial, long[] inputs) {
                memory = new Dictionary<long, long>();
                for (long i = 0; i < initial.Length; i++)
                    memory[i] = initial[i];
                pos = 0;
                relativeBase = 0;
                Halted = false;
                Waiting = false;
                inputQueue = new Queue<long>(inputs);
                outputQueue = new Queue<long>();
                OutputLast = -1;
                Verbose = false;
            }

            public Computer(long[] initial, Queue<long> inputs, Queue<long> outputs) {
                memory = new Dictionary<long, long>();
                for (long i = 0; i < initial.Length; i++)
                    memory[i] = initial[i];
                pos = 0;
                relativeBase = 0;
                Halted = false;
                Waiting = false;
                inputQueue = inputs;
                outputQueue = outputs;
                OutputLast = -1;
                Verbose = false;
            }

            public bool Run() {
                Waiting = false;
                while (!(Halted || Waiting)) {
                    Step();
                }
                return Halted;
            }

            private long MemoryRead(long address, int mode) {
                long v = memory[address];
                switch (mode) {
                    case 0:
                        //Position mode
                        memory.TryGetValue(v, out long pm);
                        return pm;
                    case 1:
                        //Immediate mode
                        return v;
                    case 2:
                        //Relative mode
                        v = relativeBase + v;
                        memory.TryGetValue(v, out long rm);
                        return rm;
                    default:
                        throw new Exception();
                }
            }

            private void MemorySet(long address, long value) {
                memory[address] = value;
            }

            public void Step() {
                long read = memory[pos];
                int opcode = (int)(read % 100);
                int mode1 = (int)(read % 1000 / 100);
                int mode2 = (int)(read % 10000 / 1000);
                int mode3 = (int)(read % 100000 / 10000);

                if (opcode == 99) {
                    Halted = true;
                    return;
                }

                long v1 = MemoryRead(pos + 1, mode1);
                long v2 = v2 = MemoryRead(pos + 2, mode2);

                switch (opcode) {
                    case 1: //Add
                        long addD = MemoryRead(pos + 3, 1);
                        if (mode3 == 2)
                            addD += relativeBase;
                        MemorySet(addD, v1 + v2);
                        pos += 4;
                        break;
                    case 2: //Multiply
                        long mulD = MemoryRead(pos + 3, 1);
                        if (mode3 == 2)
                            mulD += relativeBase;
                        MemorySet(mulD, v1 * v2);
                        pos += 4;
                        break;

                    case 3: //Input
                        bool canGetInput = inputQueue.TryDequeue(out long input);
                        if (!canGetInput) {
                            Waiting = true;
                            return;
                        }
                        long setD = MemoryRead(pos + 1, 1);
                        if (mode1 == 2)
                            setD += relativeBase;
                        MemorySet(setD, input);
                        pos += 2;
                        break;
                    case 4: //Output
                        outputQueue.Enqueue(v1);
                        OutputLast = v1;
                        if (Verbose) {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Out: {0}", v1);
                            Console.ResetColor();
                        }
                        pos += 2;
                        break;

                    case 5: //Jump-if-true
                        if (v1 != 0)
                            pos = v2;
                        else
                            pos += 3;
                        break;
                    case 6: //Jump-if-false
                        if (v1 == 0)
                            pos = v2;
                        else
                            pos += 3;
                        break;
                    case 7: //Less than
                        long lessD = MemoryRead(pos + 3, 1);
                        if (mode3 == 2)
                            lessD += relativeBase;
                        MemorySet(lessD, (v1 < v2 ? 1 : 0));
                        pos += 4;
                        break;
                    case 8: //Equals
                        long eqD = MemoryRead(pos + 3, 1);
                        if (mode3 == 2)
                            eqD += relativeBase;
                        MemorySet(eqD, (v1 == v2 ? 1 : 0));
                        pos += 4;
                        break;

                    case 9: //Adjusts the relative base
                        relativeBase += v1;
                        pos += 2;
                        break;

                    default:
                        throw new Exception();
                }
            }
        }
    }
}
