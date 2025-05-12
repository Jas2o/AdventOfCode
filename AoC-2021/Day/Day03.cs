using System.Text;

namespace AoC.Day
{
    public class Day03
    {
        public static void Run(string file) {
            Console.WriteLine("Day 3: Binary Diagnostic" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            bool outputText = (lines.Length < 20);
            int len = lines[0].Length;

            int partA = SolveA(lines, len);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Oxygen generator rating");
            Console.ResetColor();
            int oxygen = SolveB(lines, len, true, outputText);
            Console.WriteLine(oxygen);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\r\nCO2 scrubber rating");
            Console.ResetColor();
            int scrubber = SolveB(lines, len, false, outputText);
            Console.WriteLine(scrubber);

            int partB = oxygen * scrubber;

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 1131506
            Console.WriteLine("Part 2: " + partB);
            //Answer: 7863147
        }

        private static int SolveA(string[] lines, int len) {
            int[] aCount1 = new int[len];
            int[] aCount0 = new int[len];
            foreach (string line in lines) {
                for (int i = 0; i < len; i++) {
                    if (line[i] == '1')
                        aCount1[i]++;
                    else
                        aCount0[i]++;
                }
            }

            StringBuilder sbEpsilon = new StringBuilder();
            StringBuilder sbGamma = new StringBuilder();
            for (int i = 0; i < len; i++) {
                if (aCount1[i] > aCount0[i]) {
                    sbEpsilon.Append('1');
                    sbGamma.Append('0');
                } else {
                    sbEpsilon.Append('0');
                    sbGamma.Append('1');
                }
            }
            string sEpsilon = sbEpsilon.ToString();
            string sGamma = sbGamma.ToString();

            int epsilon = Convert.ToInt32(sEpsilon, 2);
            int gamma = Convert.ToInt32(sbGamma.ToString(), 2);

            int partA = epsilon * gamma;
            return partA;
        }

        private static int SolveB(string[] array, int len, bool isOne, bool outputText) {
            int value = 0;

            List<string> list = array.ToList();
            for (int i = 0; i < len; i++) {
                int bCount1 = 0;
                int bCount0 = 0;
                foreach (string line in list) {
                    if (line[i] == '1')
                        bCount1++;
                    else
                        bCount0++;
                }

                char oxykeep = (bCount1 > bCount0 ? '1' : '0');
                char scrkeep = (bCount1 < bCount0 ? '1' : '0');
                if (bCount1 == bCount0) {
                    oxykeep = '1';
                    scrkeep = '0';
                }

                List<string> listNext = new List<string>();
                foreach (string line in list) {
                    if (isOne && line[i] == oxykeep)
                        listNext.Add(line);
                    else if (!isOne && line[i] == scrkeep)
                        listNext.Add(line);
                }
                list = listNext;

                if (outputText)
                    Console.WriteLine("~ " + string.Join(',', list));

                if (list.Count == 1) {
                    value = Convert.ToInt32(list[0], 2);
                    break;
                }
            }

            return value;
        }
    }
}
