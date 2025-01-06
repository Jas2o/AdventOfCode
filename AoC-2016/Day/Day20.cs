namespace AoC.Day
{
    public class Day20
    {
        public static void Run(string file) {
            Console.WriteLine("Day 20: Firewall Rules" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            long limit = 4294967295;
            if (lines.Length < 10)
                limit = 9;

            //Fix block ranges
            List<long> blockedStart = new List<long>();
            List<long> blockedEnd = new List<long>();
            foreach (string line in lines) {
                long[] num = Array.ConvertAll(line.Split('-'), long.Parse);
                blockedStart.Add(num[0]);
                blockedEnd.Add(num[1]);
            }
            blockedStart.Sort();
            blockedEnd.Sort();

            //Solve
            long i = 0;
            long partA = 0;
            long partB = 0;
            for (int key = 0; key < blockedStart.Count; key++) {
                if (i < blockedStart[key]) {
                    if (partA == 0)
                        partA = i;
                    partB += blockedStart[key] - i;
                }
                i = Math.Max(i, blockedEnd[key] + 1);
            }
            if (i <= limit) {
                //Remainder
                partB += limit - i + 1;
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 17348574
            Console.WriteLine("Part 2: " + partB);
            //Answer: 104
        }
    }
}
