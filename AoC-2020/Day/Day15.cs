namespace AoC.Day
{
    public class Day15
    {
        public static void Run(string file) {
            Console.WriteLine("Day 15: Rambunctious Recitation");

			string[] lines = File.ReadAllLines(file); //Test file has multiple

            bool verbose = false;

            foreach (string line in lines) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(Environment.NewLine + line);
                Console.ResetColor();

                int partA = Solve(line, 2020, verbose);
                int partB = Solve(line, 30000000, false);

                Console.WriteLine();
                Console.WriteLine("Part 1: " + partA);
                //Answer: 758
                Console.WriteLine("Part 2: " + partB);
                //Answer: 814
            }
        }

        private class Memory {
            public int Num;
            public int Previous;
            public int Recent;
            public int Count;

            public Memory(int num, int first) {
                Num = num;
                Previous = first;
                Recent = first;
                Count = 1;
            }

            public override string ToString() {
                return string.Format("{0} = {1},{2},{3}", Num, Previous, Recent, Count);
            }
        }

        private static int Solve(string line, int turnCount, bool verbose) {
            int[] nums = Array.ConvertAll(line.Split(','), int.Parse);

            Dictionary<int, Memory> say = new Dictionary<int, Memory>();
            for (int i = 1; i <= nums.Length; i++) {
                int num = nums[i - 1];
                say[num] = new Memory(num, i);
                if (verbose)
                    Console.WriteLine("Turn {0}: say {1}", i, num);
            }

            Memory last = say.MaxBy(s => s.Value.Recent).Value;

            int turn = nums.Length;
            while (turn < turnCount) {
                turn++;

                int newNum = 0;
                if (last.Count != 1)
                    newNum = last.Recent - last.Previous;

                if(say.TryGetValue(newNum, out last)) {
                    last.Previous = last.Recent;
                    last.Recent = turn;
                    last.Count++;
                } else {
                    last = new Memory(newNum, turn);
                    say.Add(newNum, last);
                }

                if (verbose)
                    Console.WriteLine("Turn {0}: say {1}", turn, last.Num);
            }

            return last.Num;
        }
    }
}
