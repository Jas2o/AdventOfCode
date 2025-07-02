namespace AoC.Day
{
    public class Day01
    {
        public static void Run(string file) {
            Console.WriteLine("Day 1: Calorie Counting" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            int total = 0;
            List<int> totals = new List<int>();
            List<string> listLines = new List<string>(lines);
            listLines.Add("");

            foreach (string line in listLines) {
                if (line.Length == 0) {
                    Console.WriteLine(total);
                    totals.Add(total);
                    total = 0;
                } else
                    total += int.Parse(line);
            }

            int partA = totals.Max();
            int partB = totals.OrderDescending().Take(3).Sum();

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 67658
            Console.WriteLine("Part 2: " + partB);
            //Answer: 200158
        }
    }
}
