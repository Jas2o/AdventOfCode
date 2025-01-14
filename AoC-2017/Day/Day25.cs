namespace AoC.Day
{
    public class Day25
    {
        public static void Run(string file) {
            Console.WriteLine("Day 25: The Halting Problem" + Environment.NewLine);

            Dictionary<char, State> states = new Dictionary<char, State>();
            char currentState = '?';
            int stepsUntilChecksum = 0;

			string[] lines = File.ReadAllLines(file);
            for (int i = 0; i < lines.Length; i++) {
                string line = lines[i];
                if (line.StartsWith("Begin ")) {
                    currentState = lines[i].TakeLast(2).First();
                } else if (line.StartsWith("Perform ")) {
                    string[] fields = lines[i].Split(' ');
                    stepsUntilChecksum = int.Parse(fields[5]);
                } else if (line.StartsWith("In ")) {
                    char id = line[9];
                    string[] relevant = lines.Skip(i + 1).Take(8).ToArray();
                    State s = new State(id, relevant);
                    states.Add(id, s);
                    i += 9; //Including a gap
                }
            }

            Dictionary<int, int> tape = new Dictionary<int, int>();
            int pos = 0;
            for (int step = 0; step < stepsUntilChecksum; step++) {
                State state = states[currentState];
                if (!tape.ContainsKey(pos))
                    tape[pos] = 0;

                if (tape[pos] == 0) {
                    tape[pos] = state.IfCurrentIs0Write;
                    if (state.IfCurrentIs0MoveRight)
                        pos++;
                    else
                        pos--;
                    currentState = state.IfCurrentIs0NextState;
                } else {
                    tape[pos] = state.IfCurrentIs1Write;
                    if (state.IfCurrentIs1MoveRight)
                        pos++;
                    else
                        pos--;
                    currentState = state.IfCurrentIs1NextState;
                }
            }

            int partA = tape.Values.Sum();

            Console.WriteLine("Part 1: " + partA);
            //Answer: 4230
            Console.WriteLine("Part 2: (N/A)");
			//Answer: (there is no Part 2).
        }

        private class State {
            public char ID;

            public int IfCurrentIs0Write;
            public bool IfCurrentIs0MoveRight;
            public char IfCurrentIs0NextState;

            public int IfCurrentIs1Write;
            public bool IfCurrentIs1MoveRight;
            public char IfCurrentIs1NextState;

            public State(char id, string[] relevant) {
                ID = id;

                bool zeroWriteOne = relevant[1].Contains('1');
                IfCurrentIs0Write = (zeroWriteOne ? 1 : 0);
                IfCurrentIs0MoveRight = relevant[2].Contains("right");
                IfCurrentIs0NextState = relevant[3].TakeLast(2).First();

                bool oneWriteOne = relevant[5].Contains('1');
                IfCurrentIs1Write = (oneWriteOne ? 1 : 0);
                IfCurrentIs1MoveRight = relevant[6].Contains("right");
                IfCurrentIs1NextState = relevant[7].TakeLast(2).First();
            }
        }
    }
}
