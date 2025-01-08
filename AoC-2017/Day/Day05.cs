namespace AoC.Day
{
    public class Day05
    {
        public static void Run(string file) {
            Console.WriteLine("Day 5: A Maze of Twisty Trampolines, All Alike" + Environment.NewLine);

            List<int> nums = new List<int>();
			string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                nums.Add(int.Parse(line));
            }

            int steps_A = 0;
            int pointer = 0;
            List<int> temp = nums.ToList();
            while (true) {
                if (pointer >= temp.Count)
                    break;
                int offset = temp[pointer];
                temp[pointer]++;
                pointer += offset;
                steps_A++;
            }

            int steps_B = 0;
            pointer = 0;
            temp = nums.ToList();
            while (true) {
                if (pointer >= temp.Count)
                    break;
                int offset = temp[pointer];
                if (offset >= 3)
                    temp[pointer]--;
                else
                    temp[pointer]++;
                pointer += offset;
                steps_B++;
            }

            Console.WriteLine("Part 1: " + steps_A);
            //Answer: 373543
            Console.WriteLine("Part 2: " + steps_B);
            //Answer: 27502966
        }
    }
}
