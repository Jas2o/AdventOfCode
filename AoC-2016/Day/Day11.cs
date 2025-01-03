namespace AoC.Day {
    public class Day11
    {
        //I hate everything about this one.
        //Thanks go to u/p_tseng for freeing me from hell.
        //https://www.reddit.com/r/adventofcode/comments/5hoia9/comment/db1v1ws/

        private static readonly string[] separator = new string[] { ",", " and " };

        public static void Run(string file) {
            Console.WriteLine("Day 11: Radioisotope Thermoelectric Generators" + Environment.NewLine);

            List<string> lines = File.ReadAllLines(file).ToList();
            Simulation sim = new Simulation(lines);

            //Part 1
            Console.WriteLine("Part 1:");
            sim.DrawState(sim.initialState);
            //int stepsA = sim.Example();
            int stepsA = sim.SolveWithQueue();

            //Part 2
            lines[0] = lines[0] + ", elerium generator, elerium-compatible microchip, dilithium generator, and dilithium-compatible microchip.";
            sim = new Simulation(lines);
            Console.WriteLine("Part 2:");
            sim.DrawState(sim.initialState);
            int stepsB = sim.SolveWithQueue();

            Console.WriteLine("Part 1: " + stepsA);
            //Answer: 37
            Console.WriteLine("Part 2: " + stepsB);
            //Answer: 61
        }

        private class Simulation {
            private static string Elevator = "E";

            public int FloorMax { get;  private set; }
            private Dictionary<string, (int, int)> dictionary;
            public int[] initialState;
            private int[] pairs;
            private bool[] microchip;
            private bool[] generator;

            public Simulation(List<string> lines) {
                FloorMax = lines.Count-1;
                dictionary = new Dictionary<string, (int, int)>(); //Name, (Floor, Index)
                dictionary.Add(Elevator, (0, 0));

                List<string> tempList = new List<string>();
                for (int floor = 0; floor <= FloorMax; floor++) {
                    string[] things = lines[floor].Split("contains", 2).Last().Replace(".", "").Replace(" a ", " ").
                        Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    foreach (string thing in things) {
                        if (thing == "nothing relevant")
                            continue;
                        string[] parts = thing.Split(' ');
                        parts[0] = parts[0].Replace("-compatible", "");

                        if (tempList.Contains(parts[0])) {
                            string key = (parts[1] == "generator" ? parts[0][0] + "-Gen" : parts[0][0] + "-M" );
                            dictionary[key] = (floor, dictionary[key].Item2);
                        } else {
                            tempList.Add(parts[0]);
                            dictionary.Add(parts[0][0] + "-Gen", (floor, dictionary.Count));
                            dictionary.Add(parts[0][0] + "-M", (floor, dictionary.Count));
                        }

                        
                    }
                }

                microchip = new bool[dictionary.Count];
                generator = new bool[dictionary.Count];
                pairs = new int[dictionary.Count];
                initialState = new int[dictionary.Count]; //This describes which floor each thing is at.
                for (int i = 0; i < initialState.Length; i++) {
                    if (i == 0)
                        continue;
                    initialState[i] = dictionary.ElementAt(i).Value.Item1;
                    string[] words = dictionary.ElementAt(i).Key.Split('-');
                    if (words[1].StartsWith("G")) {
                        generator[i] = true;

                        int iMicro = dictionary[words[0] + "-M"].Item2;
                        pairs[i] = iMicro;
                        pairs[iMicro] = i;
                        microchip[iMicro] = true;
                    }
                }
            }

            public int SolveWithQueue() {
                Queue<Tuple<int, int[]>> queueState = new Queue<Tuple<int, int[]>>(); //Depth, state
                queueState.Enqueue(new Tuple<int, int[]>(0, initialState));

                Dictionary<string, bool> seenStates = new Dictionary<string, bool>();
                // We don't use the bool, but using a list would be a lot slower.

                while (queueState.Count > 0) {
                    Tuple<int, int[]> state = queueState.Dequeue();
                    int depth = state.Item1;
                    int[] givenState = state.Item2;

                    int[] interchangeable = new int[state.Item2.Length / 2 + 1];
                    interchangeable[0] = givenState[0]; //Elevator
                    for (int p = 0; p < interchangeable.Length - 1; p++) {
                        int v1 = givenState[p * 2 + 1]; //Generator
                        int v2 = givenState[p * 2 + 2]; //Microchip
                        interchangeable[p + 1] = v1 * 100 + v2;
                    }
                    Array.Sort(interchangeable);

                    string given = string.Join(',', interchangeable);
                    //string given = string.Join(',', givenState);
                    if (seenStates.ContainsKey(given))
                        continue;
                    seenStates[given] = true;

                    if (givenState.All(x => x == FloorMax))
                        return depth;

                    int itemsOnThisFloor = givenState.Where(x => x == givenState[0]).Count() - 1;
                    for (int left = 1; left < givenState.Length; left++) {
                        for (int right = 0; right < givenState.Length; right++) {
                            if (left == right)
                                continue;

                            // "If you can move two items upstairs, don't bother bringing one item upstairs."
                            // I couldn't be bothered to implement this hint.
                            int[] stateUp = (int[])givenState.Clone();
                            bool canUp = Up(stateUp, left, right);
                            if (canUp) {
                                queueState.Enqueue(new Tuple<int, int[]>(depth + 1, stateUp));
                            }

                            if (givenState.Any(x => x < given[0])) {
                                // "If you can move one item downstairs, don't bother bringing two items downstairs."
                                if (itemsOnThisFloor == 1 && right != 0)
                                    continue;

                                int[] stateDown = (int[])givenState.Clone();
                                bool canDown = Down(stateDown, left, right);
                                if (canDown) {
                                    queueState.Enqueue(new Tuple<int, int[]>(depth + 1, stateDown));
                                }
                            }
                        }
                    }
                }

                return 0;
            }

            public void DrawState(int[] state) {
                //Console.WriteLine(string.Join(',', generator) + " (G)");
                //Console.WriteLine(string.Join(',', microchip) + " (M)");
                //Console.WriteLine(string.Join(',', state));

                int longest = dictionary.Keys.OrderByDescending(s => s.Length).First().Length;
                for (int floor = FloorMax; floor > -1; floor--) {
                    Console.Write("F{0} ", floor+1);
                    for (int i = 0; i < state.Length; i++) {
                        string label = dictionary.ElementAt(i).Key;
                        if (state[i] == floor) {
                            if (label == Elevator)
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write("{0}  ", label);
                        } else {
                            Console.Write("{0}  ", ".".PadRight(label.Length));
                        }
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            public bool Validate(int[] state) {
                for (int i = 1; i < state.Length; i++) {
                    if (microchip[i]) {
                        int iGen = pairs[i];
                        if (state[i] == state[iGen])
                            continue;

                        for (int z = 1; z < state.Length; z++) {
                            //Find a non-pair generator (that has a microchip) on the same floor
                            if (generator[z] && state[z] == state[i]) {
                                int otherMicro = pairs[z];
                                if(state[otherMicro] == state[i])
                                    return false;
                            }
                        }
                    }
                }
                return true;
            }

            public bool Up(int[]state, int index1, int index2 = 0) {
                if (index1 == 0 || state[0] != state[index1] || state[0] == FloorMax)
                    return false;
                if (index2 != 0) {
                    if (state[0] == state[index2])
                        state[index2]++;
                    else
                        return false;
                }
                state[index1]++;
                state[0]++; //Elevator
                return Validate(state);
            }

            public bool Down(int[] state, int index1, int index2 = 0) {
                if (index1 == 0 || state[0] != state[index1] || state[0] == 0)
                    return false;
                if (index2 != 0) {
                    if (state[0] == state[index2])
                        state[index2]--;
                    else
                        return false;
                }
                state[index1]--;
                state[0]--; //Elevator
                return Validate(state);
            }

            public int Example() {
                int[] state = (int[])initialState.Clone();

                int steps = 0;
                if (Up(state, 2)) {
                    steps++;
                    DrawState(state);
                }
                if (Up(state, 1, 2)) {
                    steps++;
                    DrawState(state);
                }
                if (Down(state, 2)) {
                    steps++;
                    DrawState(state);
                }
                if (Down(state, 2)) {
                    steps++;
                    DrawState(state);
                }
                if (Up(state, 2, 4)) {
                    steps++;
                    DrawState(state);
                }
                if (Up(state, 2, 4)) {
                    steps++;
                    DrawState(state);
                }
                if (Up(state, 2, 4)) {
                    steps++;
                    DrawState(state);
                }
                if (Down(state, 2)) {
                    steps++;
                    DrawState(state);
                }
                if (Up(state, 1, 3)) {
                    steps++;
                    DrawState(state);
                }
                if (Down(state, 4)) {
                    steps++;
                    DrawState(state);
                }
                if (Up(state, 2, 4)) {
                    steps++;
                    DrawState(state);
                }
                return steps;
            }
        }
    }
}
