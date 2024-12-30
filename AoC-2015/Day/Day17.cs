using System.Text;

namespace AoC.Day {
    public class Day17
    {
        public static void Run(string file) {
            Console.WriteLine("Day 17: No Such Thing as Too Much" + Environment.NewLine);

            List<int> containers = new List<int>();
            string[] lines = File.ReadAllLines(file);
            for (int i = 0; i < lines.Length; i++) {
                containers.Add(int.Parse(lines[i]));
            }

            int litersOfEggnog = 150;
            if (containers.Count < 10)
                litersOfEggnog = 25;

            List<string> combinations = new List<string>();

            //Part 1
            int ops = 0;
            while(true) {
                ops++;
                string mask = Util.Int32ToString(ops, 2).PadLeft(containers.Count, '0');
                if (mask.Length > containers.Count)
                    break;

                int fills = 0;
                StringBuilder sb = new StringBuilder();
                for (int y = 0; y < mask.Length; y++) {
                    char c = mask[y];
                    if (c == '1') {
                        fills += containers[y];
                        sb.Append(string.Format("{0} ", containers[y]));
                    }
                }

                if (fills == litersOfEggnog) {
                    combinations.Add(mask);
                    Console.WriteLine("{0} {1} fills to {2}", mask, sb.ToString().Trim(), fills);
                }
            }

            //Part 2
            int minContainers = containers.Count;
            int minCount = 0;
            foreach(string combo in combinations) {
                int numContainers = combo.Count(x => x =='1');
                if(numContainers < minContainers) {
                    minContainers = numContainers;
                    minCount = 1;
                } else if(numContainers == minContainers) {
                    minCount++;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + combinations.Count);
            //Answer: 1304
            Console.WriteLine("Part 2: " + minCount);
            //Answer: 18
        }
    }
}
