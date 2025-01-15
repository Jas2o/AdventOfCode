namespace AoC.Day
{
    public class Day01
    {
        public static void Run(string file) {
            Console.WriteLine("Day 1: Chronal Calibration" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            int[] nums = Array.ConvertAll(lines, int.Parse);

            int partA = nums.Sum();

            int partB = 0;
            int freq = 0;
            Dictionary<int, int> dictionary = new Dictionary<int, int>() { { freq, -1 } };
            int n = 0;
            while (true) {
                freq += nums[n % nums.Length];
                if (dictionary.ContainsKey(freq)) {
                    partB = freq;
                    break;
                }
                dictionary.Add(freq, n);
                n++;
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 574
            Console.WriteLine("Part 2: {0} (after {1} changes)", partB, n);
            //Answer: 452
        }
    }
}
