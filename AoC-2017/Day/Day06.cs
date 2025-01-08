namespace AoC.Day
{
    public class Day06
    {
        public static void Run(string file) {
            Console.WriteLine("Day 6: Memory Reallocation" + Environment.NewLine);

            string input = File.ReadAllText(file);
            int[] num = Array.ConvertAll(input.Split("\t"), int.Parse);

            List<string> seen = new List<string>();
            seen.Add(string.Join(',', num));

            int steps = 0;
            int steps2 = 0;
            string alreadySeen = string.Empty;
            while (true) {
                int max = num.Max();
                int maxIndex = Array.IndexOf(num, max);
                num[maxIndex] = 0;
                for(int i = 1; i <= max; i++) {
                    int pos = (maxIndex + i) % num.Length;
                    num[pos]++;
                }

                string test = string.Join(',', num);
                if (alreadySeen == string.Empty) {
                    steps++;
                    if (seen.Contains(test)) {
                        alreadySeen = test;
                    }
                } else {
                    steps2++;
                    if (test == alreadySeen) {
                        break;
                    }
                }
                seen.Add(test);
            }

            Console.WriteLine("Part 1: " + steps);
            //Answer: 6681
            Console.WriteLine("Part 2: " + steps2);
            //Answer: 2392
        }
    }
}
