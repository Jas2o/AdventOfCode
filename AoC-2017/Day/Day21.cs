namespace AoC.Day {
    public class Day21
    {
        public static void Run(string file) {
            Console.WriteLine("Day 21: Fractal Art" + Environment.NewLine);

            Dictionary<string, Rule> rules = new Dictionary<string, Rule>();
			string[] lines = File.ReadAllLines(file);
            foreach(string line in lines) {
                Rule r0 = new Rule(line);

                //Rotates
                char[][] cRot1 = new char[r0.Input.Length][];
                char[][] cRot2 = new char[r0.Input.Length][];
                char[][] cRot3 = new char[r0.Input.Length][];
                for(int i = 0; i < r0.Input.Length; i++) {
                    cRot1[i] = r0.Input[i].ToCharArray();
                    cRot2[i] = r0.Input[i].ToCharArray();
                    cRot3[i] = r0.Input[i].ToCharArray();
                }

                if(cRot1.Length == 2) {
                    (cRot1[0][0], cRot1[0][1], cRot1[1][1], cRot1[1][0]) =
                        (cRot1[1][0], cRot1[0][0], cRot1[0][1], cRot1[1][1]);
                    (cRot2[0][0], cRot2[0][1], cRot2[1][1], cRot2[1][0]) =
                        (cRot1[1][0], cRot1[0][0], cRot1[0][1], cRot1[1][1]);
                    (cRot3[0][0], cRot3[0][1], cRot3[1][1], cRot3[1][0]) =
                        (cRot2[1][0], cRot2[0][0], cRot2[0][1], cRot2[1][1]);
                } else if(cRot1.Length == 3) {
                    (cRot1[0][0], cRot1[0][1], cRot1[0][2], cRot1[1][2], cRot1[2][2], cRot1[2][1], cRot1[2][0], cRot1[1][0]) =
                        (cRot1[2][0], cRot1[1][0], cRot1[0][0], cRot1[0][1], cRot1[0][2], cRot1[1][2], cRot1[2][2], cRot1[2][1]);

                    (cRot2[0][0], cRot2[0][1], cRot2[0][2], cRot2[1][2], cRot2[2][2], cRot2[2][1], cRot2[2][0], cRot2[1][0]) =
                        (cRot1[2][0], cRot1[1][0], cRot1[0][0], cRot1[0][1], cRot1[0][2], cRot1[1][2], cRot1[2][2], cRot1[2][1]);

                    (cRot3[0][0], cRot3[0][1], cRot3[0][2], cRot3[1][2], cRot3[2][2], cRot3[2][1], cRot3[2][0], cRot3[1][0]) =
                        (cRot2[2][0], cRot2[1][0], cRot2[0][0], cRot2[0][1], cRot2[0][2], cRot2[1][2], cRot2[2][2], cRot2[2][1]);
                } else {
                    throw new Exception();
                }
                string[] sRot1 = new string[cRot1.Length];
                string[] sRot2 = new string[cRot2.Length];
                string[] sRot3 = new string[cRot3.Length];
                for (int i = 0; i < cRot1.Length; i++) {
                    sRot1[i] = new string(cRot1[i]);
                    sRot2[i] = new string(cRot2[i]);
                    sRot3[i] = new string(cRot3[i]);
                }

                Rule r1 = new Rule(r0, sRot1);
                Rule r2 = new Rule(r0, sRot2);
                Rule r3 = new Rule(r0, sRot3);

                rules.TryAdd(r0.InputC, r0);
                rules.TryAdd(r1.InputC, r1);
                rules.TryAdd(r2.InputC, r2);
                rules.TryAdd(r3.InputC, r3);

                //Flips
                string[] sFlip0 = r0.Input.Reverse().ToArray();
                string[] sFlip1 = r1.Input.Reverse().ToArray();
                string[] sFlip2 = r2.Input.Reverse().ToArray();
                string[] sFlip3 = r3.Input.Reverse().ToArray();

                Rule rf0 = new Rule(r0, sFlip0);
                Rule rf1 = new Rule(r0, sFlip1);
                Rule rf2 = new Rule(r0, sFlip2);
                Rule rf3 = new Rule(r0, sFlip3);

                rules.TryAdd(rf0.InputC, rf0);
                rules.TryAdd(rf1.InputC, rf1);
                rules.TryAdd(rf2.InputC, rf2);
                rules.TryAdd(rf3.InputC, rf3);
            }

            if (lines.Length < 10) {
                int example = Solve(rules, 5); //This will actually stop after 2
                Console.WriteLine(); 
                Console.WriteLine("Example: " + example);
            } else {
                int part1 = Solve(rules, 5);
                int part2 = Solve(rules, 18);

                Console.WriteLine();
                Console.WriteLine("Part 1: " + part1);
                //Answer: 110
                Console.WriteLine("Part 2: " + part2);
                //Answer: 1277716
            }
        }

        private static int Solve(Dictionary<string, Rule> rules, int iterations, int iterationsDrawLimit = 11) {
            string[] grid = { ".#.", "..#", "###" };

            if(iterations < iterationsDrawLimit)
                DrawArray(grid);

            try {
                for (int iteration = 0; iteration < iterations; iteration++) {
                    Console.WriteLine("Iteration {0}", iteration + 1);

                    string[] gridnext = null;
                    if (grid.Length == 2 || grid.Length == 3) {
                        //2x2 to 3x3, or 3x3 to 4x4
                        string compare = string.Join("", grid);
                        gridnext = rules[compare].Output;
                    } else if (grid.Length % 2 == 0) {
                        //Break into 2x2, change into 3x3, then join together.
                        List<string[]> subs = new List<string[]>();
                        int dim = 2;
                        int div = grid.Length / dim;
                        for (int y = 0; y < div; y++) {
                            for (int x = 0; x < div; x++) {
                                string[] sub = new string[dim];
                                string temp = grid[y * dim];
                                sub[0] = grid[y * dim].Substring(x * dim, dim);
                                sub[1] = grid[y * dim + 1].Substring(x * dim, dim);

                                string compare = string.Join("", sub);
                                string[] subnext = rules[compare].Output;
                                subs.Add(subnext);
                            }
                        }

                        int newDim = 3;
                        gridnext = new string[newDim * div];
                        int i = 0;
                        for (int y = 0; y < div; y++) {
                            for (int x = 0; x < div; x++) {
                                if (x == 0) {
                                    gridnext[y * newDim] = subs[i][0];
                                    gridnext[y * newDim + 1] = subs[i][1];
                                    gridnext[y * newDim + 2] = subs[i][2];
                                } else {
                                    gridnext[y * newDim] += subs[i][0];
                                    gridnext[y * newDim + 1] += subs[i][1];
                                    gridnext[y * newDim + 2] += subs[i][2];
                                }
                                i++;
                            }
                        }
                    } else if (grid.Length % 3 == 0) {
                        //Break into 3x3, change into 4x4, then join together.
                        List<string[]> subs = new List<string[]>();
                        int dim = 3;
                        int div = grid.Length / dim;
                        for (int y = 0; y < div; y++) {
                            for (int x = 0; x < div; x++) {
                                string[] sub = new string[dim];
                                string temp = grid[y * dim];
                                sub[0] = grid[y * dim].Substring(x * dim, dim);
                                sub[1] = grid[y * dim + 1].Substring(x * dim, dim);
                                sub[2] = grid[y * dim + 2].Substring(x * dim, dim);

                                string compare = string.Join("", sub);
                                string[] subnext = rules[compare].Output;
                                subs.Add(subnext);
                            }
                        }

                        int newDim = 4;
                        gridnext = new string[newDim * div];
                        int i = 0;
                        for (int y = 0; y < div; y++) {
                            for (int x = 0; x < div; x++) {
                                if (x == 0) {
                                    gridnext[y * newDim] = subs[i][0];
                                    gridnext[y * newDim + 1] = subs[i][1];
                                    gridnext[y * newDim + 2] = subs[i][2];
                                    gridnext[y * newDim + 3] = subs[i][3];
                                } else {
                                    gridnext[y * newDim] += subs[i][0];
                                    gridnext[y * newDim + 1] += subs[i][1];
                                    gridnext[y * newDim + 2] += subs[i][2];
                                    gridnext[y * newDim + 3] += subs[i][3];
                                }
                                i++;
                            }
                        }

                    }

                    if (iterations < iterationsDrawLimit)
                        DrawArray(gridnext);

                    grid = gridnext;
                }
            } catch(KeyNotFoundException) {
                Console.WriteLine("Stopped early as could not match a square to a rule.");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            int answer = 0;
            foreach (string s in grid) {
                if (s == null)
                    break;
                int count = s.Replace(".", "").Length;
                answer += count;
            }
            return answer;
        }

        private static void DrawArray(string[] array) {
            for (int y = 0; y < array.Length; y++) {
                Console.WriteLine(array[y]);
            }
            Console.WriteLine();
        }

        private class Rule {
            public int SizeIn;
            public int SizeOut;
            public string InputC;
            public string[] Input;
            public string[] Output;

            public Rule(string line) {
                int endOfInput = line.IndexOf(' ');
                int startOfOutput = line.LastIndexOf(' ') + 1;

                Input = line.Substring(0, endOfInput).Split('/');
                Output = line.Substring(startOfOutput).Split('/');

                InputC = string.Join("", Input);

                SizeIn = Input.Length;
                SizeOut = Output.Length;
            }

            public Rule(Rule original, string[] altInput) {
                Output = original.Output;
                SizeIn = original.SizeIn;
                SizeOut = original.SizeOut;

                Input = altInput;
                InputC = string.Join("", Input);
            }
        }
    }
}
