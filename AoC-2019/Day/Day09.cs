namespace AoC.Day {
    public class Day09
    {
        public static void Run(string file) {
            Console.WriteLine("Day 9: Sensor Boost" + Environment.NewLine);

            string input = File.ReadAllText(file);
            long[] initial = Array.ConvertAll(input.Split(','), long.Parse);

            IntCode computerA = new IntCode(initial, [1]);
            if (!computerA.Run())
                Console.WriteLine("A - Did not complete");
            long partA = computerA.OutputLast;

            IntCode computerB = new IntCode(initial, [2]);
            if (!computerB.Run())
                Console.WriteLine("B - Did not complete");
            long partB = computerB.OutputLast;

            Console.WriteLine("Part 1: " + partA);
            //Answer: 3989758265
            Console.WriteLine("Part 2: " + partB);
            //Answer: 76791
        }
    }
}
