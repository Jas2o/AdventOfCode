namespace AoC.Day {
    public class Day01 {
        public static void Run(string file) {
            Console.WriteLine("Day 1: Historian Hysteria" + Environment.NewLine);

            List<int> listLeft = new List<int>();
            List<int> listRight = new List<int>();

            StreamReader sr = new StreamReader(file);
            while (!sr.EndOfStream) {
                string? line = sr.ReadLine();
                if (line != null) {
                    string[] fields = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    listLeft.Add(int.Parse(fields[0]));
                    listRight.Add(int.Parse(fields[1]));
                }
            }

            listLeft.Sort();
            listRight.Sort();

            //-------
            // Part 1

            List<int> listDistance = new List<int>();
            int total = 0;

            int count = listLeft.Count;
            for (int i = 0; i < count; i++) {
                int left = listLeft[i];
                int right = listRight[i];

                int distance = Math.Abs(left - right);
                listDistance.Add(distance);
                total += distance;
            }

            Console.WriteLine("Part 1 - Total distance: " + total);
            // Answer: 2904518

            //-------
            // Part 2

            int score = 0;

            foreach (int num in listLeft) {
                int appears = listRight.Count(x => x == num);
                int add = num * appears;
                score += add;
            }

            Console.WriteLine("Part 2 - Similarity score: " + score);
            // Answer: 18650129
        }
    }
}
