namespace AoC.Day
{
    public class Day03
    {
        public static void Run(string file) {
            Console.WriteLine("Day 3: Rucksack Reorganization" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            int count = 0;
            int partA = 0;
            foreach(string line in lines) {
                char[] left = line.Substring(0, line.Length / 2).ToCharArray();
                char[] right = line.Substring(line.Length / 2).ToCharArray();

                IEnumerable<char> both = left.Intersect(right);
                foreach (char c in both) {
                    int num = c - 96;
                    if (num <= 0)
                        num += 58;
                    partA += num;
                    Console.Write("{0}:{1,-2} ", c, num);
                    count++;
                    if (count % 16 == 0)
                        Console.WriteLine();
                }
            }
            Console.WriteLine("\r\n");

            int partB = 0;
            for(int i = 0; i < lines.Length; i += 3) {
                char[] line0 = lines[i].ToCharArray();
                char[] line1 = lines[i + 1].ToCharArray();
                char[] line2 = lines[i + 2].ToCharArray();

                var badge = line0.Intersect(line1).Intersect(line2);
                foreach (char c in badge) {
                    Console.Write(c);
                    int num = c - 96;
                    if (num <= 0)
                        num += 58;
                    partB += num;
                }
            }
            Console.WriteLine("\r\n");

            Console.WriteLine("Part 1: " + partA);
            //Answer: 7826
            Console.WriteLine("Part 2: " + partB);
            //Answer: 2577
        }
    }
}
