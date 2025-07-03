namespace AoC.Day
{
    public class Day05
    {
        public static void Run(string file) {
            Console.WriteLine("Day 5: Supply Stacks" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            //Setup
            int iBlank = Array.IndexOf(lines, "");
            int[] nums = Array.ConvertAll(lines[iBlank - 1].Split(' ', StringSplitOptions.RemoveEmptyEntries), int.Parse);
            Dictionary<int, Stack<char>> stacksA = new Dictionary<int, Stack<char>>();
            Dictionary<int, Stack<char>> stacksB = new Dictionary<int, Stack<char>>();
            foreach (int n in nums) {
                stacksA[n] = new Stack<char>();
                stacksB[n] = new Stack<char>();
            }
            for (int i = iBlank - 2; i >= 0; i--) {
                string line = lines[i];
                int pos = 0;
                for(int k = 1; k < line.Length; k += 4) {
                    pos++;
                    char c = line[k];
                    if (c != ' ') {
                        stacksA[pos].Push(c);
                        stacksB[pos].Push(c);
                    }
                }
            }

            DisplayAndGetTops(stacksA, nums, "Before:");

            //Do moves
            for (int i = iBlank + 1; i < lines.Length; i++) {
                string[] fields = lines[i].Split(' ');
                int count = int.Parse(fields[1]);
                int stackSource = int.Parse(fields[3]);
                int stackDest = int.Parse(fields[5]);

                char[] moveB = stacksB[stackSource].Take(count).Reverse().ToArray();
                for (int current = 0; current < count; current++) {
                    char c = stacksA[stackSource].Pop();
                    stacksA[stackDest].Push(c);

                    stacksB[stackSource].Pop();
                    stacksB[stackDest].Push(moveB[current]);
                }
            }

            string partA = DisplayAndGetTops(stacksA, nums, "Part 1:");
            string partB = DisplayAndGetTops(stacksB, nums, "Part 2:");

			Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: ZSQVCCJLL
            Console.WriteLine("Part 2: " + partB);
            //Answer: QZFJRWHGS
        }

        private static string DisplayAndGetTops(Dictionary<int, Stack<char>> stacks, int[] nums, string heading) {
            List<char> top = new List<char>();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(heading);
            Console.ResetColor();
            foreach (int n in nums) {
                string str = string.Join(',', stacks[n].Reverse());
                Console.WriteLine("{0} => {1}", n, str);
                top.Add(stacks[n].Peek());
            }
            Console.WriteLine();

            return new string(top.ToArray());
        }
    }
}
