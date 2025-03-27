namespace AoC.Day
{
    public class Day08
    {
        public static void Run(string file) {
            Console.WriteLine("Day 8: Handheld Halting" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            TryRun(lines, out int partA);
            int partB = -1;
            while(partB < lines.Length) {
                partB++;
                string lineBefore = lines[partB];
                if (lineBefore.StartsWith("acc"))
                    continue;
                string line = lineBefore;
                if (line.StartsWith("nop"))
                    line = line.Replace("nop", "jmp");
                else if (line.StartsWith("jmp"))
                    line = line.Replace("jmp", "nop");

                lines[partB] = line;
                if (TryRun(lines, out int acc)) {
                    Console.WriteLine("Line #{0}: [{1}] to [{2}]", partB, lineBefore, line);
                    partB = acc;
                    break;
                }
                lines[partB] = lineBefore;
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 1727
            Console.WriteLine("Part 2: " + partB);
            //Answer: 552
        }

        private static bool TryRun(string[] lines, out int accumulator) {
            int instructionPointer = 0;
            accumulator = 0;

            Dictionary<int, bool> visited = new Dictionary<int, bool>();
            while (instructionPointer < lines.Length) {
                bool added = visited.TryAdd(instructionPointer, false);
                if (!added)
                    return false;

                string line = lines[instructionPointer];
                string op = line.Substring(0, 3);
                char sign = line[4];
                int num = int.Parse(line.Substring(5));
                if (sign == '-')
                    num *= -1;

                switch (op) {
                    case "acc":
                        instructionPointer++;
                        accumulator += num;
                        break;
                    case "jmp":
                        instructionPointer += num;
                        break;
                    case "nop":
                        instructionPointer++;
                        break;
                }
            }

            return true;
        }
    }
}
