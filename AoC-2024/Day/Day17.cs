using System;
using System.Globalization;
using System.Text;

namespace AoC.Day
{
    public class Day17
    {
        public static void Run(string file) {
            Console.WriteLine("Day 17: Chronospatial Computer" + Environment.NewLine);

            Computer computer = new Computer();

            string[] lines = File.ReadAllLines(file);
            for (int i = 0; i < lines.Length; i++) {
                if(i == 3) {
                    if (lines[i].Length != 0)
                        throw new Exception();
                    continue;
                }
                int lastSpace = lines[i].LastIndexOf(' ')+1;
                string sub = lines[i].Substring(lastSpace);

                if (i == 0) computer.RegisterA = int.Parse(sub);
                else if (i == 1) computer.RegisterB = int.Parse(sub);
                else if (i == 2) computer.RegisterC = int.Parse(sub);
                else if (i == 4) computer.Program = Array.ConvertAll(sub.Split(','), long.Parse);
            }

            Console.WriteLine("When you're ready, enter \"run\" or \"run2\", then something like \"exit\".");
            Console.WriteLine("Starting session.\r\n");
            while (true) {
                Console.WriteLine("A: {0}", computer.RegisterA);
                Console.WriteLine("B: {0}", computer.RegisterB);
                Console.WriteLine("C: {0}", computer.RegisterC);
                Console.WriteLine("Program: {0}", string.Join(',', computer.Program));
                Console.WriteLine();

                Console.Write("3-bit Comp> ");
                string input = Console.ReadLine();
                if (input.ToLower().StartsWith("e") || input.ToLower().StartsWith("q")) {
                    Console.WriteLine("Ending session.");
                    break;
                } else if (input.ToLower().StartsWith("run")) {
                    if (input.Contains('2')) {
                        //Part 2
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Activating \"Part 2\" mode.\r\n");
                        Console.ResetColor();

                        Computer testComp = new Computer();
                        testComp.Program = computer.Program;

                        long[] requiredOutput = computer.Program;
                        string needed = string.Join(",", requiredOutput);
                        int neededLen = needed.Length;
                        Console.WriteLine("{0} required", needed);

                        long newRegA = 0;
                        while (true) {
                            testComp.RegisterA = newRegA;
                            testComp.RegisterB = computer.RegisterB;
                            testComp.RegisterC = computer.RegisterC;
                            testComp.Run();
                            long[] output = testComp.GetOutputArray();

                            if (output.SequenceEqual(requiredOutput)) {
                                //Console.WriteLine("{0} from A = {1} (answer)", string.Join(",", output).PadLeft(neededLen), moreRegA);
                                Console.Write("{0} from A = ", string.Join(",", output).PadLeft(neededLen));
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write(newRegA);
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write(" (answer)");
                                Console.ResetColor();
                                Console.WriteLine();
                                break;
                            }

                            if (newRegA > 0) {
                                //Since we don't want to multiple 0 by 8.
                                int diff = requiredOutput.Length - output.Length;
                                if (diff > -1) {
                                    bool match = true;
                                    for (int k = 0; k < output.Length; k++) {
                                        if (output[k] != requiredOutput[k + diff]) {
                                            match = false;
                                            break;
                                        }
                                    }

                                    if (match) {
                                        Console.WriteLine("{0} from A = {1}", string.Join(",", output).PadLeft(neededLen), newRegA);
                                        newRegA *= 8;
                                        //Never could have solved this on my own.
                                        //Thanks u/KayoNar for the gold star.
                                        continue;
                                    }
                                }
                            }

                            newRegA++;
                        }

                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\r\nReturning to normal operation.");
                        Console.ResetColor();
                    } else {
                        //Part 1
                        Console.Clear();
                        Console.WriteLine("\x1b[3J");
                        Console.WriteLine("Running with the following");
                        Console.WriteLine("A: {0}", computer.RegisterA);
                        Console.WriteLine("B: {0}", computer.RegisterB);
                        Console.WriteLine("C: {0}", computer.RegisterC);
                        Console.WriteLine("Program: {0}", string.Join(',', computer.Program));
                        Console.WriteLine();

                        computer.Run();
                        string output = computer.GetOutputText();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Output: {0}", output);
                        Console.ResetColor();
                    }
                } else if (input.Length > 1) {
                    try {
                        if (input[0] == 'A' || input[0] == 'a')
                            computer.RegisterA = long.Parse(input.Substring(1));
                        else if (input[0] == 'B' || input[0] == 'b')
                            computer.RegisterB = long.Parse(input.Substring(1));
                        else if (input[0] == 'C' || input[0] == 'c')
                            computer.RegisterC = long.Parse(input.Substring(1));
                        else if (input[0] == 'D' || input[0] == 'd') {
                            computer.DebugMode = !computer.DebugMode;
                            Console.WriteLine("Debug mode {0}", computer.DebugMode ? "ON" : "OFF");
                        } else if (input[0] == 'P' || input[0] == 'p') {
                            computer.Program = Array.ConvertAll(input.Substring(1).Split(','), long.Parse);
                            computer.instructionPointer = 0;
                        }
                    } catch(Exception) {
                        Console.WriteLine("Please take your unruly shenanigans elsewhere.");
                    }
                }

                Console.WriteLine();
            }

