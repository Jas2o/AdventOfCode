namespace AoC.Day
{
    public class Day01
    {
        public static void Run(string file) {
            Console.WriteLine("Day 1: Inverse Captcha" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                //There's only one line in the real input.
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(line);
                Console.ResetColor();

                int sumA = 0;
                int sumB = 0;
                int offset = (line.Length / 2);
                int prev = (int)char.GetNumericValue(line.Last());
                for (int i = 0; i < line.Length; i++) {
                    int current = (int)char.GetNumericValue(line[i]);
                    int other = (int)char.GetNumericValue(line[(i + offset) % line.Length]);

                    if (current == prev)
                        sumA += current;
                    if (current == other)
                        sumB += current;

                    prev = current;
                }

                Console.WriteLine();
                Console.WriteLine("Part 1: " + sumA);
                //Answer: 1171
                Console.WriteLine("Part 2: " + sumB);
                //Answer: 1024

                Console.WriteLine();
            }
        }
    }
}
