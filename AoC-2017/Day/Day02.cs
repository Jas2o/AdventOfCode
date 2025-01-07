namespace AoC.Day
{
    public class Day02
    {
        public static void Run(string file) {
            Console.WriteLine("Day 2: Corruption Checksum" + Environment.NewLine);

            int partA = 0;
            int partB = 0;

			string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                int[] num = Array.ConvertAll(line.Replace("\t", " ").Split(' '), int.Parse);

                partA += num.Max() - num.Min();

                for(int x = 0; x < num.Length; x++) {
                    for (int y = 0; y < num.Length; y++) {
                        if (x == y)
                            continue;
                        int left = num[x];
                        int right = num[y];
                        if (right == 0)
                            continue;
                        int mod = left % right;
                        if(mod == 0) {
                            partB += left / right;
                            break;
                        }
                    }
                }
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 50376
            Console.WriteLine("Part 2: " + partB);
            //Answer: 267
        }
    }
}