            //Part 1 - result of "run"
            //Answer: 1,6,3,6,5,6,5,1,7
            //Part 2 - result of "run2"
            //Answer: 247839653009594
        }

        private class Computer {
            public long RegisterA;
            public long RegisterB;
            public long RegisterC;
            public long[] Program;
            public int instructionPointer;
            private List<long> output;
            public bool DebugMode;

            public Computer() {
                RegisterA = 0;
                RegisterB = 0;
                RegisterC = 0;
                Program = Array.Empty<long>();
                instructionPointer = 0;
                output = new List<long>();
                DebugMode = false;
            }

            public void Run() {
                output.Clear();
                instructionPointer = 0;

                while (true) {
                    if (Program.Length < instructionPointer + 1)
                        break;

                    int opcode = (int)Program[instructionPointer];
                    long operand = Program[instructionPointer + 1];

                    switch (opcode) {
                        case 0:
                            if(DebugMode)
                                Console.WriteLine("op0_adv {0}", operand); 
                            op0_adv(operand);
                            break;
                        case 1:
                            if (DebugMode)
                                Console.WriteLine("op1_bxl {0}", operand);
                            op1_bxl(operand);
                            break;
                        case 2:
                            if (DebugMode)
                                Console.WriteLine("op2_bst {0}", operand);
                            op2_bst(operand);
                            break;
                        case 3:
                            if (DebugMode)
                                Console.WriteLine("op3_jnz {0}", operand);
                            op3_jnz(operand);
                            break;
                        case 4:
                            if (DebugMode)
                                Console.WriteLine("op4_bxc {0}", operand);
                            op4_bxc(operand);
                            break;
                        case 5:
                            if (DebugMode)
                                Console.WriteLine("op5_out {0}", operand);
                            op5_out(operand);
                            break;
                        case 6:
                            if (DebugMode)
                                Console.WriteLine("op6_bdv {0}", operand);
                            op6_bdv(operand);
                            break;
                        case 7:
                            if (DebugMode)
                                Console.WriteLine("op7_cdv {0}", operand);
                            op7_cdv(operand);
                            break;
                        default:
                            throw new Exception("Invalid opcode");
                    }
                }
                
                if (DebugMode) Console.WriteLine();
            }

            internal long[] GetOutputArray() {
                return output.ToArray();
            }

            internal string GetOutputText() {
                return string.Join(',', output);
            }

            public long LiteralValue(long operand) {
                return operand;
            }

            public long ComboValue(long operand) {
                if (operand < 0)
                    throw new Exception();
                else if (operand <= 3)
                    return operand;
                else if (operand == 4)
                    return RegisterA;
                else if (operand == 5)
                    return RegisterB;
                else if (operand == 6)
                    return RegisterC;
                else //if (operand == 7)
                    throw new Exception("This is reserved and not in valid programs.");
            }

            private long performDivision(long op) {
                long numerator = RegisterA;
                long denominator = (long)Math.Pow(2, ComboValue(op));
                return numerator / denominator;
            }

            public void op0_adv(long op) {
                RegisterA = performDivision(op);
                instructionPointer += 2;
            }

            public void op1_bxl(long op) {
                //bitwise XOR
                RegisterB = RegisterB ^ LiteralValue(op);
                instructionPointer += 2;
            }

            public void op2_bst(long op) {
                RegisterB = ComboValue(op) % 8 & 0b0111;
                instructionPointer += 2;
            }

            public void op3_jnz(long op) {
                if (RegisterA == 0) {
                    if (DebugMode)
                        Console.WriteLine("- A is 0, do not jump.");
                    instructionPointer += 2;
                    return;
                }
                instructionPointer = (int)LiteralValue(op);
                //Don't increase pointer by two

                if (DebugMode)
                    Console.WriteLine("- Pointer now: {0}\r\n", instructionPointer);
            }

            public void op4_bxc(long op) {
                //op intentionally ignored
                RegisterB = RegisterB ^ RegisterC;
                instructionPointer += 2;
            }

            public void op5_out(long op) {
                long forOutput = ComboValue(op) % 8 & 0b0111;
                if(DebugMode)
                    Console.WriteLine("- Output: {0}", forOutput);
                output.Add(forOutput);
                instructionPointer += 2;
            }

            public void op6_bdv(long op) {
                RegisterB = performDivision(op);
                instructionPointer += 2;
            }

            public void op7_cdv(long op) {
                RegisterC = performDivision(op);
                instructionPointer += 2;
            }

        }
    }
}
