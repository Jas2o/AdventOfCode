namespace AoC.Day {
    public class Day14
    {
        public static void Run(string file) {
            Console.WriteLine("Day 14: Space Stoichiometry" + Environment.NewLine);

            //Setup
            Dictionary<string, Reaction> reactions = new Dictionary<string, Reaction>();
            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                string[] side = line.Split(" => ");

                string[] leftparts = side[0].Split(", ");
                List<(string, int)> left = new List<(string, int)>();
                foreach (string leftpart in leftparts) {
                    string[] parts = leftpart.Split(' ');
                    left.Add((parts[1], int.Parse(parts[0])));
                }

                string[] right = side[1].Split(' ');
                Reaction react = new Reaction(right[1], int.Parse(right[0]), left.ToArray());
                reactions.Add(react.OutputName, react);
            }

            //Part 1
            long partA = 0;
            Solve(true, reactions, 1, ref partA);

            //Part 2 using bisection method
            long partB = 0, oreResultB = 0;
            if (partA > 10000) {
                long max = 1000000000000;
                long low = max / partA;
                long high = low * 2;
                //long high = (long)(low * 1.5d); //Probably a safer choice
                while (low <= high) {
                    foreach (Reaction r in reactions.Values)
                        r.Reset();

                    long mid = (long)Math.Floor((high + low) / 2d);
                    long ore = 0;
                    Solve(false, reactions, mid, ref ore);

                    if (ore < 0) {
                        //Not ideal, but can happen with an example and not using 1.5.
                        high = mid;
                        continue;
                    } else if (ore <= max) {
                        partB = mid;
                        oreResultB = ore;
                        if (ore == max)
                            break;
                    }

                    if (ore < max)
                        low = mid + 1;
                    else
                        high = mid - 1;
                }
            }

            Console.WriteLine("Part 1: {0} (ORE needed for 1 FUEL)", partA);
            //Answer: 158482
            if (oreResultB > 0) {
                Console.WriteLine("Part 2: {0} FUEL (would use {1} ORE)", partB, oreResultB);
                //Answer: 7993831
            } else {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Part 2: not calculated.");
                Console.ResetColor();
            }
        }

        private static void Solve(bool verbose, Dictionary<string, Reaction> reactions, long fuels, ref long oreUsed) {
            if (verbose)
                Console.WriteLine("Need: {0} of {1}", "FUEL", fuels);

            SubCheck(verbose, 1, reactions, "FUEL", fuels);

            int maxLevel = reactions.Max(c => c.Value.Level);
            for (int lvl = 1; lvl <= maxLevel; lvl++) {
                IEnumerable<KeyValuePair<string, Reaction>> thislevel = reactions.Where(c => c.Value.Level == lvl);
                foreach (KeyValuePair<string, Reaction> required in thislevel) {
                    Produce(required.Value, ref oreUsed);
                }
            }

            if (verbose) {
                Console.WriteLine();
                foreach (KeyValuePair<string, Reaction> c in reactions)
                    Console.WriteLine(c.Value);
                Console.WriteLine();
            }
        }

        private static void SubCheck(bool verbose, int depth, Dictionary<string, Reaction> reactions, string neededName, long neededAmount) {
            string spacing = (verbose ? "\t".PadLeft(depth, '\t') : string.Empty);

            Reaction step = reactions[neededName];
            long rem = neededAmount - step.Spare;
            long reactNum = (long)Math.Ceiling(rem / step.OutAmount);
            long reactResult = (long)(step.OutAmount * reactNum);
            step.Spare = (int)(reactResult - rem);
            double multiplier = Math.Ceiling(rem / step.OutAmount);

            if (verbose) {
                if (step.Spare > 0) {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(spacing + "There's {0} spares", step.Spare);
                    Console.ResetColor();
                }
                Console.WriteLine(spacing + "Still need: {0} of {1}", rem, neededName);
                Console.WriteLine(spacing + "{0} reactions will produce: {1}", reactNum, reactResult);
                Console.WriteLine(spacing + "leaving: {0} to spare", step.Spare);
            }

            foreach ((string, int) required in step.Inputs) {
                if(required.Item1 == "ORE") {
                    continue;
                } else {
                    long amount = (long)(required.Item2 * multiplier);
                    if (verbose)
                        Console.WriteLine(spacing + "Need: {0} of {1}", amount, required.Item1);
                    SubCheck(verbose, depth + 1, reactions, required.Item1, amount);
                    reactions[required.Item1].Needed += amount;
                }
            }
        }

        private static void Produce(Reaction step, ref long oreUsed) {
            if(step.InputIsOre) {
                long reactNum = (long)Math.Ceiling(step.Needed / step.OutAmount);
                oreUsed += step.AmountForOre * reactNum;
            }
        }

        private class Reaction {
            public string OutputName;
            public double OutAmount;
            public (string, int)[] Inputs;
            public int Level;

            public bool InputIsOre;
            public int AmountForOre;

            public int Spare;
            public long Needed;

            public Reaction(string name, int amount, (string, int)[] values) {
                OutputName = name;
                OutAmount = amount;
                Inputs = values;
                Level = Inputs.Count();
                InputIsOre = values.Any(v => v.Item1 == "ORE");
                if (InputIsOre)
                    AmountForOre = values.First().Item2;
                else
                    AmountForOre = 0;
                Spare = 0;
                Needed = 0;
            }

            public void Reset() {
                Spare = 0;
                Needed = 0;
            }

            public override string ToString() {
                return string.Format("{0} = lv {1}, needed {2}, rate {3}", OutputName, Level, Needed, OutAmount);
            }
        }
    }
}
