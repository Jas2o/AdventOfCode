namespace AoC.Day
{
    public class Day02
    {

        public static void Run(string file) {
            Console.WriteLine("Day 2: Red-Nosed Reports" + Environment.NewLine);

            int numOfSafeReports_Part1 = 0;
            int numOfSafeReports_Part2 = 0;

            StreamReader sr = new StreamReader(file);
            while (!sr.EndOfStream)
            {
                string? line = sr.ReadLine();
                if (line != null)
                {
                    string[] fields = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    List<int> levels = new List<int>();
                    for (int i = 0; i < fields.Length; i++)
                        levels.Add(int.Parse(fields[i]));

                    bool res = Test(levels);
                    if (res)
                        numOfSafeReports_Part1++;
                    else
                    {
                        for (int i = 0; i < fields.Length; i++)
                        {
                            List<int> other = new List<int>(levels);
                            other.RemoveAt(i);
                            res = Test(other);

                            if (res)
                                break;
                        }
                    }

                    if (res)
                        numOfSafeReports_Part2++;
                }
            }

            Console.WriteLine("Part 1: " + numOfSafeReports_Part1);
            //Answer 252
            Console.WriteLine("Part 2: " + numOfSafeReports_Part2);
            //Answer: 324
        }

        private static int[] acceptable = { 1, 2, 3, -1, -2, -3 };

        private static bool Test(List<int> levels)
        {
            bool goingUp = false;
            bool goingDown = false;

            for (int i = 1; i < levels.Count; i++)
            {
                int diff = levels[i] - levels[i - 1];

                if (diff < 0)
                    goingDown = true;
                else if (diff > 0)
                    goingUp = true;

                if (!acceptable.Contains(diff))
                    return false;
            }

            if (goingUp && goingDown)
                return false;
            else if (goingUp || goingDown)
                return true;

            return false;
        }
    }
}
