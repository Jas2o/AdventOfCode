namespace AoC.Day
{
    public class Day25
    {
        public static void Run(string file) {
            Console.WriteLine("Day 25: Combo Breaker" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            int cardKey = int.Parse(lines[0]);
            int doorKey = int.Parse(lines[1]);
            int doorLoop = FindLoop(doorKey);
            int cardLoop = FindLoop(cardKey);

            long partA = Transform(cardKey, doorLoop);
            //long partA = Transform(door, cardLoop);

            Console.WriteLine("Card key: {0}, loop: {1}", cardKey, cardLoop);
            Console.WriteLine("Door key: {0}, loop: {1}\r\n", doorKey, doorLoop);
            Console.WriteLine("Part 1: " + partA);
            //Answer: 7032853
            Console.WriteLine("Part 2: (N/A)");
			//Answer: (there is no Part 2).
        }

        public static int FindLoop(int key) {
            int loop = 0;
            int subject = 7;
            long value = 1;
            while(true) {
                loop++;
                value *= subject;
                value %= 20201227;

                if (value == key)
                    return loop;
            }
        }

        private static long Transform(int subject, int loops) {
            long value = 1;
            for (int current = 0; current < loops; current++) {
                value *= subject;
                value %= 20201227;
            }
            return value;
        }
    }
}
