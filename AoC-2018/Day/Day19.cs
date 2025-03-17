namespace AoC.Day
{
    public class Day19
    {
        // Yet another math problem I could not solve on my own.

        public static void Run(string file) {
            Console.WriteLine("Day 19: Go With The Flow" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            Computer computer = new Computer(lines);
            if (lines.Length < 10) {
                computer.Verbose = true;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Part 1:");
                Console.ResetColor();
            }
            computer.Run(0);
            int partA = computer.Register[0];

            int partB = 0;
            if (lines.Length > 9)
                computer.Verbose = true;
            if (computer.Verbose) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Part 2:");
                Console.ResetColor();
            }
            computer.Run(1);
            partB = computer.Register[0];

            Console.WriteLine("Part 1: " + partA);
            //Answer: 1056
            Console.WriteLine("Part 2: " + partB);
            //Answer: 10915260
        }

        //Based on Day 16
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

            public void Run(int reg0) {
                Register = new int[6];
                Register[0] = reg0;

                int cheat = Ops.FindLastIndex(o => o.Instruction == "addr") + 1;

                Dictionary<int, int> attempts = new Dictionary<int, int>();
                for (int i = 0; i < Ops.Count; i++)
                    attempts[i] = 0;

                string before = string.Empty;
                string after = string.Empty;
                while (true) {
                    int ip = Register[IpReg];
                    if (ip >= Ops.Count)
                        break;
                    attempts[ip]++;

                    if (reg0 != 0 && ip == cheat) { //34
                        //The first time we reach this, the target is in a register.
                        int target = Register.Max();

                        //The rest of the program would find the sum of target's factors, slowly.
                        //We'll just find it now and end the program.
                        List<int> factors = new List<int>() { 1, target };

                        int n = target;
                        while (n % 2 == 0) {
                            factors.Add(2);
                            n /= 2;
                        }
                        for (int i = 3; i <= Math.Sqrt(n); i += 2) {
                            while (n % i == 0) {
                                factors.Add(i);
                                n /= i;
                            }
                        }
                        if (n > 2)
                            factors.Add(n);

                        factors.Sort();
                        Register[0] = factors.Sum();

                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("Assuming this is the right input and shortcut...");
                        Console.WriteLine("{0} = {1}", string.Join(" + ", factors), Register[0]);
                        Console.ResetColor();
                        break;
                    }

                    /* //A dumber way to find it.
                    if (Register.Any(r => r > 1000000)) {
                        int[] temp = Register.OrderDescending().ToArray();
                        int target = temp[0] + temp[1];
                    }
                    */

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
                        case "eqrr": op_eqrr(op.Codes); break;
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
