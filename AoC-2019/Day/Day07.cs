using AoC.Shared;

namespace AoC.Day
{
    public class Day07
    {
        public static void Run(string file) {
            Console.WriteLine("Day 7: Amplification Circuit" + Environment.NewLine);

            string input = File.ReadAllText(file);
            int[] initial = Array.ConvertAll(input.Split(','), int.Parse);

            int partA = -1;
            string partA_seq = string.Empty;
            int[] baseA = [0, 1, 2, 3, 4];
            List<IList<int>> permsA = Permutations.Get(baseA);
            foreach (IList<int> perm in permsA) {
                int nextInput = 0;
                for (int s = 0; s < baseA.Length; s++) {
                    Amplifier amp = new Amplifier(initial, [perm[s], nextInput]);
                    bool completed = amp.Run();
                    if (completed)
                        nextInput = amp.OutputLast;
                    else {
                        Console.WriteLine("A - Did not complete\r\n");
                        break;
                    }
                }

                if (nextInput == 0)
                    break;
                if (nextInput > partA) {
                    partA = nextInput;
                    partA_seq = string.Join(',', perm);
                }
            }

            int partB = -1;
            string partB_seq = string.Empty;
            int[] baseB = [5, 6, 7, 8, 9];
            List<IList<int>> permsB = Permutations.Get(baseB);
            foreach (IList<int> perm in permsB) {
                Queue<int> queue0 = new Queue<int>([perm[0], 0]);
                Queue<int> queue1 = new Queue<int>([perm[1]]);
                Queue<int> queue2 = new Queue<int>([perm[2]]);
                Queue<int> queue3 = new Queue<int>([perm[3]]);
                Queue<int> queue4 = new Queue<int>([perm[4]]);

                Amplifier amp0 = new Amplifier(initial, queue0, queue1);
                Amplifier amp1 = new Amplifier(initial, queue1, queue2);
                Amplifier amp2 = new Amplifier(initial, queue2, queue3);
                Amplifier amp3 = new Amplifier(initial, queue3, queue4);
                Amplifier amp4 = new Amplifier(initial, queue4, queue0);

                while(!(amp0.Halted && amp1.Halted && amp2.Halted && amp3.Halted && amp4.Halted)) {
                    amp0.Run();
                    amp1.Run();
                    amp2.Run();
                    amp3.Run();
                    amp4.Run();
                }

                if (amp4.OutputLast > partB) {
                    partB = amp4.OutputLast;
                    partB_seq = string.Join(',', perm);
                }
            }

            Console.WriteLine("Part 1: {0} ({1})", partA, partA_seq);
            //Answer: 30940 (3,0,4,2,1)
            Console.WriteLine("Part 2: {0} ({1})", partB, partB_seq);
            //Answer: 76211147 (8,9,6,7,5)
        }


        private class Amplifier {
            private int[] intcode;
            private int pos;
            private bool Waiting;
            public bool Halted;

            public Queue<int> inputQueue;
            public Queue<int> outputQueue;
            public int OutputLast;

            public Amplifier(int[] initial, int[] inputs) {
                intcode = new int[initial.Length];
                Array.Copy(initial, intcode, initial.Length);
                pos = 0;
                Halted = false;
                Waiting = false;
                inputQueue = new Queue<int>(inputs);
                outputQueue = new Queue<int>();
                OutputLast = -1;
            }

            public Amplifier(int[] initial, Queue<int> inputs, Queue<int> outputs) {
                intcode = new int[initial.Length];
                Array.Copy(initial, intcode, initial.Length);
                pos = 0;
                Halted = false;
                Waiting = false;
                inputQueue = inputs;
                outputQueue = outputs;
                OutputLast = -1;
            }

            public bool Run() {
                Waiting = false;
                while (!(Halted || Waiting)) {
                    Step();
                }
                return Halted;
            }

            public void Step() {
                int read = intcode[pos];
                int opcode = read % 100;
                int mode1 = read % 1000 / 100;
                int mode2 = read % 10000 / 1000;
                int mode3 = read % 100000 / 10000;

                if (opcode == 99) {
                    Halted = true;
                    return;
                }

                int v1 = (mode1 == 1 ? intcode[pos + 1] : intcode[intcode[pos + 1]]);
                int v2 = 0;
                if (opcode != 3 && opcode != 4)
                    v2 = (mode2 == 1 ? intcode[pos + 2] : intcode[intcode[pos + 2]]);
                //int v3 = (mode3 == 1 ? intcode[pos + 3] : intcode[intcode[pos + 3]]);

                switch (opcode) {
                    case 1: //Add
                        int addD = intcode[pos + 3];
                        intcode[addD] = v1 + v2;
                        pos += 4;
                        break;
                    case 2: //Multiply
                        int mulD = intcode[pos + 3];
                        intcode[mulD] = v1 * v2;
                        pos += 4;
                        break;

                    case 3: //Input
                        bool canGetInput = inputQueue.TryDequeue(out int input);
                        if(!canGetInput) {
                            Waiting = true;
                            return;
                        }
                        //int input = inputQueue.Dequeue();
                        //Console.ForegroundColor = ConsoleColor.Cyan;
                        //Console.WriteLine("In: {0}", input);
                        //Console.ResetColor();
                        int setD = intcode[pos + 1];
                        intcode[setD] = input;
                        pos += 2;
                        break;
                    case 4: //Output
                        outputQueue.Enqueue(v1);
                        OutputLast = v1;
                        //Console.ForegroundColor = ConsoleColor.Green;
                        //Console.WriteLine("Out: {0}", v1);
                        //Console.ResetColor();
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
                        int lessD = intcode[pos + 3];
                        intcode[lessD] = (v1 < v2 ? 1 : 0);
                        pos += 4;
                        break;
                    case 8: //Equals
                        int eqD = intcode[pos + 3];
                        intcode[eqD] = (v1 == v2 ? 1 : 0);
                        pos += 4;
                        break;

                    default:
                        throw new Exception();
                }
            }
        }
    }
}
