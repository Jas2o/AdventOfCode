namespace AoC {
    public class Day23
    {
        public static void Run(string file) {
            Console.WriteLine("Day 23: Opening the Turing Lock" + Environment.NewLine);

            Computer computer = new Computer();
            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                computer.Program.Add(line);
            }

            computer.Run();
            Console.WriteLine("Part 1: A is {0} and B is {1}", computer.RegisterA, computer.RegisterB);
            //Answer: B is 170

            computer.RegisterA = 1;
            computer.RegisterB = 0;
            computer.Run();

            Console.WriteLine("Part 2: A is {0} and B is {1}", computer.RegisterA, computer.RegisterB);
            //Answer: B is 247
        }

        private class Computer {
            public uint RegisterA;
            public uint RegisterB;
            public List<string> Program;
            public int instructionPointer;

            public Computer() {
                RegisterA = 0;
                RegisterB = 0;
                Program = new List<string>();
                instructionPointer = 0;
            }

            public void Run() {
                instructionPointer = 0;

                while (true) {
                    if (Program.Count < instructionPointer + 1)
                        break;

                    string[] fields = Program[instructionPointer].Split(' ', 2);

                    switch (fields[0]) {
                        case "hlf":
                            hlf(fields[1]);
                            break;
                        case "tpl":
                            tpl(fields[1]);
                            break;
                        case "inc":
                            inc(fields[1]);
                            break;

                        case "jmp":
                            jmp(fields[1]);
                            break;
                        case "jie":
                            jie(fields[1]);
                            break;
                        case "jio":
                            jio(fields[1]);
                            break;

                        default:
                            throw new Exception("Invalid opcode");
                    }
                }
            }

            private void hlf(string r) {
                if (r == "a")
                    RegisterA = RegisterA / 2;
                else if (r == "b")
                    RegisterB = RegisterB / 2;
                instructionPointer++;
            }

            private void tpl(string r) {
                if (r == "a")
                    RegisterA = RegisterA * 3;
                else if (r == "b")
                    RegisterB = RegisterB * 3;
                instructionPointer++;
            }

            private void inc(string r) {
                if (r == "a")
                    RegisterA++;
                else if (r == "b")
                    RegisterB++;
                instructionPointer++;
            }

            private void jmp(string offset) {
                string offset_sign = offset.Substring(0, 1);
                int offset_num = int.Parse(offset.Substring(1));
                if (offset_sign == "-")
                    instructionPointer -= offset_num;
                else
                    instructionPointer += offset_num;
            }

            private void jie(string r_offset) {
                //Jump if register is even
                string r = r_offset.Substring(0, 1);
                if ((r == "a" && RegisterA % 2 == 1) || (r == "b" && RegisterB % 2 == 1)) {
                    instructionPointer++;
                    return;
                }
                jmp(r_offset.Substring(1));
            }

            private void jio(string r_offset) {
                //Jump if register is one
                string r = r_offset.Substring(0, 1);
                if ((r == "a" && RegisterA != 1) || (r == "b" && RegisterB != 1)) {
                    instructionPointer++;
                    return;
                }
                jmp(r_offset.Substring(1));
            }
        }
    }
}
