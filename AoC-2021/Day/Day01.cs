namespace AoC.Day
{
    public class Day01
    {
        public static void Run(string file) {
            Console.WriteLine("Day 1: Sonar Sweep" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            int[] nums = Array.ConvertAll(lines, int.Parse);

            //Part 1
            int partA = 0;
            int prev = nums[0];
            foreach(int num in nums) {
                if (num > prev)
                    partA++;
                prev = num;
            }

            //Part 2
            int partB = 0; 
            List<int> moreNums = new List<int>();
            for(int i = 0; i < nums.Length - 2; i++)
                moreNums.Add(nums.Skip(i).Take(3).Sum());
            prev = moreNums[0];
            foreach (int num in moreNums) {
                if (num > prev)
                    partB++;
                prev = num;
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 1559
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1600
        }
    }
}
