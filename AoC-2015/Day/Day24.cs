namespace AoC.Day {
    public class Day24
    {
        public static void Run(string file) {
            Console.WriteLine("Day 24: It Hangs in the Balance" + Environment.NewLine);

            List<int> packages = new List<int>();

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                int weight = int.Parse(line);
                packages.Add(weight);
            }

            int total = packages.Sum(x => x);
            int each3 = total / 3;
            int each4 = total / 4;
            int max = packages.Max();

            //Find which numbers are needed to sum "each".
            List<int[]> groups3 = new List<int[]>();
            List<int[]> groups4 = new List<int[]>();
            Recursive(packages, groups3, new List<int>(), each3, max);
            Recursive(packages, groups4, new List<int>(), each4, max);
            groups3 = groups3.OrderBy(x => x.Length).ToList();
            groups4 = groups4.OrderBy(x => x.Length).ToList();

            //Find out which groups fit together to sum the total.
            Console.WriteLine("Beep boop...");
            long partA = FindLowestQuantumEntanglement(packages, groups3, each3, false);
            long partB = FindLowestQuantumEntanglement(packages, groups4, each4, true);

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 11846773891
            Console.WriteLine("Part 2: " + partB);
            //Answer: 80393059
        }

        private static void Recursive(List<int> packages, List<int[]> combinations, List<int> inProgress, int reqTotal, int upper) {
            for (int y = upper; y > 0; y--) {
                if (!packages.Contains(y) || inProgress.Contains(y) || inProgress.Sum() + y > reqTotal)
                    continue;
                inProgress.Add(y);
                if (inProgress.Sum() == reqTotal) {
                    int[] clone = inProgress.ToArray();
                    combinations.Add(clone);
                } else {
                    Recursive(packages, combinations, inProgress, reqTotal, y - 1);
                }
                inProgress.Remove(y);
            }
        }

        private static long FindLowestQuantumEntanglement(List<int> packages, List<int[]> groups, int each, bool isPart2) {
            int smallestGroupSize = groups.Min(x => x.Length);

            long lowestQE = long.MaxValue;
            List<int[]> best = new List<int[]>();

            for (int first = 0; first < groups.Count; first++) {
                if (groups[first].Length > smallestGroupSize)
                    continue;
                long qe = groups[first][0];
                for (int x = 1; x < groups[first].Length; x++)
                    qe *= groups[first][x];
                if (qe > lowestQE)
                    continue;

                //Console.WriteLine("## " + string.Join(' ', groups[first]));
                for (int second = 0; second < groups.Count; second++) {
                    if (second == first || groups[first].Intersect(groups[second]).Any())
                        continue;
                    if (!isPart2) {
                        //Part 1
                        IEnumerable<int> union12 = groups[first].Union(groups[second]);
                        int[] except = packages.Except(union12).ToArray();
                        if (except.Sum() == each) {
                            lowestQE = qe;
                            best.Clear();
                            best.Add(groups[first]);
                            best.Add(groups[second]);
                            best.Add(except);
                            //Console.WriteLine("{0} (QE= {3}) .. {1} .. {2}", string.Join(' ', groups[first]), string.Join(' ', groups[second]), string.Join(' ', except), qe);
                            break;
                        }
                    } else {
                        //Part 2
                        for (int third = 0; third < groups.Count; third++) {
                            if (third == first || third == second)
                                continue;
                            if (groups[third].Intersect(groups[first]).Any() || groups[third].Intersect(groups[second]).Any())
                                continue;
                            IEnumerable<int> union123 = groups[first].Union(groups[second]).Union(groups[third]);
                            int[] except = packages.Except(union123).ToArray();
                            if (except.Sum() == each) {
                                lowestQE = qe;
                                best.Clear();
                                best.Add(groups[first]);
                                best.Add(groups[second]);
                                best.Add(groups[third]);
                                best.Add(except);
                                break;
                            }
                        }
                    }
                }
            }

            if (lowestQE == long.MaxValue)
                return 0;
            best[best.Count-1] = best.Last().OrderDescending().ToArray();

            Console.WriteLine("{0} (QE = {4}) .. {1} .. {2}{3}",
                string.Join(' ', best[0]),
                string.Join(' ', best[1]),
                string.Join(' ', best[2]),
                (isPart2 ? " .. " + string.Join(' ', best[3]) : ""),
                lowestQE);

            return lowestQE;
        }
    }
}
