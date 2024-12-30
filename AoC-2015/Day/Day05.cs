namespace AoC.Day {
    public class Day05
    {
        public static void Run(string file) {
            Console.WriteLine("Day 5: Doesn't He Have Intern-Elves For This?" + Environment.NewLine);

            List<string> badSubs = new List<string>() {
                "ab", "cd", "pq", "xy"
            };

            int countNiceA = 0;
            int countNiceB = 0;

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                //Part 1
                int a = line.Count(x => x == 'a');
                int e = line.Count(x => x == 'e');
                int i = line.Count(x => x == 'i');
                int o = line.Count(x => x == 'o');
                int u = line.Count(x => x == 'u');

                int vowels = a + e + i + o + u;

                bool requirement1 = (vowels >= 3);
                bool requirement2 = false;
                bool requirement3 = !badSubs.Any(line.Contains);

                bool requirement4 = false;
                bool requirement5 = false;

                char prev = line[0];
                for(int pos = 1; pos < line.Length; pos++) {
                    char current = line[pos];
                    if(prev == current)
                        requirement2 = true;

                    if (line.Substring(pos + 1).Contains(string.Format("{0}{1}", prev, current)))
                        requirement4 = true;

                    if ((pos < line.Length - 1) && prev == line[pos + 1])
                        requirement5 = true;

                    prev = current;
                }
                
                //--

                Console.Write(line + " is (1) ");
                if (requirement1 && requirement2 && requirement3) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Nice");
                    countNiceA++;
                } else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Naughty");
                }
                Console.ResetColor();
                Console.Write(" or (2) ");
                if (requirement4 && requirement5) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Nice");
                    countNiceB++;
                } else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Naughty");
                }

                Console.ResetColor();
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + countNiceA);
            //Answer: 255
            Console.WriteLine("Part 2: " + countNiceB);
            //Answer: 55
        }
    }
}
