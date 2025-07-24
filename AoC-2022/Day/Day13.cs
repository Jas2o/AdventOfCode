using System.Text;

namespace AoC.Day
{
    public class Day13
    {
        //This was hell, there was multiple rewrites.

        public static void Run(string file) {
            Console.WriteLine("Day 13: Distress Signal" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            int partA = SolveA(lines);
            int partB = SolveB(lines);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 5003
            Console.WriteLine("Part 2: " + partB);
            //Answer: 20280
        }

        private static char[] tokens = [',', '[', ']'];

        private static Nested Convert(string text) {
            //Step 1: create a mapping of the depth, tokens or numbers (digits combined).
            List<(int depth, object)> map = new List<(int depth, object)>();
            int indent = 0;
            StringBuilder inProg = new StringBuilder();
            for (int i = 0; i < text.Length; i++) {
                char c = text[i];
                if (tokens.Contains(c)) {
                    if (inProg.Length > 0) {
                        int num = int.Parse(inProg.ToString());
                        map.Add((indent, num));
                        inProg.Clear();
                    }
                    if (c == '[')
                        indent++;
                    map.Add((indent, c));
                    if (c == ']')
                        indent--;
                } else {
                    inProg.Append(c);
                }
            }
            if (inProg.Length > 0) {
                int num = int.Parse(inProg.ToString());
                map.Add((indent, num));
                inProg.Clear();
            }

            //Step 2: change that mapping into nested objects.
            while (true) {
                int min = map.Min(m => m.depth);
                int max = map.Max(m => m.depth);

                if (min == max) {
                    Nested n = new Nested(map);
                    return n;
                } else {
                    List<int> indexes = new List<int>();
                    List<(int depth, object)> sub = new List<(int depth, object)>();

                    for (int i = 0; i < map.Count; i++) {
                        (int depth, object) here = map[i];
                        if (here.depth == max) {
                            indexes.Add(i);
                            sub.Add(here);
                        } else if (indexes.Count > 0)
                            break;
                    }

                    int replaceIndex = indexes.First();
                    (int depth, object) replace = map[replaceIndex];
                    replace.depth = replace.depth - 1;
                    replace.Item2 = new Nested(sub);
                    map[replaceIndex] = replace;

                    for (int c = 1; c < indexes.Count; c++)
                        map.RemoveAt(replaceIndex + 1);
                }
            }
        }

        private static int Compare(Nested left, Nested right) {
            return Compare(left, right, false);
        }

        private static int Compare(Nested left, Nested right, bool displayText) {
            int r = 0;

            if (right.List.Count == 0) {
                if (left.List.Count == 0)
                    return 0;
                if (left.List.Count > 0)
                    return 1;
            } else if (left.List.Count == 0) {
                return -1;
            }

            if(displayText)
                Console.WriteLine("- Compare {0} vs {1}", left.Preview, right.Preview);

            int max = Math.Max(left.List.Count, right.List.Count);

            for (int i = 0; i <= max; i++) {
                if (i == left.List.Count) {
                    if (i == right.List.Count) {
                        if (displayText)
                            Console.WriteLine("~ Are equal.");
                        return 0;
                    }
                    if (displayText)
                        Console.WriteLine("~ Left side ran out of items.");
                    return -1;
                }
                if (i == right.List.Count) {
                    if (displayText)
                        Console.WriteLine("~ Right side ran out of items.");
                    return 1;
                }

                object cL = left.List[i];
                object cR = right.List[i];

                if (cL is int && cR is int) {
                    int numL = (int)cL;
                    int numR = (int)cR;
                    if (displayText)
                        Console.WriteLine("# Compare {0} vs {1}", numL, numR);

                    if (numL < numR)
                        return -1;
                    else if (numL > numR)
                        return 1;
                    else
                        continue;
                }

                if (cL is Nested && !(cR is Nested)) {
                    if (displayText) {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("# Convert right");
                        Console.ResetColor();
                    }
                    cR = new Nested((int)cR);
                } else if (cR is Nested && !(cL is Nested)) {
                    if (displayText) {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("# Convert left");
                        Console.ResetColor();
                    }
                    cL = new Nested((int)cL);
                }

                r = Compare((Nested)cL, (Nested)cR, displayText);
                if (r != 0)
                    return r;
            }

            return r;
        }

        private static int SolveA(string[] lines) {
            Queue<string> queue = new Queue<string>(lines);
            queue.Enqueue("");

            List<int> indices = new List<int>();
            int num = 0;
            while (queue.Any()) {
                num++;
                Console.WriteLine("== Pair {0} ==", num);

                string left = queue.Dequeue();
                string right = queue.Dequeue();
                Nested nestedLeft = Convert(left);
                Nested nestedRight = Convert(right);

                int r = Compare(nestedLeft, nestedRight, true);
                if (r == -1) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Correct");
                    Console.ResetColor();
                    indices.Add(num);
                } else if (r == 1) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong");
                    Console.ResetColor();
                }

                Console.WriteLine();
                queue.Dequeue(); //Blank
            }

            foreach (var packet in indices)
                Console.WriteLine(packet);
            Console.WriteLine();

            return indices.Sum();
        }

        private static int SolveB(string[] lines) {
            string two = "[[2]]";
            string six = "[[6]]";
            List<string> list = new List<string>(lines);
            list.Add(two);
            list.Add(six);

            List<Nested> packets = new List<Nested>();
            foreach (string item in list) {
                if (item.Length == 0)
                    continue;
                Nested nested = Convert(item);
                packets.Add(nested);
            }

            packets.Sort(Compare);

            foreach(Nested packet in packets)
                Console.WriteLine(packet.Preview);
            Console.WriteLine();

            int idxTwo = packets.IndexOf(packets.Find(n => n.Preview == two)) + 1;
            int idxSix = packets.IndexOf(packets.Find(n => n.Preview == six)) + 1;
            int result = idxTwo * idxSix;
            return result;
        }

        private class Nested {
            public List<object> List;
            public string Preview;

            public Nested(List<(int depth, object)> map) {
                List = new List<object>();
                foreach(var thing in map) {
                    if (thing.Item2 is int || thing.Item2 is Nested)
                        List.Add(thing.Item2);
                }

                List<string> previewParts = new List<string>();

                foreach(var thing in List) {
                    if (thing is Nested)
                        previewParts.Add(((Nested)thing).Preview);
                    else
                        previewParts.Add(thing.ToString());
                }

                Preview = string.Format("[{0}]", string.Join(',', previewParts));
            }

            public Nested(int alone) {
                List = new List<object>() { alone };
                Preview = string.Format("[{0}]", alone);
            }

            public override string ToString() {
                return Preview;
            }
        }
    }
}
