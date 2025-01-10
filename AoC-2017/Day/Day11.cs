namespace AoC.Day
{
    public class Day11
    {
        public static void Run(string file) {
            Console.WriteLine("Day 11: Hex Ed" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                //There's only one line in the real input.
                //Console.WriteLine(line);

                string[] path = line.Split(',');

                //Cube coordinates
                int q = 0; 
                int r = 0;
                int s = 0;
                int furthest = 0;
                foreach (string p in path) {
                    switch(p) {
                        case "n": s++; r--;  break;
                        case "ne": q++; r--; break;
                        case "nw": q--; s++; break;
                        case "sw": q--; r++; break;
                        case "se": q++; s--; break;
                        case "s": s--; r++; break;
                    }

                    int d = (Math.Abs(q) + Math.Abs(r) + Math.Abs(s)) / 2;
                    furthest = Math.Max(furthest, d);
                }

                int distance = (Math.Abs(q) + Math.Abs(r) + Math.Abs(s)) / 2;

                Console.WriteLine("Part 1: " + distance);
                //Answer: 818
                Console.WriteLine("Part 2: " + furthest);
                //Answer: 1596
                Console.WriteLine();
            }
        }
    }
}
