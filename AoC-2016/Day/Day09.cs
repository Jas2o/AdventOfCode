using System.Text;

namespace AoC.Day {
    public class Day09
    {
        public static void Run(string file) {
            Console.WriteLine("Day 9: Explosives in Cyberspace" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            //The real input only has a single line.
            foreach (string input in lines) {
                StringBuilder decompressed = new StringBuilder();
                long part2 = 0;
                Queue<char> queue = MakeQueue(input);
                ProcessQueue(queue, decompressed, ref part2);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(input);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(decompressed.ToString());
                Console.ResetColor();
                Console.WriteLine();

                Console.WriteLine("Part 1: " + decompressed.Length);
                //Answer: 102239
                Console.WriteLine("Part 2: " + part2);
                //Answer: 10780403063

                Console.WriteLine();
            }
        }

        private static Queue<char> MakeQueue(string input) {
            Queue<char> queue = new Queue<char>();
            foreach (char c in input)
                queue.Enqueue(c);

            return queue;
        }

        private static void ProcessQueue(Queue<char> queue, StringBuilder decompressed, ref long part2) {
            while (queue.Any()) {
                char c = queue.Dequeue();

                if (c == '(') {
                    StringBuilder code = new StringBuilder();
                    c = queue.Dequeue();
                    do {
                        code.Append(c);
                        c = queue.Dequeue();
                    } while (c != ')');
                    string coded = code.ToString();
                    int x = coded.IndexOf('x');
                    int width = int.Parse(coded.Substring(0, x));
                    int times = int.Parse(coded.Substring(x + 1));

                    StringBuilder section = new StringBuilder();
                    for (int i = 0; i < width; i++)
                        section.Append(queue.Dequeue());
                    string s = section.ToString();
                    if (decompressed != null) {
                        for (int i = 0; i < times; i++) {
                            decompressed.Append(s);
                        }
                    }

                    //Part 2
                    if (s.Contains('(')) {
                        Queue<char> subqueue = MakeQueue(s);
                        long sublen = 0;
                        ProcessQueue(subqueue, null, ref sublen);
                        part2 += (sublen * times);
                    } else {
                        part2 += (s.Length * times);
                    }
                } else {
                    if (decompressed != null)
                        decompressed.Append(c);
                    part2++;
                }
            }
        }
    }
}
