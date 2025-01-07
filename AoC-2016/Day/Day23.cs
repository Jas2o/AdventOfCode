namespace AoC.Day
{
    public class Day23
    {
        //Modified 2016 Day 12

        public static void Run(string file) {
            Console.WriteLine("Day 23: Safe Cracking" + Environment.NewLine);

            Computer computer = new Computer();
            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                computer.ProgramStore.Add(line);
            }
            if (lines.Length > 10)
                computer.RegisterA = 7;
            //computer.Verbose = true;
            computer.Run();

            int partA = computer.RegisterA;
            int partB = 0;
            if (lines.Length > 10) {
                computer.Reset();
                computer.Optimize();
                computer.RegisterA = 12;
                computer.Run();
                partB = computer.RegisterA;
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 10152
            Console.WriteLine("Part 2: " + partB);
            //Answer: 479006712
        }

        private class Computer {
            public int RegisterA;
            public int RegisterB;
            public int RegisterC;
            public int RegisterD;
            public List<string> ProgramStore;
            private List<string[]> Program;
            public int instructionPointer;
            public bool Verbose;

            public Computer() {
                RegisterA = 0;
                RegisterB = 0;
                RegisterC = 0;
                RegisterD = 0;
                ProgramStore = new List<string>();
                instructionPointer = 0;
            }

            public void Reset() {
                RegisterA = 0;
                RegisterB = 0;
                RegisterC = 0;
                RegisterD = 0;
                Verbose = false;
            }

            public void Run() {
                instructionPointer = 0;
                Program = new List<string[]>();
                foreach(string line in ProgramStore) {
                    Program.Add(line.Split(' '));
                }

                int steps = 0;
                while (true) {
                    if (Program.Count < instructionPointer + 1) {
                        if (Verbose)
                            Console.WriteLine();
                        break;
                    }

                    string[] fields = Program[instructionPointer];

                    if(Verbose)
                        Console.WriteLine(string.Join(' ', fields));

                    switch (fields[0]) {
                        case "cpy":
                            cpy(fields);
                            break;
                        case "inc":
                            inc(fields);
                            break;
                        case "dec":
                            dec(fields);
                            break;

                        case "jnz":
                            jnz(fields);
                            break;

                        case "tgl":
                            tgl(fields);
                            break;
                        case "mul":
                            mul(fields);
                            break;
                        case "nop":
                            instructionPointer++;
                            break;

                        default:
                            throw new Exception("Invalid opcode");
                    }

                    if (Verbose) {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("A:{0} B:{1} C:{2} D:{3}", RegisterA, RegisterB, RegisterC, RegisterD);
                        Console.ResetColor();
                    }
                    steps++;
                }
            }

            private void inc(string[] fields) {
                string r = fields[1];
                if (r == "a")
                    RegisterA++;
                else if (r == "b")
                    RegisterB++;
                else if (r == "c")
                    RegisterC++;
                else if (r == "d")
                    RegisterD++;
                instructionPointer++;
            }

            private void dec(string[] fields) {
                string r = fields[1];
                if (r == "a")
                    RegisterA--;
                else if (r == "b")
                    RegisterB--;
                else if (r == "c")
                    RegisterC--;
                else if (r == "d")
                    RegisterD--;
                instructionPointer++;
            }

            private void cpy(string[] fields) {
                int value = StrRegOrNumToValue(fields[1]);
                Set(fields[2], value);
                instructionPointer++;
            }

            private void jnz(string[] fields) {
                int x = StrRegOrNumToValue(fields[1]);
                int y = StrRegOrNumToValue(fields[2]);
                if (x == 0)
                    instructionPointer++;
                else
                    instructionPointer += y;
            }

            private void tgl(string[] fields) {
                int x = StrRegOrNumToValue(fields[1]);

                int pointingAt = instructionPointer + x;
                if (pointingAt < Program.Count) {
                    string opBefore = Program[pointingAt][0];
                    string opAfter = "";

                    int arguments = Program[pointingAt].Length - 1;
                    if (arguments == 1) {
                        if (opBefore == "inc")
                            opAfter = "dec";
                        else
                            opAfter = "inc";
                    } else if (arguments == 2) {
                        if (opBefore == "jnz")
                            opAfter = "cpy";
                        else
                            opAfter = "jnz";
                    } else {
                        throw new NotImplementedException();
                    }

                    if(Verbose)
                        Console.WriteLine("Change {0} from {1} to {2}", pointingAt, opBefore, opAfter);
                    Program[pointingAt][0] = opAfter;
                }
                instructionPointer++;
            }

            private void mul(string[] fields) { //b d a c
                int x = StrRegOrNumToValue(fields[1]); //b
                int y = StrRegOrNumToValue(fields[2]); //d
                Set(fields[2], 0); //d
                Set(fields[4], 0); //c
                int value = x * y;

                if (fields[3] == "a") //a
                    RegisterA += value;
                else if (fields[3] == "b")
                    RegisterB += value;
                else if (fields[3] == "c")
                    RegisterC += value;
                else if (fields[3] == "d")
                    RegisterD += value;
                else
                    throw new ArgumentException();

                instructionPointer++;
            }

            private void Set(string register, int value) {
                if (register == "a")
                    RegisterA = value;
                else if (register == "b")
                    RegisterB = value;
                else if (register == "c")
                    RegisterC = value;
                else if (register == "d")
                    RegisterD = value;
                else
                    throw new ArgumentException();
            }

            private int StrRegOrNumToValue(string input) {
                int x = 0;
                if (!int.TryParse(input, out x)) {
                    if (input == "a")
                        x = RegisterA;
                    else if (input == "b")
                        x = RegisterB;
                    else if (input == "c")
                        x = RegisterC;
                    else if (input == "d")
                        x = RegisterD;
                    else
                        throw new ArgumentException();
                }
                return x;
            }

            public void Optimize() {
                string[] find = new string[] {
                    "cpy b c",
                    "inc a",
                    "dec c",
                    "jnz c -2",
                    "dec d",
                    "jnz d -5"
                };
                string[] replace = new string[] {
                    "mul b d a c",
                    "nop",
                    "nop",
                    "nop",
                    "nop",
                    "nop"
                };

                string[] array = ProgramStore.ToArray();

                int index = 0;
                for(int i = 0; i < array.Length; i++) {
                    if (array[i] == find[0]) {
                        bool match = true;
                        for (int k = 1; k < find.Length; k++) {
                            if (array[i+k] != find[k]) {
                                match = false;
                                break;
                            }
                        }
                        if(match) {
                            index = i;
                            break;
                        }
                    }
                }

                string[] before = array.Take(index).ToArray();
                string[] after = array.Skip(index + find.Length).ToArray();

                ProgramStore = before.Concat(replace).Concat(after).ToList();
            }
        }
    }
}
