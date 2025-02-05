namespace AoC.Day
{
    public class Day01
    {
        public static void Run(string file) {
            Console.WriteLine("Day 1: The Tyranny of the Rocket Equation" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            int[] nums = Array.ConvertAll(lines, int.Parse);

            int partA = 0;
            foreach (int num in nums) {
                int fuel = (num / 3) - 2;
                partA += fuel;
                //Console.WriteLine(fuel);
            }

            //Console.WriteLine();

            int partB = 0;
            foreach (int num in nums) {
                int fuel = (num / 3) - 2;
                partB += fuel;

                while(true) {
                    fuel = (fuel / 3) - 2;
                    if (fuel > 0) {
                        //Console.WriteLine(fuel);
                        partB += fuel;
                    } else
                        break;
                }
            }

			//Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 3353880
            Console.WriteLine("Part 2: " + partB);
            //Answer: 5027950
        }
    }
}
