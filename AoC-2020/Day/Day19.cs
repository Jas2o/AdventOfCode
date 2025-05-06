using System.Text;
using System.Text.RegularExpressions;

namespace AoC.Day
{
    public class Day19
    {
        public static void Run(string file) {
            Console.WriteLine("Day 19: Monster Messages" + Environment.NewLine);

            Dictionary<int, string> rules = new Dictionary<int, string>();
            List<string> messages = new List<string>();

            string[] lines = File.ReadAllLines(file);
            foreach(string line in lines) {
                if(line.Length == 0) continue;

                string[] fields = line.Split(": ");
                if(fields.Length == 2) {
                    int num = int.Parse(fields[0]);
                    string r = fields[1];
                    if(r.StartsWith('"'))
                        rules.Add(num, r.Substring(1, 1));
                    else
                        rules.Add(num, r);
                } else {
                    messages.Add(fields[0]);
                }
            }

            int partA = SolveA(rules, messages, true);
            int partB = SolveB_Chunks(rules, messages, true);
            int partB_alt = SolveB_ChangeRules(rules, messages, false);

            if (partB != partB_alt) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The two methods disagree.\r\n");
                Console.ResetColor();
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 118
            Console.WriteLine("Part 2: " + partB);
            //Answer: 246
        }

        private static int SolveA(Dictionary<int, string> rules, List<string> messages, bool output) {
            int answer = 0;

            StringBuilder sbA = new StringBuilder();
            Expand(sbA, rules, rules[0]);
            string patternA = string.Format("^{0}$", sbA.ToString());

            if (output) {
                Console.WriteLine("0: " + patternA);
                Console.WriteLine();
            }

            foreach (string msg in messages) {
                if (Regex.IsMatch(msg, patternA)) {
                    answer++;
                    if (output)
                        Console.WriteLine(msg);
                }
            }

            if (output)
                Console.WriteLine();

            return answer;
        }

        private static int SolveB_ChangeRules(Dictionary<int, string> rules, List<string> messages, bool output) {
            int answer = 0;

            rules[8] = "42+";
            rules[11] = "42 31 | 42 42 31 31 | 42 42 42 31 31 31 | 42 42 42 42 31 31 31 31";
            //Really dumb, but it worked...

            StringBuilder sbB = new StringBuilder();
            Expand(sbB, rules, rules[0]);
            string patternB = string.Format("^({0})$", sbB.ToString());

            if (output) {
                Console.WriteLine("0: " + patternB);
                Console.WriteLine();
            }

            foreach (string msg in messages) {
                Match matchB = Regex.Match(msg, patternB);
                if (matchB.Success) {
                    answer++;
                    if (output)
                        Console.WriteLine(msg);
                }
            }

            if (output)
                Console.WriteLine();

            return answer;
        }

        private static int SolveB_Chunks(Dictionary<int, string> rules, List<string> messages, bool output) {
            int answer = 0;

            StringBuilder sb42 = new StringBuilder();
            StringBuilder sb31 = new StringBuilder();
            Expand(sb42, rules, rules[42]);
            Expand(sb31, rules, rules[31]);
            string pattern42 = string.Format("^({0})$", sb42.ToString());
            string pattern31 = string.Format("^({0})$", sb31.ToString());

            if (output) {
                Console.WriteLine("42: " + pattern42);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("31: " + pattern31);
                Console.ResetColor();
                Console.WriteLine();
            }

            int numBase = 2;
            int bitplaces = (messages.Count < 20 ? 5 : 8);
            int possible = (int)Math.Pow(numBase, bitplaces);
            List<string> matches42 = new List<string>();
            List<string> matches31 = new List<string>();
            for (int ops = 0; ops < possible; ops++) {
                string mask = Shared.Number.IntToString(ops, numBase);
                mask = mask.Replace('0', 'a').Replace('1', 'b').PadLeft(bitplaces, 'a');

                Match m42 = Regex.Match(mask, pattern42);
                Match m31 = Regex.Match(mask, pattern31);
                if (m42.Success)
                    matches42.Add(mask);
                if (m31.Success)
                    matches31.Add(mask);

                //With my input, there wasn't anything in both lists.
            }

            foreach (string msg in messages) {
                List<string> chunks = msg.Chunk(bitplaces).Select(x => new string(x)).ToList();
                bool[] is31 = new bool[chunks.Count];

                bool valid = true;
                int num42 = 0;
                int num31 = 0;

                for (int c = 0; c < chunks.Count; c++) {
                    if (matches42.Contains(chunks[c])) {
                        num42++;
                        if (num31 > 0)
                            valid = false;
                    }
                    if (matches31.Contains(chunks[c])) {
                        num31++;
                        is31[c] = true;
                    }
                }

                if (valid && num42 > num31 && num31 > 0) {
                    answer++;
                    if (output) {
                        //Console.WriteLine(msg);
                        for (int c = 0; c < chunks.Count; c++) {
                            if (is31[c])
                                Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(chunks[c]);
                        }
                        Console.ResetColor();
                        Console.WriteLine();
                    }
                }
            }

            if (output)
                Console.WriteLine();

            return answer;
        }

        private static void Expand(StringBuilder sb, Dictionary<int, string> rules, string outer) {
            string[] o = outer.Split(' ');
            foreach (string s in o) {
                if (s == "a" || s == "b" || s == "|")
                    sb.Append(s);
                else {
                    if(s.Contains('?')) {
                        sb.Append("?");
                        continue;
                    }

                    bool plus = s.Contains('+');
                    int n;
                    if (plus)
                        n = int.Parse(s.Substring(0, s.Length - 1));
                    else
                        n = int.Parse(s);
                    string match = rules[n];

                    if (match.Contains('|')) {
                        sb.Append("(");
                        Expand(sb, rules, match);
                        sb.Append(")");
                    } else {
                        Expand(sb, rules, match);
                    }

                    if (plus)
                        sb.Append("+");
                }
            }
        }
    }
}
