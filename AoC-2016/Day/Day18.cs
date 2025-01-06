using System.Text;

namespace AoC.Day
{
    public class Day18
    {
        public static void Run(string file) {
            Console.WriteLine("Day 18: Like a Rogue" + Environment.NewLine);

            string firstLine = File.ReadAllText(file);
            int rows = (firstLine.Length == 10 ? 10 : 40);

            int partA = Solve(firstLine, rows, true);
            int partB = Solve(firstLine, 400000, false);

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 1951
            Console.WriteLine("Part 2: " + partB);
            //Answer: 20002936
        }

        private static int Solve(string firstLine, int rows, bool print) {
            if(print)
                Console.WriteLine(firstLine);

            int safe = firstLine.Replace("^", "").Length;
            string lastLine = firstLine;

            for (int row = 1; row < rows; row++) {
                StringBuilder sb = new StringBuilder();

                for (int col = 0; col < firstLine.Length; col++) {
                    bool left = (col == 0 ? '.' : lastLine[col - 1]) == '^';
                    bool center = lastLine[col] == '^';
                    bool right = (col + 1 == firstLine.Length ? '.' : lastLine[col + 1]) == '^';

                    bool rule1 = (left && center && !right);
                    bool rule2 = (!left && center && right);
                    bool rule3 = (left && !center && !right);
                    bool rule4 = (!left && !center && right);

                    if (rule1 || rule2 || rule3 || rule4)
                        sb.Append('^');
                    else
                        sb.Append('.');
                }

                lastLine = sb.ToString();
                safe += lastLine.Replace("^", "").Length;
                if(print)
                    Console.WriteLine(lastLine);
            }

            return safe;
        }
    }
}
