using System.Text;

namespace AoC.Day
{
    public class Day10
    {

        public static void Run(string file) {
            Console.WriteLine("Day 10: Knot Hash" + Environment.NewLine);

            string input = File.ReadAllText(file);

            int part1 = Part1(input);
            string part2 = Part2(input);

            Console.WriteLine("Part 1: " + part1);
            //Answer: 6909
            Console.WriteLine("Part 2: " + part2);
            //Answer: 9d5f4561367d379cfbf04f8c471c0095
        }

        private static void Common(List<int> nums, int[] seqLength, ref int currentPos, ref int skipSize) {
            foreach (int seq in seqLength) {
                int[] subsection = new int[seq];
                for (int i = 0; i < seq; i++) {
                    int actualPos = (currentPos + i) % nums.Count;
                    subsection[i] = nums[actualPos];
                }
                Array.Reverse(subsection);
                for (int i = 0; i < seq; i++) {
                    int actualPos = (currentPos + i) % nums.Count;
                    nums[actualPos] = subsection[i];
                }

                currentPos += seq + skipSize;
                skipSize++;
            }
        }

        private static int Part1(string input) {
            //List<int> nums = new List<int>(input.Length == 7 ? Enumerable.Range(0, 5) : Enumerable.Range(0, 256));
            List<int> nums = new List<int>(Enumerable.Range(0, 256));

            if (input.Length > 0 && input.IndexOf(',') > -1) {
                //To support the empty example. Part 1 examples might not work.
                int[] seqLength = Array.ConvertAll(input.Split(','), int.Parse);
                int currentPos = 0;
                int skipSize = 0;
                Common(nums, seqLength, ref currentPos, ref skipSize);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(string.Join(' ', seqLength) + "\r\n");
                Console.ResetColor();
            }

            return nums[0] * nums[1];
        }

        private static string Part2(string input) {
            List<int> nums = new List<int>(Enumerable.Range(0, 256));

            int[] seqLength = new int[input.Length + 5];
            for (int i = 0; i < input.Length; i++) {
                seqLength[i] = (byte)input[i];
            }
            int[] end = [17, 31, 73, 47, 23];
            Array.Copy(end, 0, seqLength, input.Length, end.Length);

            int currentPos = 0;
            int skipSize = 0;
            int rounds = 64;
            for (int r = 0; r < rounds; r++) {
                Common(nums, seqLength, ref currentPos, ref skipSize);
            }

            StringBuilder sb = new StringBuilder();
            if (nums.Count == 256) {
                int offset = 0;
                while (offset < 256) {
                    int previous = 0;
                    for (int i = 0; i < 16; i++) {
                        previous ^= nums[offset + i];
                    }
                    offset += 16;
                    sb.Append(previous.ToString("x2"));
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(string.Join(' ', seqLength) + "\r\n");
            Console.ResetColor();

            return sb.ToString();
        }
    }
}
