namespace AoC.Day
{
    public class Day12 {
        public static void Run(string file) {
            Console.WriteLine("Day 12: Leonardo's Monorail" + Environment.NewLine);

            Computer computer = new Computer();
            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                computer.Program.Add(line);
            }
            computer.Run();
            int partA = computer.RegisterA;

            computer.Reset();
            computer.RegisterC = 1;
            computer.Run();
            int partB = computer.RegisterA;

            Console.WriteLine("Part 1: " + partA);
            //Answer: 318009

            Console.WriteLine("Part 2: " + partB);
            //Answer: 9227663
        }

        private class Computer {
            public int RegisterA;
            public int RegisterB;
            public int RegisterC;
            public int RegisterD;
            public List<string> Program;
            public int instructionPointer;

            public Computer() {
                RegisterA = 0;
                RegisterB = 0;
                RegisterC = 0;
                RegisterD = 0;
                Program = new List<string>();
                instructionPointer = 0;
            }

            public void Reset() {
                RegisterA = 0;
                RegisterB = 0;
                RegisterC = 0;
                RegisterD = 0;
            }

            public void Run() {
                instructionPointer = 0;

                while (true) {
                    if (Program.Count < instructionPointer + 1)
                        break;

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

                        default:
                            throw new Exception("Invalid opcode");
                    }
                }
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
                if(!int.TryParse(fields[0], out value)) {
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
        }
    }
}
