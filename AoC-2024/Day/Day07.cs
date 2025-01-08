using AoC.Shared;
using System.Text;

namespace AoC.Day
{
    public class Day07 {
        private static bool verbose = true;

        private static bool tmi;
        private static bool quitEarly;

        public static void Run(string file) {
            Console.WriteLine("Day 7: Bridge Repair" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            tmi = (lines.Length < 100);
            quitEarly = !tmi;

            long partA = CheckForBase(lines, 2);
            Console.WriteLine("Part 1: " + partA);
            //Answer: 5030892084481

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("* intense calculator time *");
            Console.ResetColor();
            Console.WriteLine();

            long partB = CheckForBase(lines, 3);
            Console.WriteLine("Part 2: " + partB);
            //Answer: 91377448644679
        }

        private static long CheckForBase(string[] lines, int numBase) {
            long supertotal = 0;

            foreach (string line in lines) {
                bool solved = false;

                long total = long.Parse(line.Substring(0, line.IndexOf(':')));
                int[] values = Array.ConvertAll(line.Substring(line.IndexOf(' ') + 1).Split(' '), int.Parse);

                int bitplaces = values.Length - 1;
                int possible = (int)Math.Pow(numBase, bitplaces);
                for (int ops = 0; ops < possible; ops++) {
                    //string binary = Convert.ToString(ops, numBase).PadLeft(bitplaces, '0');
                    string mask = Number.IntToString(ops, numBase).PadLeft(bitplaces, '0');

                    StringBuilder vtext = new StringBuilder(values[0].ToString());

                    long testtotal = values[0];
                    for (int y = 0; y < bitplaces; y++) {
                        if (mask[y] == '0') {
                            testtotal += values[y + 1];
                            if (verbose)
                                vtext.Append(" + " + values[y + 1]);
                        } else if (mask[y] == '1') {
                            testtotal *= values[y + 1];
                            if (verbose)
                                vtext.Append(" x " + values[y + 1]);
                        } else if (mask[y] == '2') {
                            //Concatenate
                            testtotal = long.Parse(testtotal.ToString() + values[y + 1].ToString());
                            if(verbose)
                                vtext.Append(values[y + 1].ToString());
                        }
                    }
                    bool testcorrect = testtotal == total;

                    if (verbose && (testcorrect || tmi)) {
                        Console.Write("{2} :: {0} = {1}", vtext.ToString(), total, mask);
                    }

                    if (testcorrect) {
                        solved = true;

                        if (verbose) {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" Correct");
                            Console.ResetColor();

                            //if (quitEarly) Console.WriteLine();
                        }

                        if(quitEarly)
                            break;
                    } else {
                        if (verbose && tmi) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(" actually " + testtotal);
                            Console.ResetColor();
                        }
                    }

                    if (verbose && (testcorrect || tmi)) {
                        Console.WriteLine();
                    }
                }

                if (solved) {
                    supertotal += total;
                    if (verbose)
                        Console.WriteLine();
                } else {
                    if (verbose && tmi)
                        Console.WriteLine();
                }
            }

            return supertotal;
        }
    }
}