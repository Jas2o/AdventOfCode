namespace AoC.Day
{
    public class Day25
    {
        //Modified 2016 Day 12 (not Day 23)

        public static void Run(string file) {
            Console.WriteLine("Day 25: Clock Signal" + Environment.NewLine);

            Computer computer = new Computer();
            string[] lines = File.ReadAllLines(file);
            if (lines.Length < 5) {
                Console.WriteLine("There is no test for this day.");
                return;
            }
            foreach (string line in lines)
                computer.Program.Add(line);

            int partA = 0;
            for (int i = 0; i < int.MaxValue; i++) {
                Console.Write('.');
                computer.Reset();
                computer.RegisterA = i;
                bool clockLoop = computer.Run();
                if(clockLoop) {
                    partA = i;
                    break;
                }
            }

            Console.WriteLine("\r\n");
            Console.WriteLine("Part 1: " + partA);
            //Answer: 189
            Console.WriteLine("Part 2: (N/A)");
            //Answer: (there is no Part 2).
        }

        private class Computer {
            public int RegisterA;
            public int RegisterB;
            public int RegisterC;
            public int RegisterD;
            public List<string> Program;
            public int instructionPointer;

            private List<int> Clock;

            public Computer() {
                RegisterA = 0;
                RegisterB = 0;
                RegisterC = 0;
                RegisterD = 0;
                Program = new List<string>();
                instructionPointer = 0;

                Clock = new List<int>();
            }

            public void Reset() {
                RegisterA = 0;
                RegisterB = 0;
                RegisterC = 0;
                RegisterD = 0;
                Clock.Clear();
            }

            public bool Run() {
                instructionPointer = 0;

                while (true) {
                    if (Program.Count < instructionPointer + 1)
                        break;
                    if (Clock.Count > 50) //For my input, greater than 8 was enough.
                        return true;

                    string[] fields = Program[instructionPointer].Split(' ', 2);

                    switch (fields[0]) {
                        case "cpy":
                            cpy(fields[1]);
                            break;
                        case "inc":
                            inc(fields[1]);
                            break;
                        case "dec":
                            dec(fields[1]);
                            break;

                        case "jnz":
                            jnz(fields[1]);
                            break;

                        case "out":
                            op_out(fields[1]);

                            if(Clock.Count > 1) {
                                int[] compare = Clock.TakeLast(2).ToArray();
                                if (compare[0] == compare[1])
                                    return false;
                            }
                            break;

                        default:
                            throw new Exception("Invalid opcode");
                    }

                }

                return false;
            }

            private void inc(string r) {
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

            private void dec(string r) {
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

            private void cpy(string v) {
                string[] fields = v.Split(' ', 2);
                int value = 0;
                if (!int.TryParse(fields[0], out value)) {
                    if (fields[0] == "a")
                        value = RegisterA;
                    else if (fields[0] == "b")
                        value = RegisterB;
                    else if (fields[0] == "c")
                        value = RegisterC;
                    else if (fields[0] == "d")
                        value = RegisterD;
                }

                if (fields[1] == "a")
                    RegisterA = value;
                else if (fields[1] == "b")
                    RegisterB = value;
                else if (fields[1] == "c")
                    RegisterC = value;
                else if (fields[1] == "d")
                    RegisterD = value;

                instructionPointer++;
            }

            private void jnz(string v) {
                string[] fields = v.Split(' ', 2);

                int x = 0;
                if (!int.TryParse(fields[0], out x)) {
                    if (fields[0] == "a")
                        x = RegisterA;
                    else if (fields[0] == "b")
                        x = RegisterB;
                    else if (fields[0] == "c")
                        x = RegisterC;
                    else if (fields[0] == "d")
                        x = RegisterD;
                }

                int y = int.Parse(fields[1]);

                if (x == 0)
                    instructionPointer++;
                else
                    instructionPointer += y;
            }

            private void op_out(string x) {
                int value = 0;
                if (!int.TryParse(x, out value)) {
                    if (x == "a")
                        value = RegisterA;
                    else if (x == "b")
                        value = RegisterB;
                    else if (x == "c")
                        value = RegisterC;
                    else if (x == "d")
                        value = RegisterD;
                }
                Clock.Add(value);
                instructionPointer++;
            }
        }
    }
}
