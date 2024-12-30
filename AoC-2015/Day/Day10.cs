using System.Text;

namespace AoC.Day {
    public class Day10
    {
        public static void Run(string file) {
            Console.WriteLine("Day 10: Elves Look, Elves Say" + Environment.NewLine);

            string line = File.ReadAllText(file);
            Console.WriteLine("{0} => {1}", 0, line);
            int lenA = 0;
            for (int i = 1; i <= 50; i++) {
                line = LookAndSay(line);
                if(i > 9)
                    Console.Write("{0} ", i);
                else
                    Console.WriteLine("{0} => {1}", i, line);

                if (i == 40)
                    lenA = line.Length;
            }
            Console.WriteLine("\r\n");

            int lenB = line.Length;

            Console.WriteLine("Part 1: " + lenA);
            //Answer: 360154
            Console.WriteLine("Part 2: " + lenB);
            //Answer: 
        }

        public static string LookAndSay(string line) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < line.Length; i++) {
                char c = line[i];

                int count = 1;
                for (int peek = i + 1; peek < line.Length; peek++) {
                    char p = line[peek];
                    if (p == c) {
                        count++;
                    } else
                        break;
                }

                sb.Append(count);
                sb.Append(c);

                i += count - 1;
            }

            return sb.ToString();
        }
    }
}
