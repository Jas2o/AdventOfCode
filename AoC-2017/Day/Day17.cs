namespace AoC.Day
{
    public class Day17
    {
        public static void Run(string file) {
            Console.WriteLine("Day 17: Spinlock" + Environment.NewLine);

			string input = File.ReadAllText(file);
			int spinsPerInsert = int.Parse(input);

            List<int> buffer = new List<int>() { 0 };
            int pos = 0;

            int numOfInsertions = 2017;
            for (int i = 1; i <= numOfInsertions; i++) {
                pos = ((pos + spinsPerInsert) % i) + 1;
                buffer.Insert(pos, i);
            }
            int indexLastInsert = buffer.IndexOf(numOfInsertions);
            int part1 = buffer[indexLastInsert + 1];

            int numOfInsertions2 = 50000000;
            int part2 = 0;
            for (int i = numOfInsertions + 1; i <= numOfInsertions2; i++) {
                pos = ((pos + spinsPerInsert) % i) + 1;
                if (pos == 1)
                    part2 = i;
                //Since 0 is always in the first position, we just need to remember what was put after it.
            }

            Console.WriteLine("Part 1: " + part1);
            //Answer: 808
            Console.WriteLine("Part 2: " + part2);
            //Answer: 47465686
        }
    }
}
