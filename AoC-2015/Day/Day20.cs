namespace AoC.Day {
    public class Day20
    {
        public static void Run(string file) {
            Console.WriteLine("Day 20: Infinite Elves and Infinite Houses" + Environment.NewLine);

            string input = File.ReadAllText(file);
            int target = int.Parse(input);

            int partA = FindHouseNumThatFirstReachesTarget(target, false);
            int partB = FindHouseNumThatFirstReachesTarget(target, true);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 665280
            Console.WriteLine("Part 2: " + partB);
            //Answer: 705600
        }

        private static int FindHouseNumThatFirstReachesTarget(int target, bool isPart2) {
            int each = (isPart2 ? 11 : 10);
            int[] houses = new int[target / each + 1];

            //It's strangely faster to populate an array and then check it after.

            for (int elf = 1; elf < houses.Length; elf++) {
                int visits = 0;
                for (int house = elf; house < houses.Length; house += elf) {
                    if (isPart2 && visits == 50)
                        break;
                    visits++;
                    houses[house] += elf * each;
                }
            }

            for (int house = 1; house < houses.Length; house++) {
                if (houses[house] >= target) {
                    return house;
                }
            }

            return 0;
        }
    }
}
