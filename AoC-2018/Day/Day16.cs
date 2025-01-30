namespace AoC.Day
{
    public class Day16
    {
        public static void Run(string file) {
            Console.WriteLine("Day 16: Chronal Classification" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            Computer computer = new Computer(lines);
            int partA = computer.RunSamples();
            int partB = computer.RunTest();

			Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 646
            Console.WriteLine("Part 2: " + partB);
            //Answer: 681
        }

        private class Computer {
            public int[] Register;
            public List<Sample> Samples;
            public List<int[]> TestProgram;
            private Dictionary<int, OpName> Mapping;

            public Computer(string[] lines) {
                Register = new int[4];
                Samples = new List<Sample>();
                TestProgram = new List<int[]>();
                Mapping = new Dictionary<int, OpName>();

                for (int i = 0; i < lines.Length; i++) {
                    string line1 = lines[i];
                    if(line1.StartsWith("Before:")) {
                        string line2 = lines[i + 1];
                        string line3 = lines[i + 2];
                        i += 3; //There's a gap after
                        Sample s = new Sample(line1, line2, line3);
                        Samples.Add(s);
                    } else {
                        if (lines[i].Length > 0) {
                            int[] op = Array.ConvertAll(lines[i].Split(' '), int.Parse);
                            TestProgram.Add(op);
                        }
                    }
                }
            }

            private enum OpName { addr, addi, mulr, muli, banr, bani, borr, bori, setr, seti, gtir, gtri, gtrr, eqir, eqri, eqrr };

            public int RunSamples() {
                Dictionary<OpName, List<int>> mappingSamples = new Dictionary<OpName, List<int>>();
                for(int e = 0; e < 16; e++)
                    mappingSamples[(OpName)e] = new List<int>();

                int threeormore = 0;
                foreach(Sample s in Samples) {
                    int match = 0;
                    for (int op = 0; op < 16; op++) {
                        Array.Copy(s.RegisterBefore, Register, 4);
                        switch(op) {
                            case 0: op_addr(s.Op); break;
                            case 1: op_addi(s.Op); break;
                            case 2: op_mulr(s.Op); break;
                            case 3: op_muli(s.Op); break;
                            case 4: op_banr(s.Op); break;
                            case 5: op_bani(s.Op); break;
                            case 6: op_borr(s.Op); break;
                            case 7: op_bori(s.Op); break;
                            case 8: op_setr(s.Op); break;
                            case 9: op_seti(s.Op); break;
                            case 10: op_gtir(s.Op); break;
                            case 11: op_gtri(s.Op); break;
                            case 12: op_gtrr(s.Op); break;
                            case 13: op_eqir(s.Op); break;
                            case 14: op_eqri(s.Op); break;
                            case 15: op_eqrr(s.Op); break;
                        }

                        if (Register.SequenceEqual(s.RegisterAfter)) {
                            match++;
                            mappingSamples[(OpName)op].Add(s.Op[0]);
                        }
                    }

                    if (match > 2)
                        threeormore++;
                }

                //Compress the lists down to single values ordered by frequency.
                foreach (KeyValuePair<OpName, List<int>> pair in mappingSamples) {
                    mappingSamples[pair.Key] = pair.Value.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).ToList();
                }

                foreach (KeyValuePair<OpName, List<int>> pair in mappingSamples) {
                    if (pair.Value.Count > 0) {
                        Console.WriteLine("{0} could be {1}", pair.Key, string.Join(',', pair.Value));
                    }
                }
                Console.WriteLine();

                if (mappingSamples.Any(m => m.Value.Count == 0)) {
                    Console.WriteLine("Would not be able to match all.");
                } else {
                    while (mappingSamples.Any()) {
                        //Find any that only matched a single op code.
                        IEnumerable<KeyValuePair<OpName, List<int>>> onlyOne = mappingSamples.Where(s => s.Value.Count == 1);
                        if (onlyOne.Any()) {
                            KeyValuePair<OpName, List<int>> pair = onlyOne.First();
                            int val = pair.Value.First();
                            Mapping[val] = pair.Key;

                            Console.WriteLine("Matched op {0} to {1}", val, pair.Key);

                            //Now that that op code is solved, remove it from the search.
                            mappingSamples.Remove(pair.Key);
                            foreach (KeyValuePair<OpName, List<int>> other in mappingSamples) {
                                other.Value.Remove(val);
                            }
                        } else {
                            Console.WriteLine("Matching broke...");
                            break; //The test is incomplete
                        }
                    }
                }

                return threeormore;
            }

            public int RunTest() {
                Array.Clear(Register);

                foreach (int[] Op in TestProgram) {
                    int mapped = (int)Mapping[Op[0]];

                    switch (mapped) {
                        case 0: op_addr(Op); break;
                        case 1: op_addi(Op); break;
                        case 2: op_mulr(Op); break;
                        case 3: op_muli(Op); break;
                        case 4: op_banr(Op); break;
                        case 5: op_bani(Op); break;
                        case 6: op_borr(Op); break;
                        case 7: op_bori(Op); break;
                        case 8: op_setr(Op); break;
                        case 9: op_seti(Op); break;
                        case 10: op_gtir(Op); break;
                        case 11: op_gtri(Op); break;
                        case 12: op_gtrr(Op); break;
                        case 13: op_eqir(Op); break;
                        case 14: op_eqri(Op); break;
                        case 15: op_eqrr(Op); break;
                    }
                }

                return Register[0];
            }

            public class Sample {
                public int[] RegisterBefore;
                public int[] Op;
                public int[] RegisterAfter;

                public Sample(string line1, string line2, string line3) {
                    line1 = line1.Substring(9, 10);
                    line3 = line3.Substring(9, 10);
                    RegisterBefore = Array.ConvertAll(line1.Split(", "), int.Parse);
                    Op = Array.ConvertAll(line2.Split(' '), int.Parse);
                    RegisterAfter = Array.ConvertAll(line3.Split(", "), int.Parse);
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
