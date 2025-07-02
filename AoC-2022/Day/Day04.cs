namespace AoC.Day
{
    public class Day04
    {
        public static void Run(string file) {
            Console.WriteLine("Day 4: Camp Cleanup" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            int partA = 0;
            int partB = 0;
            foreach (string line in lines) {
                int[] nums = Array.ConvertAll(line.Replace(',', '-').Split('-'), int.Parse);

                //One side fully contains other
                bool test1 = (nums[0] <= nums[2]);
                bool test2 = (nums[1] >= nums[3]);
                bool test3 = (nums[1] <= nums[3]);
                bool test4 = (nums[0] >= nums[2]);
                if ((test1 && test2) || (test3 && test4))
                    partA++;

                //Any overlap
                bool test5 = (nums[1] >= nums[2]);
                bool test6 = (nums[3] >= nums[0]);
                if (test5 && test6)
                    partB++;
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 532
            Console.WriteLine("Part 2: " + partB);
            //Answer: 854
        }
    }
}
