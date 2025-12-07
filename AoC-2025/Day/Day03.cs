
namespace AoC.Day
{
    public class Day03
    {
        public static void Run(string file) {
            Console.WriteLine("Day 3: Lobby" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            int partA = 0;
            long partB = 0;
            foreach (string line in lines) {
                Dictionary<int, int> dNums = new Dictionary<int, int>();
                for(int pos = 0; pos < line.Length; pos++) {
                    dNums.Add(pos, (int)char.GetNumericValue(line[pos]));
                }

                partA += FindA(dNums);
                partB += FindB(dNums, false);
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 17376
            Console.WriteLine("Part 2: " + partB);
            //Answer: 172119830406258
        }

        private static int FindA(Dictionary<int, int> dNums) {
            KeyValuePair<int, int> high1 = dNums.SkipLast(1).OrderByDescending(n => n.Value).First();
            KeyValuePair<int, int> high2 = dNums.Skip(1).Where(k => k.Key > high1.Key).OrderByDescending(n => n.Value).First();
            int combo = (high1.Value * 10) + high2.Value;
            return combo;
        }

        private static long FindB(Dictionary<int, int> dNums, bool verbose) {
            Dictionary<int, int> gathered = new Dictionary<int, int>();
            Dictionary<int, int> lastPossible = dNums.TakeLast(12).ToDictionary();

            int posHighest = -1;
            foreach(KeyValuePair<int, int> pair in lastPossible) {
                if(verbose)
                    Console.Write("Looking at {0} (at {1}) : ", pair.Value, pair.Key);

                bool foundBetter = false;

                for (int num = 9; num >= pair.Value; num--) {
                    var these = dNums.Where(n => n.Value == num && n.Key < pair.Key && n.Key > posHighest).Except(gathered);
                    if (these.Any()) {
                        var first = these.First();
                        if (verbose)
                            Console.Write("{0} (at {1})", first.Value, first.Key);
                        gathered.Add(first.Key, first.Value);
                        foundBetter = true;
                        posHighest = first.Key;
                        break;
                    }
                }

                if (!foundBetter) {
                    gathered.Add(pair.Key, pair.Value);
                    posHighest = pair.Key;
                }

                if (verbose)
                    Console.WriteLine();
            }

            string str = string.Join("", gathered.Values);
            if (verbose)
                Console.WriteLine(str + "\r\n");
            long combo = long.Parse(str);
            return combo;
        }
    }
}
