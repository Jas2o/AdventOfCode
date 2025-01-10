namespace AoC.Day
{
    public class Day13
    {
        public static void Run(string file) {
            Console.WriteLine("Day 13: Packet Scanners" + Environment.NewLine);

            Dictionary<int, Scanner> scanners = new Dictionary<int, Scanner>();
			string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                string[] fields = line.Split(": ");
                scanners.Add(int.Parse(fields[0]), new Scanner(int.Parse(fields[1])));
            }

            int length = scanners.Max(s => s.Key);

            bool verbose = (length < 20);
            int part1severity = FirewallGetSeverity(length, scanners, verbose);

            foreach (KeyValuePair<int, Scanner> pair in scanners) {
                pair.Value.Reset();
            }
            int delay = 0;
            while (true) {
                bool part2caught = FirewallCheckIfCaught(delay, length, scanners);
                if (!part2caught)
                    break;
                delay++;
            }

            Console.WriteLine("Part 1: " + part1severity);
            //Answer: 1704
            Console.WriteLine("Part 2: " + delay);
            //Answer: 3970918
        }

        private class Scanner {
            public int Current;
            public int Range;
            public bool DirUp;

            public Scanner(int range) {
                Current = 1;
                Range = range;
                DirUp = true;
            }

            public void Reset() {
                Current = 1;
                DirUp = true;
            }

            public void Step() {
                if(DirUp)
                    Current++;
                else
                    Current--;

                if(Current > Range) {
                    DirUp = false;
                    Current = Range - 1;
                } else if(Current < 1) {
                    DirUp = true;
                    Current = 2;
                }
            }
        }

        private static int FirewallGetSeverity(int length, Dictionary<int, Scanner> scanners, bool verbose) {
            int severityTotal = 0;
            for (int picosecond = 0; picosecond <= length; picosecond++) {
                if(verbose)
                    Console.WriteLine("Picosecond {0}:", picosecond);

                for (int i = 0; i <= length; i++) {
                    if (verbose && i == picosecond)
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    if (scanners.ContainsKey(i)) {
                        Scanner s = scanners[i];

                        if (picosecond == i && s.Current == 1) {
                            int severity = i * s.Range;
                            severityTotal += severity;
                            if(verbose)
                                Console.WriteLine("Caught, severity: {0}", severity);
                        }

                        if(verbose)
                            Console.WriteLine("{0} : {1} / {2}", i, s.Current, s.Range);
                        s.Step();
                    } else if(verbose) {
                        Console.WriteLine("{0} : ...", i);
                    }

                    if (verbose)
                        Console.ResetColor();
                }

                if (verbose)
                    Console.WriteLine();
            }
            return severityTotal;
        }

        private static bool FirewallCheckIfCaught(int delay, int length, Dictionary<int, Scanner> scanners) {
            for (int i = 0; i <= length; i++) {
                if (scanners.ContainsKey(i)) {
                    int picoseconds = i + delay;
                    int multiple = (2 * scanners[i].Range - 2);
                    if (picoseconds % multiple == 0)
                        return true;
                }
            }
            return false;
        }
    }
}
