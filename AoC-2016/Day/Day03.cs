namespace AoC.Day {
    public class Day03
    {
        public static void Run(string file) {
            Console.WriteLine("Day 3: Squares With Three Sides" + Environment.NewLine);

            List<int> num = new List<int>();

            string[] lines = File.ReadAllLines(file);
            int valid_A = 0;
            int valid_B = 0;
            foreach (string line in lines) {
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int[] numInLine = Array.ConvertAll(parts, int.Parse);
                num.AddRange(numInLine);
            }

            //Part 1
            for(int i = 0; i < num.Count; i += 3) {
                int[] tri = { num[i], num[i+1], num[i+2] };
                Array.Sort(tri);
                if (tri[0] + tri[1] > tri[2]) {
                    //Console.WriteLine("(A) valid: " + string.Join(' ', tri));
                    valid_A++;
                }
            }
            //Console.WriteLine();

            //Part 2
            if (num.Count > 6) {
                for (int x = 0; x < 3; x++) {
                    for (int i = x; i < num.Count; i += 9) {
                        int[] tri = { num[i + 0], num[i + 3], num[i + 6] };
                        Array.Sort(tri);
                        if (tri[0] + tri[1] > tri[2]) {
                            //Console.WriteLine("(B) valid: " + string.Join(' ', tri));
                            valid_B++;
                        }
                    }
                }
            }

            //Console.WriteLine();
            Console.WriteLine("Part 1: " + valid_A);
            //Answer: 869
            Console.WriteLine("Part 2: " + valid_B);
            //Answer: 1544
        }
    }
}
