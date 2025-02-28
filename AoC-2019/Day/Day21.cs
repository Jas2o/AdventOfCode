using System.Text;

namespace AoC.Day
{
    public class Day21
    {
        //In future I should consider revisiting this to solve it properly (or a memory hack way).

        public static void Run(string file) {
            Console.WriteLine("Day 21: Springdroid Adventure" + Environment.NewLine);

            string input = File.ReadAllText(file);
            if (input.IndexOf(',') == -1) {
                Console.WriteLine("There is no test for this day.");
                return;
            }
            long[] initial = Array.ConvertAll(input.Split(','), long.Parse);

            // No I did not come up with either of these...
            string[] cmdsA = [
                "NOT C J",
                "AND D J",
                "NOT A T",
                "OR T J",
                "WALK"
            ];
            string[] cmdsB = [
                "NOT C J",
                "AND H J",
                "NOT B T",
                "OR T J",
                "NOT A T",
                "OR T J",
                "AND D J",
                "RUN"
            ];

            long partA = Solve(initial, cmdsA);
            long partB = Solve(initial, cmdsB);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 19362822
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1143625214
        }

        private static long Solve(long[] initial, string[] cmds) {
            if (initial.Length == 0)
                return 0;

            long result = 0;
            IntCode computer = new IntCode(initial, []);
            foreach (string cmd in cmds) {
                foreach (char c in cmd)
                    computer.inputQueue.Enqueue(c);
                computer.inputQueue.Enqueue('\n');
            }
            computer.Run();

            StringBuilder sb = new StringBuilder();
            while (computer.outputQueue.Any()) {
                long value = computer.outputQueue.Dequeue();
                if (value < 128)
                    sb.Append((char)value);
                else {
                    result = value;
                    break;
                }
            }

            string[] lines = sb.ToString().Split("\n");
            foreach (string line in lines) {
                if (line.Length == 0 || line.StartsWith("Input "))
                    continue;
                Console.WriteLine(line);
            }
            Console.WriteLine();

            return result;
        }
    }
}
