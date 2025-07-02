namespace AoC.Day
{
    public class Day24
    {
        public static void Run(string file) {
            Console.WriteLine("Day 24: Arithmetic Logic Unit" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            Computer computer = new Computer(lines);
            if (computer.Program.Count != 252) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Unable to use shortcut with this input.\r\n");
                Console.ResetColor();
                computer.RunTest();
                return;
            }

            Shortcut shortcut = new Shortcut(computer.Program);

            int block;
            Dictionary<(int, int, long), long> cache = new Dictionary<(int, int, long), long>();
            //Block (0-13), Input (1-9), InputZ = OutputZ

            //For each block, work out upper limits for previous Z values.
            int depth = 1;
            Dictionary<int, long> maxes = new Dictionary<int, long> { { 0, 0 } };
            for (block = 1; block < 14; block++) {
                maxes.Add(block, (long)Math.Pow(26, depth)); //For my input could be as low as 24
                if (shortcut.divz[block] == 1)
                    depth++;
                else
                    depth--;
            }

            //Go through each block in reverse to get reduced results.
            long min = 0;
            HashSet<long> needed = new HashSet<long>() { 0 };
            for (block = 13; block >= 0; block--) {
                long max = maxes[block];

                for (long z = min; z <= max; z++) {
                    for (int input = 1; input <= 9; input++) {
                        long result = shortcut.Run(input, z, block);
                        if (needed.Contains(result))
                            cache.Add((block, input, z), result);
                    }
                }

                IEnumerable<KeyValuePair<(int, int, long), long>> relevant = cache.Where(t => t.Key.Item1 == block);
                long iMin = relevant.Min(t => t.Key.Item3);
                long iMax = relevant.Max(t => t.Key.Item3);
                long oMin = relevant.Min(t => t.Value);
                long oMax = relevant.Max(t => t.Value);
                Console.WriteLine("{0,2} = {1,2} in:{2}-{3,8} (try up to {4,8}) out:{5,2}-{6,8}", block, shortcut.divz[block], iMin, iMax, max, oMin, oMax);

                needed.Clear();
                IEnumerable<long> inputZ = relevant.Select(t => t.Key.Item3);
                foreach (int z in inputZ)
                    needed.Add(z);
            }

            //Go forward through the results to find anwsers.
            long partA = shortcut.SolveRecursive(cache, 0, 0, new int[14], false);
            long partB = shortcut.SolveRecursive(cache, 0, 0, new int[14], true);

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 39999698799429
            Console.WriteLine("Part 2: " + partB);
            //Answer: 18116121134117
        }

        private class Shortcut {
            public Dictionary<int, int> divz = new Dictionary<int, int>();
            public Dictionary<int, int> addx = new Dictionary<int, int>();
            public Dictionary<int, int> addy = new Dictionary<int, int>();

            public Shortcut(List<Computer.ProgramInstruction> program) {
                for (int block = 0; block < 14; block++) {
                    int offset = block * 18;
                    divz[block] = program[offset + 4].NonVarB;
                    addx[block] = program[offset + 5].NonVarB;
                    addy[block] = program[offset + 15].NonVarB;
                }
            }

            public long SolveRecursive(Dictionary<(int, int, long), long> cache, int block, long prevZ, int[] digits, bool isPart2) {
                //Block (0-13), Input (1-9), InputZ = OutputZ

                if (block == 14) {
                    if (prevZ == 0) {
                        long result = long.Parse(string.Join("", digits));
                        return result;
                    }
                    return -1;
                }

                IEnumerable<KeyValuePair<(int, int, long), long>> relevant = cache.Where(t => t.Key.Item1 == block && t.Key.Item3 == prevZ);
                if(!isPart2)
                    relevant = relevant.OrderByDescending(t => t.Key.Item2);
                else
                    relevant = relevant.OrderBy(t => t.Key.Item2);

                foreach (var r in relevant) {
                    digits[block] = r.Key.Item2;
                    long newZ = Run(r.Key.Item2, prevZ, block);
                    long result = SolveRecursive(cache, block + 1, newZ, digits, isPart2);
                    if (result > -1)
                        return result;
                }

                return -1;
            }

            public long Run(int inputW, long z, int block) {
                //inp w
                //mul x 0  => x = 0;
                //add x z  => x = z;
                //mod x 26 => x %= 26;
                long x = z % 26;

                //div z 1 or 26
                z /= divz[block];

                //add x 14 => x += addx;
                //eql x w  => x = (x == inputW ? 1 : 0);
                //eql x 0  => x = (x == 0 ? 1 : 0);
                x = (x + addx[block] == inputW ? 0 : 1);

                //mul y 0  => y = 0;
                //add y 25 => y = 25;
                //mul y x  => y *= x;
                //add y 1  => y++;
                long y = (25 * x) + 1;

                //mul z y
                z *= y;

                //mul y 0  => y = 0;
                //add y w  => y = inputW;
                //add y 12 => y = inputW + addy;
                //mul y x  => y *= x; 
                y = (inputW + addy[block]) * x;

                //add z y
                z += y;
                return z;
            }
        }

        private class Computer {
            public int VarW;
            public int VarX;
            public int VarY;
            public int VarZ;

            public List<ProgramInstruction> Program;

            public Computer(string[] program) {
                VarW = 0;
                VarX = 0;
                VarY = 0;
                VarZ = 0;

                Program = new List<ProgramInstruction>();
                foreach(string line in program) {
                    if (line == "Test")
                        break;
                    string[] fields = line.Split(' ', 3);
                    ProgramInstruction inst = new ProgramInstruction(fields);
                    Program.Add(inst);
                }
            }

            public void Reset() {
                VarW = 0;
                VarX = 0;
                VarY = 0;
                VarZ = 0;
            }

            public void RunTest() {
                foreach (ProgramInstruction inst in Program) {
                    switch (inst.Type) {
                        case Instruction.inp:
                            int num = 0;
                            bool havenum = false;
                            while(!havenum) {
                                Console.Write("Input a number: ");
                                string txt = Console.ReadLine();
                                havenum = int.TryParse(txt, out num);
                            }
                            inp(inst, num);
                            break;
                        case Instruction.add: add(inst); break;
                        case Instruction.mul: mul(inst); break;
                        case Instruction.div: div(inst); break;
                        case Instruction.mod: mod(inst); break;
                        case Instruction.eql: eql(inst); break;

                        default:
                            throw new Exception("Invalid instruction");
                    }
                }

                Console.WriteLine("Test completed, variables are:");
                Console.WriteLine("w: " + VarW);
                Console.WriteLine("x: " + VarX);
                Console.WriteLine("y: " + VarY);
                Console.WriteLine("z: " + VarZ);
            }

            public void RunBlock(int startmulti, int input) {
                int start = 18 * startmulti;
                int end = start + 18;

                for (int i = start; i < end; i++) {
                    ProgramInstruction inst = Program[i];
                    switch (inst.Type) {
                        case Instruction.inp: inp(inst, input); break;
                        case Instruction.add: add(inst); break;
                        case Instruction.mul: mul(inst); break;
                        case Instruction.div: div(inst); break;
                        case Instruction.mod: mod(inst); break;
                        case Instruction.eql: eql(inst); break;

                        default:
                            throw new Exception("Invalid instruction");
                    }
                    //Console.WriteLine("{0},{1},{2},{3},{4}", inst.Type.ToString(), VarW, VarX, VarY, VarZ);
                }
            }

            private int GetVarA(ProgramInstruction inst) {
                int a = inst.NonVarA;
                switch (inst.VariableA) {
                    case Var.W: a = VarW; break;
                    case Var.X: a = VarX; break;
                    case Var.Y: a = VarY; break;
                    case Var.Z: a = VarZ; break;
                }
                return a;
            }

            private int GetVarB(ProgramInstruction inst) {
                int b = inst.NonVarB;
                switch (inst.VariableB) {
                    case Var.W: b = VarW; break;
                    case Var.X: b = VarX; break;
                    case Var.Y: b = VarY; break;
                    case Var.Z: b = VarZ; break;
                }
                return b;
            }

            private void SetVar(Var var, int value) {
                switch (var) {
                    case Var.W: VarW = value; break;
                    case Var.X: VarX = value; break;
                    case Var.Y: VarY = value; break;
                    case Var.Z: VarZ = value; break;
                    default: throw new Exception();
                }
            }

            /*
            private void inp(ProgramInstruction inst) {
                int value = Inputs[InputIndex++];
                SetVar(inst.VariableA, value);
            }
            */

            private void inp(ProgramInstruction inst, int input) {
                SetVar(inst.VariableA, input);
            }

            private void add(ProgramInstruction inst) {
                int a = GetVarA(inst);
                int b = GetVarB(inst);
                SetVar(inst.VariableA, a + b);
            }

            private void mul(ProgramInstruction inst) {
                int a = GetVarA(inst);
                int b = GetVarB(inst);
                SetVar(inst.VariableA, a * b);
            }

            private void div(ProgramInstruction inst) {
                int a = GetVarA(inst);
                int b = GetVarB(inst);
                SetVar(inst.VariableA, a / b);
            }

            private void mod(ProgramInstruction inst) {
                int a = GetVarA(inst);
                int b = GetVarB(inst);
                SetVar(inst.VariableA, a % b);
            }

            private void eql(ProgramInstruction inst) {
                int a = GetVarA(inst);
                int b = GetVarB(inst);
                SetVar(inst.VariableA, (a == b ? 1 : 0));
            }

            public enum Instruction {
                NONE, inp, add, mul, div, mod, eql
            }

            public enum Var {
                Non, W, X, Y, Z
            }

            public class ProgramInstruction {
                public Instruction Type;
                public Var VariableA;
                public Var VariableB;
                public int NonVarA;
                public int NonVarB;

                public ProgramInstruction(string[] fields) {
                    switch (fields[0]) {
                        case "inp": Type = Instruction.inp; break;
                        case "add": Type = Instruction.add; break;
                        case "mul": Type = Instruction.mul; break;
                        case "div": Type = Instruction.div; break;
                        case "mod": Type = Instruction.mod; break;
                        case "eql": Type = Instruction.eql; break;
                        default: throw new Exception();
                    }

                    switch (fields[1]) {
                        case "w": VariableA = Var.W; break;
                        case "x": VariableA = Var.X; break;
                        case "y": VariableA = Var.Y; break;
                        case "z": VariableA = Var.Z; break;
                        default:
                            VariableA = Var.Non;
                            NonVarA = int.Parse(fields[1]);
                            break;
                    }

                    if (fields.Length > 2) {
                        switch (fields[2]) {
                            case "w": VariableB = Var.W; break;
                            case "x": VariableB = Var.X; break;
                            case "y": VariableB = Var.Y; break;
                            case "z": VariableB = Var.Z; break;
                            default:
                                VariableB = Var.Non;
                                NonVarB = int.Parse(fields[2]);
                                break;
                        }
                    }
                }
            }
        }
    }
}
