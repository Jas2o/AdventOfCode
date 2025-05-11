namespace AoC.Day
{
    public class Day02
    {
        public static void Run(string file) {
            Console.WriteLine("Day 2: Dive!" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            int partA = SolveA(lines);
            int partB = SolveB(lines);

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 1924923
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1982495697
        }

        private static int SolveA(string[] lines) {
            int position = 0;
            int depth = 0;

            foreach (string line in lines) {
                int space = line.IndexOf(' ');
                string command = line.Substring(0, space);
                int num = int.Parse(line.Substring(space + 1));

                switch (command) {
                    case "forward":
                        position += num;
                        break;
                    case "down":
                        depth += num;
                        break;
                    case "up":
                        depth -= num;
                        break;
                }
            }

            Console.WriteLine("{0},{1}", position, depth);
            return position * depth;
        }

        private static int SolveB(string[] lines) {
            int position = 0;
            int depth = 0;
            int aim = 0;

            foreach (string line in lines) {
                int space = line.IndexOf(' ');
                string command = line.Substring(0, space);
                int num = int.Parse(line.Substring(space + 1));

                switch (command) {
                    case "forward":
                        position += num;
                        depth += (aim * num);
                        break;
                    case "down":
                        aim += num;
                        break;
                    case "up":
                        aim -= num;
                        break;
                }
            }

            Console.WriteLine("{0},{1}", position, depth);
            return position * depth;
        }
    }
}
