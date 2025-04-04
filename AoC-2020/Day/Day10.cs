namespace AoC.Day
{
    public class Day10
    {
        public static void Run(string file) {
            Console.WriteLine("Day 10: Adapter Array" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            List<int> nums = Array.ConvertAll(lines, int.Parse).Order().ToList();
            nums.Add(nums.Max() + 3);

            bool verbose = (lines.Length < 20 ? true : false);

            int aOne = 0;
            int aThree = 0;

            Dictionary<int, int> tribonacci = new Dictionary<int, int>() {
                //{ 1, 1 },//Not needed
                { 2, 2 },
                { 3, 4 },
                { 4, 7 },
                //{ 5, 13 }//Not seen
            };
            Dictionary<int, int> bCount = new Dictionary<int, int>() {
                //{1, 0},//Not needed
                {2, 0},
                {3, 0},
                {4, 0},
                //{5, 0}//Not seen
            };

            int previous = 0;
            List<int> group = new List<int>();
            if (verbose)
                Console.WriteLine("0");
            foreach (int i in nums) {
                int diff = i - previous;
                previous = i;
                //There is no differences of 2 in example or given input.
                if (diff == 1) {
                    aOne++;
                    group.Add(i);
                } else {
                    aThree++;
                    if(tribonacci.ContainsKey(group.Count))
                        bCount[group.Count]++;

                    if (verbose) {
                        //The last number on each line is required.
                        if(group.Count > 0)
                            Console.WriteLine(string.Join(',', group));
                        Console.WriteLine(i);
                    }
                    group.Clear();
                }
            }
            if (verbose)
                Console.WriteLine();

            int partA = aOne * aThree;
            double partB = 1;
            foreach(KeyValuePair<int, int> pair in tribonacci)
                partB *= Math.Pow(pair.Value, bCount[pair.Key]);

            Console.WriteLine("Diffs:");
            Console.WriteLine("1: {0}", aOne);
            Console.WriteLine("3: {0}", aThree);
            Console.WriteLine("\r\nRuns:");
            foreach(KeyValuePair<int, int> c in bCount)
                Console.WriteLine("{0}: {1}", c.Key, c.Value);

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 2346
            Console.WriteLine("Part 2: " + partB);
            //Answer: 6044831973376
        }
    }
}
