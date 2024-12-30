namespace AoC.Day {
    public class Day25
    {
        public static void Run(string file) {
            Console.WriteLine("Day 25: Let It Snow" + Environment.NewLine);

            string text = File.ReadAllText(file);
            if (text.Length < 10) {
                Console.WriteLine("There is no test for this day.");
                return;
            }
            string[] fields = text.Substring(text.IndexOf("row")).Replace(".", "").Replace(",", "").Split(' ');
            int row = int.Parse(fields[1]);
            int column = int.Parse(fields[3]);

            //int limit = CodeIsAfter(6,6); // Pos 60 result would be 27995004
            int limit = CodeIsAfter(row, column);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\"{0}\"\r\n", text.Replace(". ", ".\r\n"));
            Console.ResetColor();

            Console.WriteLine("Code at position: {0} (answer is below)", limit);

            long code = 20151125;
            //Console.WriteLine(code);
            for (int i = 0; i < limit; i++) {
                code = (code * 252533) % 33554393;
                //Console.WriteLine(code);
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + code);
            //Answer: 19980801
            Console.WriteLine("Part 2: (N/A)");
            //Answer: (there is no Part 2).
        }

        private static int CodeIsAfter(int row, int col) {
            if (row == 0 || col == 0)
                throw new Exception();
            int n = row + col - 1;
            int triangularNum = (n * (n + 1)) / 2;
            int pos = triangularNum - row;
            return pos;
        }
    }
}
