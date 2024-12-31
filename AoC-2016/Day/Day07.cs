namespace AoC.Day {
    public class Day07
    {
        public static void Run(string file) {
            Console.WriteLine("Day 7: Internet Protocol Version 7" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            int supportTLS = 0;
            int supportSSL = 0;
            foreach(string line in lines) {
                //Some of the real inputs have multiple hypernets.
                List<string> rems = new List<string>();
                List<string> hypernets = new List<string>();
                string[] left = line.Split('[');
                foreach (string test in left) {
                    string[] right = test.Split(']');
                    if (right.Length == 1)
                        rems.Add(right[0]);
                    else {
                        hypernets.Add(right[0]);
                        rems.Add(right[1]);
                    }
                }

                //Part 1
                bool abbaH = false;
                foreach (string hypernet in hypernets) {
                    bool a = HasABBA(hypernet);
                    if (a) {
                        abbaH = true;
                        break;
                    }
                }

                bool remH = false;
                foreach (string rem in rems) {
                    bool a = HasABBA(rem);
                    if (a) {
                        remH = true;
                        break;
                    }
                }

                //Part 2
                bool babH = false;
                string[] remABAs = GetABA(string.Join('-', rems));
                foreach(string remABA in remABAs) {
                    string bab = remABA.Substring(1, 2) + remABA[1];
                    foreach (string hypernet in hypernets) {
                        if (hypernet.Contains(bab)) {
                            babH = true;
                            break;
                        }
                    }

                    if (babH)
                        break;
                }

                //Check
                if (!abbaH && remH) {
                    Console.WriteLine("TLS: " + line);
                    supportTLS++;
                }

                if(babH) {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("SSL: " + line);
                    Console.ResetColor();
                    supportSSL++;
                }

                //Console.WriteLine(line);
                //Console.WriteLine(hypernet);
                //Console.WriteLine(rem);
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + supportTLS);
            //Answer: 118
            Console.WriteLine("Part 2: " + supportSSL);
            //Answer: 260
        }

        private static bool HasABBA(string input) {
            for(int i = 2; i < input.Length-1; i++) {
                if (input[i] == input[i - 1]) {
                    if (input[i - 2] == input[i + 1]) {
                        if (input[i] == input[i + 1])
                            continue;
                        return true;
                    }
                }
            }
            return false;
        }

        private static string[] GetABA(string input) {
            List<string> found = new List<string>();
            for (int i = 1; i < input.Length - 1; i++) {
                if (input[i-1] == input[i + 1] && input[i - 1] != input[i]) {
                    found.Add(input.Substring(i - 1, 3));
                }
            }
            return found.ToArray();
        }
    }
}
