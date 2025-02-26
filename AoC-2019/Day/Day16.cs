namespace AoC.Day
{
    public class Day16
    {
        // My thanks goes to u/paul2718 as I could not optimize for part 2.

        public static void Run(string file) {
            Console.WriteLine("Day 16: Flawed Frequency Transmission" + Environment.NewLine);

            //Setup
            string input = File.ReadAllText(file);
            List<int> initial = new List<int>();
            foreach(char c in input) {
                int num = (int)char.GetNumericValue(c);
                initial.Add(num);
            }
            int timesB = 10000;
            int[] arrayA = initial.ToArray();
            int[] arrayB = new int[arrayA.Length * timesB];
            for (int t = 0; t < timesB; t++)
                Array.Copy(arrayA, 0, arrayB, arrayA.Length * t, arrayA.Length);

            //Part 1
            Solve(arrayA);
            string partA = string.Join("", arrayA.Take(8));

            //Part 2
            int offsetB = int.Parse(input.Substring(0, 7));
            Solve(arrayB, offsetB);
            string partB = string.Join("", arrayB.Skip(offsetB).Take(8));
            if (partB.Length < 7)
                partB = "(this input doesn't work here)";

            Console.WriteLine("Part 1: " + partA);
            //Answer: 90744714
            Console.WriteLine("Part 2: " + partB);
            //Answer: 82994322
        }

        private static void Solve(int[] array, int offset = 0) {
            int phaseLimit = 100;
            for (int p = 1; p <= phaseLimit; p++) {
                if (offset == 0) {
                    //Part 1
                    for (int i = offset; i < array.Length; i++) {
                        int v = 0; //Running total
                        int k = i; //Starting digit, skip the first
                        int repeat = 1; //Cycle of [0, 1, 0, -1] starting at 1
                        while (k < array.Length) {
                            if (repeat == 0) {
                                //Skip as it's 0
                            } else if (repeat == 1) {
                                //Add as it's 1
                                v += array.Skip(k).Take(i + 1).Sum();
                            } else if (repeat == 2) {
                                //Skip as it's 0
                            } else if (repeat == 3) {
                                //Subtract as it's -1
                                v -= array.Skip(k).Take(i + 1).Sum();
                            }
                            k += i + 1;
                            repeat = (repeat + 1) % 4;
                        }
                        array[i] = Math.Abs(v) % 10;
                    }
                } else {
                    //Part 2, this is probably considered an exploit.
                    int prev = 0;
                    for(int i = array.Length - 1; i >= offset; i--) {
                        int current = array[i];
                        array[i] = Math.Abs(prev + current) % 10;
                        prev = array[i];
                    }
                }
            }
        }
    }
}
