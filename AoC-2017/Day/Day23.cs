namespace AoC.Day
{
    public class Day23
    {
        //Based on 2017 Day 18

        public static void Run(string file) {
            Console.WriteLine("Day 23: Coprocessor Conflagration" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            if (lines.Length < 5) {
                Console.WriteLine("There is no test for this day.");
                return;
            }
            
            Computer computer = new Computer(lines);
            int partA = computer.RunPart1();
            int partB = computer.RunPart2();

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 8281
            Console.WriteLine("Part 2: " + partB);
            //Answer: 911
        }

        private class Computer {
            private Dictionary<string, int> dRegister = new Dictionary<string, int>();
            private List<string> Program;
            private int numMul = 0;

            public Computer(string[] lines) {
                Program = new List<string>();
                Program.AddRange(lines);
            }

            public int RunPart1() {
                numMul = 0;
                dRegister.Clear();
                Run(false);
                return numMul;
            }

            public int RunPart2() {
                dRegister.Clear();
                dRegister["a"] = 1;

                //Run part of the program just to get values b and c.
                Run(true);

                string[] setD = Program.Where(x => x.StartsWith("set d")).ToArray();
                string[] setE = Program.Where(x => x.StartsWith("set e")).ToArray();
                string[] subB = Program.Where(x => x.StartsWith("sub b -")).ToArray();

                if (setD.Count() != 1 || setE.Count() != 1 || subB.Count() == 0)
                    throw new Exception();

                int bStart = dRegister["b"];
                int cEnd = dRegister["c"];
                int incB = int.Parse(subB.Last().Substring(7));
                int dStart = int.Parse(setD[0].Substring(6));
                int eStart = int.Parse(setE[0].Substring(6));

                int h = 0;

                Console.WriteLine("Running on \"other\" computer.");
                for (int b = bStart; b <= cEnd; b += incB) {
                    for (int d = dStart; d < b; d++) {
                        if (b % d != 0)
                            continue;
                        for (int e = eStart; e < b; e++) {
                            if (d * e == b) {
                                h++;
                                Console.Write("{0} * {1} = {2}\t", d, e, b);
                                if (h % 5 == 0)
                                    Console.WriteLine();
                                d = b;
                                e = b;
                                break;
                            }
                        }
                    }
                }
                Console.WriteLine();

                return h;
            }

            private void Run(bool isForPart2) {
                int instructionPointer = 0;
                while (true) {
                    if (Program.Count < instructionPointer + 1)
                        break;

                    if (isForPart2 && Program[instructionPointer] == "set f 1") {
                        Console.WriteLine("b is {0} and c is {1}", dRegister["b"], dRegister["c"]);
                        Console.WriteLine("Stopping on \"this\" computer.");
                        break;
                    }

                    string[] fields = Program[instructionPointer].Split(' ');
                    string op = fields[0];

                    int regvalue;
                    string reg = fields[1];
                    if (!int.TryParse(fields[1], out regvalue)) {
                        if (!dRegister.ContainsKey(reg))
                            dRegister.Add(reg, 0);
                        regvalue = dRegister[reg];
                    }

                    int value = 0;
                    if (fields.Length == 3) {
                        if (!int.TryParse(fields[2], out value))
                            value = dRegister[fields[2]];
                    }

                    if (!dRegister.ContainsKey(reg))
                        dRegister.Add(reg, 0);

                    int pointeradd = 1;
                    switch (op) {
                        case "set": dRegister[reg] = value; break;
                        case "sub": dRegister[reg] -= value; break;
                        case "mul":
                            dRegister[reg] *= value;
                            numMul++;
                            break;
                        case "jnz":
                            if (regvalue != 0)
                                pointeradd = (int)value;
                            break;
                        default:
                            throw new Exception("Invalid opcode");
                    }
                    instructionPointer += pointeradd;
                }
            }
        }
    }
}
