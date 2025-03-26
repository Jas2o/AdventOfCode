namespace AoC.Day
{
    public class Day03
    {
        public static void Run(string file) {
            Console.WriteLine("Day 3: Toboggan Trajectory" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            int partA = TestSlope(lines, 3, 1);

            int[] partB_tests = new int[] {
                TestSlope(lines, 1, 1),
                partA,
                TestSlope(lines, 5, 1),
                TestSlope(lines, 7, 1),
                TestSlope(lines, 1, 2)
            };
            
            long partB = partB_tests[0];
            for (int t = 0; t < partB_tests.Length; t++) {
                Console.WriteLine(partB_tests[t]);
                if(t > 0)
                    partB *= partB_tests[t];
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 240
            Console.WriteLine("Part 2: " + partB);
            //Answer: 2832009600
        }

        private static int TestSlope(string[] lines, int r, int d) {
            int result = 0;
            int width = lines[0].Length;
            int right = 0;
            for (int down = d; down < lines.Length; down += d) {
                right = (right + r) % width;

                char hit = lines[down][right];
                if (hit == '#')
                    result++;
            }
            return result;
        }
    }
}
