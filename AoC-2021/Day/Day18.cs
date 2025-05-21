using System.Text;

namespace AoC.Day
{
    public class Day18
    {
        //Ugly, but it got the job done.

        public static void Run(string file) {
            Console.WriteLine("Day 18: Snailfish" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            //Part 1
            bool outputText = true;
            SnailNumber running = new SnailNumber(lines[0], null);
            if (outputText)
                WriteOutString("  ", running);
            for (int i = 1; i < lines.Length; i++) {
                SnailNumber current = new SnailNumber(lines[i], null);
                if (outputText)
                    WriteOutString("+ ", current);
                running = new SnailNumber(running, current);
                Reduce(ref running, outputText);
                if (outputText)
                    WriteOutString("= ", running);
            }
            if (outputText)
                Console.WriteLine();
            int partA = Magnitude(running);

            //Part 2
            int partB = 0;
            for (int i = 0; i < lines.Length; i++) {
                for (int k = 0; k < lines.Length; k++) {
                    if (i == k)
                        continue;
                    SnailNumber first = new SnailNumber(lines[i], null);
                    SnailNumber second = new SnailNumber(lines[k], null);
                    SnailNumber trial = new SnailNumber(first, second);
                    Reduce(ref trial, false);
                    int result = Magnitude(trial);
                    if (result > partB) {
                        Console.WriteLine("{0} and {1} => {2}", i, k, result);
                        partB = result;
                    }
                }
            }
            Console.WriteLine();

            Console.WriteLine("Part 1: " + partA);
            //Answer: 4088
            Console.WriteLine("Part 2: " + partB);
            //Answer: 4536
        }

        private static void Reduce(ref SnailNumber running, bool outputText) {
            while (true) {
                SnailNumber nested = FindNested(running, 0);
                SnailNumber ten = FindTenOrGreater(running);
                if (nested == null && ten == null)
                    break;

                if (nested != null) {
                    //If any pair is nested inside four pairs, the leftmost such pair explodes.
                    nested.Marked = true;
                    List<string> parts = SnToString(running);
                    nested.Marked = false;

                    //This was easier with string than using the SnailNumbers.

                    //the pair's left value is added to the first regular number to the left of the exploding pair (if any)
                    int nLeft = 0;
                    string sLeft = parts[0];
                    int leftNumIndex = -1;
                    int leftNumLength = 0;
                    int leftNumEnded = -1;
                    for (int k = sLeft.Length - 1; k > -1; k--) {
                        if (char.IsNumber(sLeft[k])) {
                            if (leftNumEnded == -1) {
                                leftNumEnded = k + 1;
                                leftNumLength = 1;
                            } else
                                leftNumLength++;
                        } else if (leftNumLength != 0) {
                            leftNumIndex = k + 1;
                            break;
                        }
                    }
                    if (leftNumLength > 0) {
                        string sLeftLeft = sLeft.Substring(0, leftNumIndex);
                        string sLeftMid = sLeft.Substring(leftNumIndex, leftNumLength);
                        string sLeftRight = sLeft.Substring(leftNumEnded);
                        nLeft = int.Parse(sLeftMid) + nested.XRegular.Value;
                        parts[0] = string.Format("{0}{1}{2}", sLeftLeft, nLeft, sLeftRight);
                    }

                    //the pair's right value is added to the first regular number to the right of the exploding pair (if any)
                    int nRight = 0;
                    string sRight = parts[2];
                    int rightNumIndex = -1;
                    int rightNumLength = 0;
                    int rightNumEnded = -1;
                    for (int k = 0; k < sRight.Length; k++) {
                        if (char.IsNumber(sRight[k])) {
                            if (rightNumIndex == -1) {
                                rightNumIndex = k;
                                rightNumLength = 1;
                            } else
                                rightNumLength++;
                        } else if (rightNumIndex != -1) {
                            rightNumEnded = k;
                            break;
                        }
                    }
                    if (rightNumIndex > -1) {
                        string sRightLeft = sRight.Substring(0, rightNumIndex);
                        string sRightMid = sRight.Substring(rightNumIndex, rightNumLength);
                        string sRightRight = sRight.Substring(rightNumEnded);
                        nRight = int.Parse(sRightMid) + nested.YRegular.Value;
                        parts[2] = string.Format("{0}{1}{2}", sRightLeft, nRight, sRightRight);
                    }

                    //the entire exploding pair is replaced with the regular number 0.
                    parts[1] = "0";
                    string joined = string.Join("", parts);
                    running = new SnailNumber(joined, null);
                    if(outputText)
                        WriteOutString("E ", running);
                    continue;
                }

                if (ten != null) {
                    //If any regular number is 10 or greater, the leftmost such regular number splits.
                    ten.Marked = true;
                    List<string> parts = SnToString(running);
                    ten.Marked = false;

                    if (ten.XRegular >= 10) {
                        int down = (int)Math.Floor(ten.XRegular.Value / 2.0);
                        int up = (int)Math.Ceiling(ten.XRegular.Value / 2.0);
                        ten.XPair = new SnailNumber(down, up, ten);
                        ten.XRegular = null;
                    } else if (ten.YRegular >= 10) {
                        int down = (int)Math.Floor(ten.YRegular.Value / 2.0);
                        int up = (int)Math.Ceiling(ten.YRegular.Value / 2.0);
                        ten.YPair = new SnailNumber(down, up, ten);
                        ten.YRegular = null;
                    }
                    if(outputText)
                        WriteOutString("S ", running);
                    continue;
                }
                break;
            }
        }

        private static void WriteOutString(string prefix, SnailNumber snail) {
            //The flip/color isn't used anymore, but was used when debugging the explode function by marking the nested snail.
            List<string> parts = SnToString(snail);
            Console.Write(prefix);
            bool flip = false;
            foreach (string part in parts) {
                if (flip)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(part);
                if (flip)
                    Console.ResetColor();
                flip = !flip;
            }
            Console.WriteLine();
        }

        private static List<string> SnToString(SnailNumber snail) {
            List<string> parts = new List<string>();
            StringBuilder sb = new StringBuilder();
            SnToStringRecursive(parts, sb, snail);
            parts.Add(sb.ToString());
            return parts;
        }

        private static void SnToStringRecursive(List<string> parts, StringBuilder sb, SnailNumber snail) {
            if(snail.Marked) {
                parts.Add(sb.ToString());
                sb.Clear();
            }
            sb.Append('[');

            if (snail.XPair != null)
                SnToStringRecursive(parts, sb, snail.XPair);
            else
                sb.Append(snail.XRegular);
            sb.Append(',');
            if (snail.YPair != null)
                SnToStringRecursive(parts, sb, snail.YPair);
            else
                sb.Append(snail.YRegular);

            sb.Append(']');
            if (snail.Marked) {
                parts.Add(sb.ToString());
                sb.Clear();
            }
        }

        private static int Magnitude(SnailNumber snail) {
            int left = (snail.XRegular != null ? snail.XRegular.Value : Magnitude(snail.XPair));
            int right = (snail.YRegular != null ? snail.YRegular.Value : Magnitude(snail.YPair));
            left *= 3;
            right *= 2;
            return left + right;
        }

        private static SnailNumber? FindNested(SnailNumber snail, int depth) {
            if (depth == 4)
                return snail;
            if (snail.XPair != null) {
                SnailNumber result = FindNested(snail.XPair, depth + 1);
                if (result != null)
                    return result;
            }
            if (snail.YPair != null)
                return FindNested(snail.YPair, depth + 1);
            return null;
        }

        private static SnailNumber? FindTenOrGreater(SnailNumber snail) {
            if (snail.XRegular >= 10)
                return snail;
            if (snail.XPair != null) {
                SnailNumber? result = FindTenOrGreater(snail.XPair);
                if (result != null)
                    return result;
            }
            if (snail.YPair != null) {
                SnailNumber? result = FindTenOrGreater(snail.YPair);
                if (result != null)
                    return result;
            }
            if (snail.YRegular >= 10)
                return snail;
            return null;
        }

        private class SnailNumber {
            public int? XRegular;
            public int? YRegular;
            public SnailNumber? XPair;
            public SnailNumber? YPair;
            public SnailNumber? ZParent;
            public bool Marked;

            public SnailNumber(SnailNumber x, SnailNumber y) {
                XPair = x;
                YPair = y;
                XPair.ZParent = this;
                YPair.ZParent = this;
            }

            public SnailNumber(int x, int y, SnailNumber? parent) {
                XRegular = x;
                YRegular = y;
                ZParent = parent;
            }

            public SnailNumber(string input, SnailNumber? parent) {
                ZParent = parent;
                int indent = 0;
                Dictionary<int, (int depth, char c)> mapping = new Dictionary<int, (int, char)>();
                for (int i = 0; i < input.Length; i++) {
                    char c = input[i];
                    if (c == '[')
                        indent++;
                    mapping.Add(i, (indent, c));
                    if (c == ']')
                        indent--;
                }

                int max = mapping.Values.Max(d => d.depth);
                if (max > 1) {
                    int firstTwo = mapping.First(m => m.Value.depth == 2).Key;
                    int lastTwo = mapping.Last(m => m.Value.depth == 2).Key;
                    int nextOne = mapping.Skip(firstTwo).First(m => m.Value.depth == 1).Key;

                    string test1 = input.Substring(firstTwo, lastTwo - firstTwo + 1);
                    string test2 = input.Substring(firstTwo, nextOne - firstTwo);
                    string remBefore = input.Substring(1, firstTwo - 1);
                    if (remBefore.Length > 0) {
                        remBefore = remBefore.Substring(0, remBefore.Length - 1);
                        if (remBefore.IndexOf('[') == -1) {
                            XRegular = int.Parse(remBefore);
                            remBefore = string.Empty;
                        }
                    }
                    string remAfter = input.Substring(nextOne + 1);
                    if (remAfter.Length > 0) {
                        remAfter = remAfter.Substring(0, remAfter.Length - 1);
                        if (remAfter.IndexOf('[') == -1) {
                            YRegular = int.Parse(remAfter);
                            remAfter = string.Empty;
                        }
                    }

                    if (nextOne < lastTwo) {
                        if (XRegular == null)
                            XPair = new SnailNumber(test2, this);
                        else
                            throw new Exception();
                    } else {
                        if (XRegular == null)
                            XPair = new SnailNumber(test1, this);
                        else
                            YPair = new SnailNumber(test1, this);
                    }
                    if (YRegular == null && YPair == null)
                        YPair = new SnailNumber(remAfter, this);
                } else {
                    string[] numbers = input.Substring(1, input.Length - 2).Split(',');
                    XRegular = int.Parse(numbers[0]);
                    YRegular = int.Parse(numbers[1]);
                }
            }
        }
    }
}
