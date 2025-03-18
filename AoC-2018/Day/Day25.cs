namespace AoC.Day
{
    public class Day25
    {
        public static void Run(string file) {
            Console.WriteLine("Day 25: Four-Dimensional Adventure" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            //Intention is to have multiple inputs in the test file separated by blank lines.
            List<List<FixedPoint>> samples = new List<List<FixedPoint>>();
            if (true) {
                List<FixedPoint> sample = new List<FixedPoint>();
                foreach (string line in lines) {
                    if (line.Contains(',')) {
                        sample.Add(new FixedPoint(line));
                    } else {
                        if (sample.Any()) {
                            samples.Add(sample);
                            sample = new List<FixedPoint>();
                        }
                    }
                }
                if (sample.Any())
                    samples.Add(sample);
            }

            foreach(List<FixedPoint> sample in samples) {
                int partA = 0;

                while (sample.Any(s => s.ConstellationID == 0)) {
                    partA++;
                    FixedPoint first = sample.First(s => s.ConstellationID == 0);
                    List<FixedPoint> constellation = new List<FixedPoint>();
                    constellation.Add(first);

                    bool loop = false;
                    do {
                        loop = false;

                        Queue<FixedPoint> remaining = new Queue<FixedPoint>();
                        foreach (FixedPoint p in sample.Where(s => s.ConstellationID == 0)) {
                            if (constellation.Contains(p))
                                continue;
                            remaining.Enqueue(p);
                        }

                        while (remaining.Any()) {
                            FixedPoint other = remaining.Dequeue();
                            foreach (FixedPoint point in constellation) {
                                int manhattan = Math.Abs(point.W - other.W) + Math.Abs(point.X - other.X) + Math.Abs(point.Y - other.Y) + Math.Abs(point.Z - other.Z);
                                if (manhattan < 4) {
                                    constellation.Add(other);
                                    loop = true;
                                    break;
                                }
                            }
                        }
                    } while (loop);

                    if (partA % 2 == 0)
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    foreach (FixedPoint point in constellation) {
                        point.ConstellationID = partA;
                        Console.WriteLine(point);
                    }
                    Console.ResetColor();
                }

                Console.WriteLine("\r\nPart 1: {0}\r\n", partA);
                //Answer: 352
            }
            Console.WriteLine("Part 2: (N/A)");
			//Answer: (there is no Part 2).
        }

        private class FixedPoint {
            public int W;
            public int X;
            public int Y;
            public int Z;
            public int ConstellationID;

            public FixedPoint(string line) {
                int[] nums = Array.ConvertAll(line.Split(','), int.Parse);
                W = nums[0];
                X = nums[1];
                Y = nums[2];
                Z = nums[3];
                ConstellationID = 0;
            }

            public override string ToString() {
                return string.Format("{0},{1},{2},{3} in {4}", W, X, Y, Z, ConstellationID);
            }
        }
    }
}
