namespace AoC.Day
{
    public class Day19
    {
        public static void Run(string file) {
            Console.WriteLine("Day 19: Beacon Scanner" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            Dictionary<int, Scanner> scanners = new Dictionary<int, Scanner>();
            Scanner current = new Scanner(-1);
            foreach (string line in lines) {
                if (line.Length == 0)
                    continue;
                else if (line.StartsWith("--")) {
                    if (current.Beacons.Any())
                        scanners.Add(scanners.Count, current); //current.ID

                    string[] parts = line.Split(' ');
                    int num = int.Parse(parts[2]);
                    current = new Scanner(num);
                } else {
                    string[] parts = line.Split(',');
                    int[] nums = Array.ConvertAll(parts, int.Parse);
                    if (nums.Length == 2)
                        current.Beacons.Add((nums[0], nums[1], 0));
                    else
                        current.Beacons.Add((nums[0], nums[1], nums[2]));
                }
            }
            if (current.Beacons.Any())
                scanners.Add(scanners.Count, current);

            //Part 1
            HashSet<(int, int, int)> beacons = new HashSet<(int, int, int)>();
            scanners[0].PositionKnown = true;
            foreach (var b in scanners[0].Beacons)
                beacons.Add(b);

            Queue<Scanner> queue = new Queue<Scanner>();
            for (int i = 1; i < scanners.Count; i++) {
                scanners[i].UpdatePermutations();
                queue.Enqueue(scanners[i]);
            }

            int minOverlap = (beacons.Count > 12 ? 12 : 3);
            int solved = 0;
            while(queue.Any()) {
                Scanner scanner = queue.Dequeue();
                Dictionary<(int, int, int, int), int> test = new Dictionary<(int, int, int, int), int>(); //x, y, z, perm
                foreach (KeyValuePair<int, List<(int, int, int)>> perm in scanner.BeaconPermutations) {
                    foreach ((int, int, int) b in perm.Value) {
                        foreach ((int, int, int) other in beacons) {
                            int x = other.Item1 - b.Item1;
                            int y = other.Item2 - b.Item2;
                            int z = other.Item3 - b.Item3;

                            (int, int, int, int) key = (x, y, z, perm.Key);
                            if (test.ContainsKey(key))
                                test[key]++;
                            else
                                test[key] = 1;
                        }
                    }
                }

                KeyValuePair<(int, int, int, int), int> max = test.MaxBy(t => t.Value);
                if (max.Value < minOverlap) {
                    queue.Enqueue(scanner);
                    continue;
                }

                scanner.X = max.Key.Item1;
                scanner.Y = max.Key.Item2;
                scanner.Z = max.Key.Item3;
                scanner.PermNum = max.Key.Item4;
                scanner.SolveOrder = ++solved;
                scanner.PositionKnown = true;
                scanner.Beacons = scanner.BeaconPermutations[scanner.PermNum];
                foreach(var b in scanner.Beacons)
                    beacons.Add((b.Item1 + scanner.X, b.Item2 + scanner.Y, b.Item3 + scanner.Z));
            }

            int partA = beacons.Count();
            foreach (KeyValuePair<int, Scanner> pair in scanners) {
                Scanner scanner = pair.Value;
                Console.WriteLine("Scanner {0,2} (v{1,2}, s{5,2}) at {2},{3},{4}", scanner.ID, scanner.PermNum, scanner.X, scanner.Y, scanner.Z, scanner.SolveOrder);
            }
            Console.WriteLine();

            //Part 2
            int partB = 0;
            foreach (KeyValuePair<int, Scanner> sA in scanners) {
                Scanner scannerA = sA.Value;
                foreach (KeyValuePair<int, Scanner> sB in scanners) {
                    if (sA.Key == sB.Key)
                        continue;
                    Scanner scannerB = sB.Value;
                    int manhattan = Math.Abs(scannerA.X - scannerB.X) + Math.Abs(scannerA.Y - scannerB.Y) + Math.Abs(scannerA.Z - scannerB.Z);
                    if (manhattan > partB) {
                        Console.WriteLine("Scanner {0} and {1} are {2} units apart.", scannerA.ID, scannerB.ID, manhattan);
                        partB = manhattan;
                    }
                }
            }
            Console.WriteLine();

            Console.WriteLine("Part 1: " + partA);
            //Answer: 362
            Console.WriteLine("Part 2: " + partB);
            //Answer: 12204
        }

        private class Scanner {
            public int ID;
            public List<(int, int, int)> Beacons;
            public Dictionary<int, List<(int, int, int)>> BeaconPermutations;
            public bool PositionKnown;
            public int X;
            public int Y;
            public int Z;
            public int PermNum;
            public int SolveOrder;

            public Scanner(int num) {
                ID = num;
                Beacons = new List<(int, int, int)>();
                BeaconPermutations = new Dictionary<int, List<(int, int, int)>>();
                BeaconPermutations[0] = Beacons;
                PermNum = 0;
                SolveOrder = 0;
            }

            public void UpdatePermutations() {
                for (int p = 1; p <= 24; p++)
                    BeaconPermutations[p] = new List<(int, int, int)>();

                foreach((int, int, int) set in Beacons) {
                    //BeaconPermutations[0].Add((set.Item1, set.Item2, set.Item3));
                    BeaconPermutations[1].Add((set.Item3, set.Item2, -set.Item1));
                    BeaconPermutations[2].Add((-set.Item1, set.Item2, -set.Item3));
                    BeaconPermutations[3].Add((-set.Item3, set.Item2, set.Item1));
                    BeaconPermutations[4].Add((set.Item1, -set.Item3, set.Item2));
                    BeaconPermutations[5].Add((set.Item2, -set.Item3, -set.Item1));
                    BeaconPermutations[6].Add((-set.Item1, -set.Item3, -set.Item2));
                    BeaconPermutations[7].Add((-set.Item2, -set.Item3, set.Item1));
                    BeaconPermutations[8].Add((set.Item1, -set.Item2, -set.Item3));
                    BeaconPermutations[9].Add((-set.Item3, -set.Item2, -set.Item1));
                    BeaconPermutations[10].Add((-set.Item1, -set.Item2, set.Item3));
                    BeaconPermutations[11].Add((set.Item3, -set.Item2, set.Item1));
                    BeaconPermutations[12].Add((set.Item1, set.Item3, -set.Item2));
                    BeaconPermutations[13].Add((-set.Item2, set.Item3, -set.Item1));
                    BeaconPermutations[14].Add((-set.Item1, set.Item3, set.Item2));
                    BeaconPermutations[15].Add((set.Item2, set.Item3, set.Item1));
                    BeaconPermutations[16].Add((-set.Item3, set.Item1, -set.Item2));
                    BeaconPermutations[17].Add((-set.Item2, set.Item1, set.Item3));
                    BeaconPermutations[18].Add((set.Item3, set.Item1, set.Item2));
                    BeaconPermutations[19].Add((set.Item2, set.Item1, -set.Item3));
                    BeaconPermutations[20].Add((set.Item3, -set.Item1, -set.Item2));
                    BeaconPermutations[21].Add((-set.Item2, -set.Item1, -set.Item3));
                    BeaconPermutations[22].Add((-set.Item3, -set.Item1, set.Item2));
                    BeaconPermutations[23].Add((set.Item2, -set.Item1, set.Item3));
                }
            }
        }
    }
}
