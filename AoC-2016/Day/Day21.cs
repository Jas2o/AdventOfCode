namespace AoC.Day
{
    public class Day21
    {
        public static void Run(string file) {
            Console.WriteLine("Day 21: Scrambled Letters and Hash" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            string start = (lines.Length < 10 ? "abcde" : "abcdefgh");
            string partA = Solve(lines, start, false);

            Console.WriteLine();

            string startB = (lines.Length < 10 ? partA : "fbgdceah");
            //startB = partA;
            string partB = Solve(lines.Reverse().ToArray(), startB, true);

            Console.WriteLine();

            //Console.WriteLine(start);
            Console.WriteLine("Part 1: " + partA);
            //Answer: gbhafcde
            Console.WriteLine("Part 2: " + partB);
            //Answer: bcfaegdh

            /*
            Console.WriteLine();
            Console.WriteLine("Rotate normal");
            Whyyyyy(false);
            Console.WriteLine();
            Console.WriteLine("Rotate reverse");
            Whyyyyy(true);
            */
        }

        private static IEnumerable<T> ShiftRight<T>(IList<T> values, int shift) {
            IEnumerable<T> newBegin = values.Skip(values.Count - shift);
            IEnumerable<T> newEnd = values.Take(values.Count - shift);
            return newBegin.Concat(newEnd);
        }

        private static IEnumerable<T> ShiftLeft<T>(IList<T> values, int shift) {
            IEnumerable<T> newEnd = values.Take(shift);
            IEnumerable<T> newBegin = values.Skip(shift);
            return newBegin.Concat(newEnd);
        }

        private static string Solve(string[] lines, string start, bool reverse) {
            char[] current = start.ToCharArray();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Starting with: {0}", start);
            Console.ResetColor();

            foreach (string line in lines) {
                Console.WriteLine(line);
                string[] fields = line.Split(' ');

                if (fields[0] == "swap") {
                    int posX, posY;
                    if (fields[1] == "position") {
                        posX = int.Parse(fields[2]);
                        posY = int.Parse(fields[5]);
                    } else {
                        posX = Array.IndexOf(current, fields[2][0]);
                        posY = Array.IndexOf(current, fields[5][0]);
                    }
                    if(reverse)
                        (posX, posY) = (posY, posX);

                    char x = current[posX];
                    char y = current[posY];
                    current[posX] = y;
                    current[posY] = x;
                } else if (fields[0] == "rotate") {
                    if (fields[1] == "based") {
                        int ind = Array.IndexOf(current, fields[6][0]);
                        if (reverse) {
                            // I hate everything about this.
                            if (ind == 0)
                                ind = 1;
                            else if (ind % 2 == 0)
                                ind = ((ind + current.Length) / 2) + 1;
                            else
                                ind = (ind / 2) + 1;
                            ind = ind % current.Length;
                            if (ind != 0)
                                current = ShiftLeft(current, ind).ToArray();
                        } else {
                            if (ind >= 4)
                                ind++;
                            ind = ind % current.Length;
                            current = ShiftRight(current, ind + 1).ToArray();
                        }
                    } else {
                        string dir = fields[1];
                        int steps = int.Parse(fields[2]);
                        steps = steps % current.Length;
                        if ((dir == "left" && !reverse) || (reverse && dir == "right")) {
                            current = ShiftLeft(current, steps).ToArray();
                        } else {
                            current = ShiftRight(current, steps).ToArray();
                        }
                    }
                } else if (fields[0] == "reverse") {
                    int posX = int.Parse(fields[2]);
                    int posY = int.Parse(fields[4]);
                    Array.Reverse(current, posX, posY + 1 - posX);
                } else if (fields[0] == "move") {
                    int posX = int.Parse(fields[2]);
                    int posY = int.Parse(fields[5]);
                    if (reverse)
                        (posX, posY) = (posY, posX);

                    char move = current[posX];
                    char[] next = new char[current.Length];
                    int n = 0;
                    for (int i = 0; i < current.Length; i++) {
                        if (i == posY && posY < posX)
                            next[n++] = move;
                        if (i == posX)
                            continue;
                        next[n++] = current[i];
                        if (i == posY && posY > posX)
                            next[n++] = move;
                    }
                    current = next;
                } else {
                    throw new NotImplementedException();
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(current);
                Console.ResetColor();
            }

            return new string(current);
        }

        /*
        private static void Whyyyyy(bool reverse) {
            List<string> examples = new List<string>() {
                "a_______",
                "_a______",
                "__a_____",
                "___a____",
                "____a___",
                "_____a__",
                "______a_",
                "_______a"
            };

            foreach (string ex in examples) {
                char[] current = ex.ToCharArray();

                int ind = Array.IndexOf(current, 'a');
                int ind0 = ind;
                if (reverse) {
                    if (ind == 0)
                        ind = 1;
                    else if (ind % 2 == 0)
                        ind = ((ind + current.Length) / 2) + 1;
                    else
                        ind = (ind / 2) + 1;
                    ind = ind % current.Length;
                    if (ind != 0)
                        current = ShiftLeft(current, ind).ToArray();
                } else {
                    if (ind >= 4)
                        ind++;
                    ind = ind % current.Length;
                    current = ShiftRight(current, ind + 1).ToArray();
                }

                int ind2 = Array.IndexOf(current, 'a');
                Console.WriteLine("{0} => {1} {2} {3} {4}", ex, new string(current), ind0, ind, ind2);
            }
        }
        */
    }
}
