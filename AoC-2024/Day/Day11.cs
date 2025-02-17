namespace AoC.Day {
    public class Day11
    {
        public static void Run(string file) {
            Console.WriteLine("Day 11: Plutonian Pebbles" + Environment.NewLine);

            string[] input = File.ReadAllText(file).Split(' ');

            Dictionary<long, long> stones = new Dictionary<long, long>();
            foreach(string i in input) {
                long num = long.Parse(i);
                if (stones.ContainsKey(num))
                    stones[num]++;
                else
                    stones[num] = 1;
            }

            int blink = 0;
            int blinks = 25;
            for(; blink < blinks; blink++) {
                stones = Blink(stones);
                long count = stones.Sum(x => x.Value);
                Console.WriteLine("{0} blinks has {1} stones", blink + 1, count);
            }
            long partA = stones.Sum(x => x.Value);

            Console.WriteLine();

            blinks = 75;
            for(; blink < blinks; blink++) {
                Console.Write(" {0}", blink + 1);
                stones = Blink(stones);
                //long count = stones.Sum(x => x.Value);
            }
            long partB = stones.Sum(x => x.Value);

            Console.WriteLine("\r\n");
            Console.WriteLine("Part 1: " + partA);
            //Answer: 212655
            Console.WriteLine("Part 2: " + partB);
            //Answer: 253582809724830
        }

        private static Dictionary<long, long> Blink(Dictionary<long, long> stones) {
            Dictionary<long, long> after = new Dictionary<long, long>();

            foreach(KeyValuePair<long, long> magicStone in stones) {
                string st = magicStone.Key.ToString();

                long newNum1 = -1;
                long newNum2 = -1;

                if (magicStone.Key == 0) {
                    newNum1 = 1;
                } else if(st.Length % 2 == 0) {
                    newNum1 = long.Parse(st.Substring(0, st.Length / 2));
                    newNum2 = long.Parse(st.Substring(st.Length / 2));
                } else {
                    newNum1 = magicStone.Key * 2024;
                }

                //--

                if (after.ContainsKey(newNum1))
                    after[newNum1] += magicStone.Value;
                else
                    after[newNum1] = magicStone.Value;

                if (newNum2 > -1) {
                    if (after.ContainsKey(newNum2))
                        after[newNum2] += magicStone.Value;
                    else
                        after[newNum2] = magicStone.Value;
                }
            }

            return after;
        }
    }
}
