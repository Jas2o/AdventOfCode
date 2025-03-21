namespace AoC.Day
{
    public class Day21
    {
        //Unfortuntely used the method where the input program is not understood.
        //Part 2 takes a long time to resolve (within 1 min on my PC).

        public static void Run(string file) {
            string title = "Day 21: Chronal Conversion" + Environment.NewLine;
            Console.WriteLine(title);

			string[] lines = File.ReadAllLines(file);
            if (lines.Length < 5) {
                Console.WriteLine("There is no test for this day.");
                return;
            }

            Console.WriteLine("This can take a while to run, there will be a beep when done.");

            Computer computer = new Computer(lines);
            int max = int.MaxValue - 1;

            int partA = 0;
            int partB = 0;
            for (int i = 0; i < max; i++) {
                if(partA == 0) {
                    int resultA = computer.Run(partA, false);
                    if (resultA > 0) {
                        partA = resultA;
                        Console.WriteLine("\r\nPart 1: " + partA);
                    }
                }

                if (partB == 0) {
                    int resultB = computer.Run(partB, true);
                    if (resultB > 0) {
                        partB = resultB;
                        Console.WriteLine("\r\nPart 2: " + partB);
                        break;
                    }
                }

                if (partA != 0 && partB != 0)
                    break;
            }

            Console.Beep();

            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine("Part 1: " + partA);
            //Answer: 6483199
            Console.WriteLine("Part 2: " + partB);
            //Answer: 13338900
        }

        //Based on Day 19 (which was based on Day 16)
        //Change made to Run after eqrr, will pull answer from there.
        private class Computer {
            public int IpReg;
            public int[] Register;
            public List<Op> Ops;
            public bool Verbose;

            public Computer(string[] lines) {
                Register = new int[6];
                Ops = new List<Op>();
                Verbose = false;

                foreach (string line in lines) {
                    string[] fields = line.Split(' ');
                    if (line.StartsWith('#')) {
                        IpReg = int.Parse(fields[1]);
                        continue;
                    }

                    Ops.Add(new Op(fields));
                }
            }

            public int Run(int reg0, bool isPartB) {
                Register = new int[6];
                Register[0] = reg0;

                Dictionary<int, bool> Seen = new Dictionary<int, bool>();

                string before = string.Empty;
                string after = string.Empty;
                while (true) {
                    int ip = Register[IpReg];
                    if (ip >= Ops.Count)
                        break;

                    if (Verbose)
                        before = string.Join(", ", Register);

                    Op op = Ops[ip];
                    switch (op.Instruction) {
                        case "addr": op_addr(op.Codes); break;
                        case "addi": op_addi(op.Codes); break;
                        case "mulr": op_mulr(op.Codes); break;
                        case "muli": op_muli(op.Codes); break;
                        case "banr": op_banr(op.Codes); break;
                        case "bani": op_bani(op.Codes); break;
                        case "borr": op_borr(op.Codes); break;
                        case "bori": op_bori(op.Codes); break;
                        case "setr": op_setr(op.Codes); break;
                        case "seti": op_seti(op.Codes); break;
                        case "gtir": op_gtir(op.Codes); break;
                        case "gtri": op_gtri(op.Codes); break;
                        case "gtrr": op_gtrr(op.Codes); break;
                        case "eqir": op_eqir(op.Codes); break;
                        case "eqri": op_eqri(op.Codes); break;
                        case "eqrr":
                            op_eqrr(op.Codes);
                            int comparingTo = Register[op.Codes[1]];

                            if (isPartB) {
                                bool added = Seen.TryAdd(comparingTo, true);
                                if (!added) {
                                    Seen.Remove(comparingTo);
                                    int lastUnique = Seen.Last().Key;
                                    return lastUnique;
                                }
                            } else
                                return comparingTo;
                            break;
                        default:
                            throw new Exception();
                    }

                    if (Verbose) {
                        after = string.Join(", ", Register);
                        Console.WriteLine("ip={0} [{1}] {2} [{3}]", ip, before, op.ToString(), after);
                    }

                    Register[IpReg]++;
                }

                if (Verbose)
                    Console.WriteLine();

                return 0;
            }

            public class Op {
                public string Instruction;
                public int[] Codes;

                public Op(string[] fields) {
                    Instruction = fields[0];
                    Codes = new int[] { 0, int.Parse(fields[1]), int.Parse(fields[2]), int.Parse(fields[3]) };
                    //The first is kept as 0 for compatibility with Day 16.
                }

                public override string ToString() {
                    return string.Format("{0} {1}", Instruction, string.Join(' ', Codes.Skip(1)));
                }
            }

            //Addition
            public void op_addr(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];
                Register[c] = Register[a] + Register[b];
            }

            public void op_addi(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];
                Register[c] = Register[a] + b;
            }

            //Multiplication
            public void op_mulr(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];
                Register[c] = Register[a] * Register[b];
            }

            public void op_muli(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];
                Register[c] = Register[a] * b;
            }

            //Bitwise AND
            public void op_banr(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];
                Register[c] = Register[a] & Register[b];
            }

            public void op_bani(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];
                Register[c] = Register[a] & b;
            }

            //Bitwise OR
            public void op_borr(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];
                Register[c] = Register[a] | Register[b];
            }

            public void op_bori(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];
                Register[c] = Register[a] | b;
            }

            //Assignment
            public void op_setr(int[] op) {
                int a = op[1];
                //int b = op[2];
                int c = op[3];
                Register[c] = Register[a];
            }

            public void op_seti(int[] op) {
                int a = op[1];
                //int b = op[2];
                int c = op[3];
                Register[c] = a;
            }

            //Greater-than testing
            public void op_gtir(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];
                if (a > Register[b])
                    Register[c] = 1;
                else
                    Register[c] = 0;
            }

            public void op_gtri(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];
                if (Register[a] > b)
                    Register[c] = 1;
                else
                    Register[c] = 0;
            }

            public void op_gtrr(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];
                if (Register[a] > Register[b])
                    Register[c] = 1;
                else
                    Register[c] = 0;
            }

            //Equality testing
            public void op_eqir(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];
                if (a == Register[b])
                    Register[c] = 1;
                else
                    Register[c] = 0;
            }

            public void op_eqri(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];
                if (Register[a] == b)
                    Register[c] = 1;
                else
                    Register[c] = 0;
            }

            public void op_eqrr(int[] op) {
                int a = op[1];
                int b = op[2];
                int c = op[3];

                if (Register[a] == Register[b])
                    Register[c] = 1;
                else
                    Register[c] = 0;
            }
        }
    }
}
