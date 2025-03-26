namespace AoC.Day
{
    public class Day02
    {
        public static void Run(string file) {
            Console.WriteLine("Day 2: Password Philosophy" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            int partA = 0;
            int partB = 0;
            foreach (string line in lines) {
                int valid = 0;

                string[] fields = line.Replace(":", "").Split(' ');
                int[] num = Array.ConvertAll(fields[0].Split('-'), int.Parse);

                int lenBefore = fields[2].Length;
                string passWithout = fields[2].Replace(fields[1], "");
                int lenAfter = passWithout.Length;
                int diff = lenBefore - lenAfter;

                if (diff >= num[0] && diff <= num[1]) {
                    valid += 1;
                    partA++;
                }

                char one = fields[2][num[0] - 1];
                char two = fields[2][num[1] - 1];
                char needed = fields[1][0];
                if (one != two) {
                    if(one == needed || two == needed) {
                        valid += 2;
                        partB++;
                    }
                }

                if (valid > 0) {
                    if (valid == 3)
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("{0} = {1}", valid, line);
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 445
            Console.WriteLine("Part 2: " + partB);
            //Answer: 491
        }
    }
}
