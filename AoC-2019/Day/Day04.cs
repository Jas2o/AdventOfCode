namespace AoC.Day {
    public class Day04
    {
        public static void Run(string file) {
            Console.WriteLine("Day 4: Secure Container" + Environment.NewLine);

			string input = File.ReadAllText(file);
            if (!input.Contains('-')) {
                Console.WriteLine("There is no test for this day.");
                return;
            }
            int[] range = Array.ConvertAll(input.Split('-'), int.Parse);

            int partA = 0;
            int partB = 0;
            for (int n = range[0]; n < range[1]; n++) {
                string inputN = n.ToString();

                bool hasDouble = false;
                bool hasDoubleNotPartOfLarger = false;
                bool hasDecrease = false;
                int previous = -1;

                for (int i = 0; i < inputN.Length; i++) {
                    int num = (int)char.GetNumericValue(inputN[i]);

                    if (previous == num) {
                        hasDouble = true;
                        string triple = string.Format("{0}{0}{0}", num);
                        if (!inputN.Contains(triple))
                            hasDoubleNotPartOfLarger = true;
                    }
                    if (num < previous)
                        hasDecrease = true;

                    previous = num;
                }

                if (!hasDecrease) {
                    if(hasDouble)
                        partA++;
                    if (hasDoubleNotPartOfLarger)
                        partB++;
                }
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 1729
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1172
        }
    }
}
