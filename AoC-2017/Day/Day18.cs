namespace AoC.Day {
    public class Day18
    {
        public static void Run(string file) {
            Console.WriteLine("Day 18: Duet" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            Computer computer = new Computer(lines);
            long partA = computer.RunPart1();
            int partB = computer.RunPart2();

            Console.WriteLine("Part 1: " + partA);
            //Answer: 4601
            Console.WriteLine("Part 2: " + partB);
            //Answer: 6858
        }

        private class Computer {
            private List<string> Program;

            public Computer(string[] lines) {
                Program = new List<string>();
                Program.AddRange(lines);
            }

            public long RunPart1() {
                long SoundLast = 0;
                int instructionPointer = 0;
                Dictionary<string, long> dRegister = new Dictionary<string, long>();

                while (true) {
                    if (Program.Count < instructionPointer + 1)
                        break;

                    string[] fields = Program[instructionPointer].Split(' ');
                    string op = fields[0];

                    long regvalue;
                    string reg = fields[1];
                    if (!long.TryParse(fields[1], out regvalue)) {
                        if (!dRegister.ContainsKey(reg))
                            dRegister.Add(reg, 0);
                        regvalue = dRegister[reg];
                    }

                    long value = 0;
                    if (fields.Length == 3) {
                        if (!long.TryParse(fields[2], out value))
                            value = dRegister[fields[2]];
                    }

                    if (!dRegister.ContainsKey(reg))
                        dRegister.Add(reg, 0);

                    int pointeradd = 1;
                    switch (op) {
                        case "set": dRegister[reg] = value; break;
                        case "add": dRegister[reg] += value; break;
                        case "mul": dRegister[reg] *= value; break;
                        case "mod": dRegister[reg] %= value; break;
                        case "snd":
                            SoundLast = regvalue;
                            break;
                        case "rcv":
                            if(regvalue != 0) {
                                if (SoundLast != 0)
                                    return SoundLast;
                            }
                            break;
                        case "jgz": if (regvalue > 0) { pointeradd = (int)value; } break;

                        default:
                            throw new Exception("Invalid opcode");
                    }
                    instructionPointer += pointeradd;
                }

                return 0;
            }

            public int RunPart2() {
                Queue<long>[] queue = {
                    new Queue<long>(),
                    new Queue<long>(),
                };
                int[] pointer = { 0, 0 };
                int[] sends = { 0, 0 };

                Dictionary<string, long>[] dRegister = new Dictionary<string, long>[] {
                    new Dictionary<string, long>(),
                    new Dictionary<string, long>()
                };
                dRegister[0]["p"] = 0;
                dRegister[1]["p"] = 1;

                bool run = true;
                while (run) {
                    for (int p = 0; p < 2; p++) {
                        if (Program.Count < pointer[p] + 1)
                            break;

                        string[] fields = Program[pointer[p]].Split(' ');

                        string op = fields[0];
                        
                        long regvalue;
                        string reg = fields[1];
                        if (!long.TryParse(fields[1], out regvalue)) {
                            if (!dRegister[p].ContainsKey(reg))
                                dRegister[p].Add(reg, 0);
                            regvalue = dRegister[p][reg];
                        }
                        long value = 0;
                        if (fields.Length == 3) {
                            if (!long.TryParse(fields[2], out value))
                                value = dRegister[p][fields[2]];
                        }

                        if (!dRegister[p].ContainsKey(reg))
                            dRegister[p].Add(reg, 0);

                        int pointeradd = 1;
                        switch (op) {
                            case "set": dRegister[p][reg] = value; break;
                            case "add": dRegister[p][reg] += value; break;
                            case "mul": dRegister[p][reg] *= value; break;
                            case "mod": dRegister[p][reg] %= value; break;
                            case "snd":
                                queue[1 - p].Enqueue(regvalue);
                                sends[p]++;
                                break;
                            case "rcv":
                                if (queue[p].TryDequeue(out long receive))
                                    dRegister[p][reg] = receive;
                                else {
                                    //It waits, unless a deadlock.
                                    if (pointer[0] == pointer[1] && queue[0].Count == 0 && queue[1].Count == 0)
                                        run = false;
                                    continue;
                                }
                                break;
                            case "jgz": if (regvalue > 0) { pointeradd = (int)value; } break;

                            default:
                                throw new Exception("Invalid opcode");
                        }
                        pointer[p] += pointeradd;
                    }
                }

                return sends[1];
            }
        }
    }
}
