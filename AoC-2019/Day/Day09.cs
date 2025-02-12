namespace AoC.Day {
    public class Day09
    {
        public static void Run(string file) {
            Console.WriteLine("Day 9: Sensor Boost" + Environment.NewLine);

            string input = File.ReadAllText(file);
            long[] initial = Array.ConvertAll(input.Split(','), long.Parse);

            Computer computerA = new Computer(initial, [1]);
            if (!computerA.Run())
                Console.WriteLine("A - Did not complete");
            long partA = computerA.OutputLast;

            Computer computerB = new Computer(initial, [2]);
            if (!computerB.Run())
                Console.WriteLine("B - Did not complete");
            long partB = computerB.OutputLast;

            Console.WriteLine("Part 1: " + partA);
            //Answer: 3989758265
            Console.WriteLine("Part 2: " + partB);
            //Answer: 76791
        }

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
                for(long i = 0; i < initial.Length; i++)
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
