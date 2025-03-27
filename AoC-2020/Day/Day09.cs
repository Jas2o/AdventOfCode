namespace AoC.Day
{
    public class Day09
    {
        public static void Run(string file) {
            Console.WriteLine("Day 9: Encoding Error" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            long[] nums = Array.ConvertAll(lines, long.Parse);
            int preamble = (nums.Length < 25 ? 5 : 25);

            long partA = 0;
            int end = 0;
            for (int i = preamble; i < nums.Length; i++) {
                bool found = false;
                for (int p1 = i - preamble; p1 < i + preamble; p1++) {
                    if (p1 >= nums.Length)
                        break;
                    for (int p2 = i - preamble + 1; p2 < i + preamble; p2++) {
                        if (p1 == p2)
                            continue;
                        if (p2 >= nums.Length)
                            break;
                        if (nums[p1] + nums[p2] == nums[i]) {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                        break;
                }

                if (!found) {
                    partA = nums[i];
                    end = i;
                    break;
                }
            }

            long partB = FindWeakness(nums, partA, end);

			Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 1124361034
            Console.WriteLine("Part 2: " + partB);
            //Answer: 129444555
        }

        private static long FindWeakness(long[] nums, long match, int end) {
            int length = 0;
            while (length < end) {
                length++;
                for (int start = 0; start < end; start++) {
                    for (int until = 2; until <= length; until++) {
                        IEnumerable<long> contiguous = nums.Skip(start).Take(until);
                        if (contiguous.Sum() == match) {
                            long min = contiguous.Min();
                            long max = contiguous.Max();
                            return min + max;
                        }
                    }
                }
            }
            return 0;
        }
    }
}
