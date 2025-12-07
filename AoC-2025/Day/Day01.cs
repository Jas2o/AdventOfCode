namespace AoC.Day
{
    public class Day01
    {
        public static void Run(string file) {
            Console.WriteLine("Day 1: Secret Entrance" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            int partA = 0;
            int partB = 0;
            int dial_before = 50;
            int dial_after = dial_before;
            char dir = '?';
            int rotate = 0;
            int pass = 0;
            Console.WriteLine("The dial starts by pointing at {0}.", dial_before);
            foreach (string line in lines) {
                dir = line[0];
                rotate = int.Parse(line.Substring(1));
                pass = 0;

                if(dir == 'L')
                    dial_after -= rotate;
                else if(dir == 'R')
                    dial_after += rotate;

                if(dial_after < 0) {
                    if (dial_before == 0)
                        pass--;
                    while(dial_after < 0) {
                        dial_after += 100;
                        pass++;
                    }
                } else if(dial_after > 99) {
                    while (dial_after > 99) {
                        dial_after -= 100;
                        pass++;
                    }
                    if (dial_after == 0)
                        pass--;
                }

                if (dial_after == 0) {
                    partA++;
                    partB++;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                Console.WriteLine("The dial is rotated {0} to point at {1}.", line, dial_after);

                if (pass > 0) {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Passed {0} times.", pass);
                    partB += pass;
                }
                Console.ResetColor();

                dial_before = dial_after;
            }

			Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 1152
            Console.WriteLine("Part 2: " + partB);
            //Answer: 6671
        }
    }
}
