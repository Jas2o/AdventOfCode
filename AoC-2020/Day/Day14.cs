using AoC.Shared;

namespace AoC.Day
{
    public class Day14
    {
        public static void Run(string file) {
            Console.WriteLine("Day 14: Docking Data" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            bool print = (lines.Length < 10);
            long partA = SolveA(lines, print);
            long partB = SolveB(lines, print);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 12408060320841
            Console.WriteLine("Part 2: " + partB);
            //Answer: 4466434626828
        }

        private static long SolveA(string[] lines, bool outputText) {
            Dictionary<int, char> mask = new Dictionary<int, char>();
            Dictionary<int, long> memory = new Dictionary<int, long>();

            foreach (string line in lines) {
                string[] fields = line.Split(" = ");
                if (fields[0] == "mask") {
                    for (int i = 0; i < 36; i++) {
                        char c = fields[1][i];
                        if (c == '0')
                            mask[35 - i] = '0';
                        else if (c == '1')
                            mask[35 - i] = '1';
                        else
                            mask.Remove(35 - i);
                    }
                } else {
                    int address = int.Parse(fields[0].Substring(4, fields[0].Length - 5));
                    long num = long.Parse(fields[1]);
                    char[] c = Convert.ToString(num, 2).PadLeft(36, '0').ToArray();
                    foreach (var pair in mask)
                        c[35 - pair.Key] = pair.Value;
                    num = Convert.ToInt64(new string(c), 2);

                    memory[address] = num;
                }
            }

            if (outputText) {
                foreach (var pair in memory.OrderBy(m => m.Key))
                    Console.WriteLine("{0} = {1}", pair.Key, pair.Value);
                Console.WriteLine();
            }

            return memory.Values.Sum();
        }

        private static long SolveB(string[] lines, bool outputText) {
            Dictionary<int, char> mask = new Dictionary<int, char>();
            Dictionary<long, long> memory = new Dictionary<long, long>();

            Dictionary<int, char> temp = new Dictionary<int, char>();

            foreach (string line in lines) {
                string[] fields = line.Split(" = ");
                if (fields[0] == "mask") {
                    for (int i = 0; i < 36; i++) {
                        char c = fields[1][i];
                        if (c == '0')
                            mask.Remove(35 - i);
                        else if (c == '1')
                            mask[35 - i] = '1';
                        else
                            mask[35 - i] = 'X';
                    }
                } else {
                    int addressOrg = int.Parse(fields[0].Substring(4, fields[0].Length - 5));
                    int writeNum = int.Parse(fields[1]);

                    string addressBin = Convert.ToString(addressOrg, 2).PadLeft(36, '0');
                    //Convert the address to something indexed we can modify.
                    for(int i = 0; i < 36; i++)
                        temp[i] = addressBin[i];
                    //Apply the mask
                    foreach (var pair in mask)
                        temp[35 - pair.Key] = pair.Value;
                    //Get the positions of the "floating" bits.
                    int[] floating = temp.Where(t => t.Value == 'X').Select(t => t.Key).ToArray();

                    //Fill in each combination and set.
                    int bitplaces = floating.Count();
                    int possible = (int)Math.Pow(2, bitplaces);
                    for (int ops = 0; ops < possible; ops++) {
                        string m = Number.IntToString(ops, 2).PadLeft(bitplaces, '0');
                        for(int b = 0; b < bitplaces; b++) {
                            int f = floating[b];
                            temp[f] = m[b];
                        }

                        string str = string.Join("", temp.Values);
                        long address = Convert.ToInt64(str, 2);
                        memory[address] = writeNum;
                    }
                }
            }

            if (outputText) {
                foreach (var pair in memory.OrderBy(m => m.Key))
                    Console.WriteLine("{0} = {1}", pair.Key, pair.Value);
                Console.WriteLine();
            }

            return memory.Values.Sum();
        }
    }
}
